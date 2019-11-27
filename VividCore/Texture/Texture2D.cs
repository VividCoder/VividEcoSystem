using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace Vivid.Texture
{
    public enum LoadMethod
    {
        Single, Multi
    }

    public enum TextureFormat
    {
        Normal, RGB16F, RGB32F
    }

    public class Texture2D : TextureBase
    {
        public static Dictionary<string, Texture2D> Lut = new Dictionary<string, Texture2D>();
        public static byte[] TmpStore = null;
        public bool Alpha = false;
        public bool Binded = false;
        public bool Loaded = false;
        public Mutex LoadMutex = new Mutex();
        public Thread LoadThread = null;
        public byte[] pixs = null;
        public byte[] RawData;
        public Bitmap TexData = null;
        private readonly bool PreLoaded = false;
        private FileStream nf;

        private BinaryReader nr;

        public string GetName()
        {
            return Path;
        }
        public Texture2D(int w, int h, bool alpha = false)
        {
            GenTex(w, h, alpha);
        }

        public Texture2D(int w, int h, bool alpha, TextureFormat format)
        {
            GenTex(w, h, alpha, format);
        }

        public Texture2D()
        {
        }

        public Texture2D(Pixels.PixelMap pixels)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            Alpha = pixels.Alpha;
            W = pixels.Width;
            H = pixels.Height;
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            if (pixels.Alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.Data);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, W, H, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pixels.Data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
            pixs = pixels.Data;
        }

        public Texture2D(int w, int h, byte[] dat, bool alpha = true)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            Alpha = alpha;
            W = w;
            H = h;
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, w, h, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dat);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, dat);
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
            pixs = dat;
        }

        public void LoadSub(byte[] data)
        {
            GL.BindTexture(TextureTarget.Texture2D, ID);
            if (Alpha)
            {
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, W, H, PixelFormat.Rgba, PixelType.UnsignedByte, data);
            }
            else
            {
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, W, H, PixelFormat.Rgb, PixelType.UnsignedByte, data);
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public bool smallDone = false, bigDone = false;
        public static List<Texture2D> LoadingTexs = new List<Texture2D>();

        //public Thread LoadThread = null;
        //public Mutex LoadMutex = null;
        public int sW, sH;

        public int bW, bH;
        private int wS = 0;

        public static void UpdateLoading()
        {
            foreach (var tex in LoadingTexs)
            {
                if (tex.smallDone)
                {
                    if (!tex.bigDone)
                    {
                        GL.Enable(EnableCap.Texture2D);
                        GL.BindTexture(TextureTarget.Texture2D, tex.ID);

                        if (tex.Alpha)
                        {
                            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, tex.sW, tex.sH, 0, PixelFormat.Rgba, PixelType.UnsignedByte, tex.SmallRawData);
                        }
                        else
                        {
                            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, tex.sW, tex.sH, 0, PixelFormat.Rgb, PixelType.UnsignedByte, tex.SmallRawData);
                        }

                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                        //    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                        GL.BindTexture(TextureTarget.Texture2D, 0);
                    }
                    tex.smallDone = false;
                }

                if (tex.bigDone)
                {
                    GL.Enable(EnableCap.Texture2D);
                    GL.BindTexture(TextureTarget.Texture2D, tex.ID);

                    if (tex.Alpha)
                    {
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, tex.bW, tex.bH, 0, PixelFormat.Rgba, PixelType.UnsignedByte, tex.RawData);
                    }
                    else
                    {
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, tex.bW, tex.bH, 0, PixelFormat.Rgb, PixelType.UnsignedByte, tex.RawData);
                    }

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                    tex.bigDone = true;
                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    LoadingTexs.Remove(tex);
                    return;
                }
            }
        }

        public byte[] SmallRawData;

        public Texture2D(string path, LoadMethod lm, bool alpha = true)
        {
            Load(path, alpha);
        }

        public void Load(string path, bool alpha)
        {
            if (File.Exists(path) == false)
            {
                return;
            }
            if (path == string.Empty || path == "" || path == null)
            {
                return;
            }
            Path = path;
            if (Lut.ContainsKey(path))
            {
                var otex = Lut[path];
                this.ID = otex.ID;
                this.RawData = otex.RawData;
                this.W = otex.W;
                this.H = otex.H;
                this.Path = otex.Path;
                this.Alpha = otex.Alpha;
                this.bigDone = otex.bigDone;
                this.smallDone = otex.smallDone;
                return;
            }

            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ID);

            if (new FileInfo(path + ".texDat").Exists)
            {
                void Load_Tex()
                {
                    SmallRawData = LoadTexData(path + ".texDatSmall");
                    sW = W;
                    sH = H;
                    smallDone = true;
                    RawData = LoadTexData(path + ".texDat");
                    bW = W;
                    bH = H;
                    bigDone = true;
                }

                LoadingTexs.Add(this);

                LoadThread = new Thread(new ThreadStart(Load_Tex));

                LoadThread.Start();
            }
            else
            {
                Bitmap img = null;
                try
                {
                    img = new Bitmap(path);
                    //System.Drawing.Imaging.BitmapData dat = img.LockBits( new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.);
                }
                catch
                {
                    img = new Bitmap(32, 32);
                }
                W = img.Width;
                H = img.Height;
                Alpha = alpha;

                int pc = 3;
                if (Alpha)
                {
                    pc = 4;
                }
                Bitmap small_img = new Bitmap(img, new Size(img.Width / 4, img.Height / 4));

                pc = WriteDat(path, alpha, small_img, pc, true, small_img.Width, small_img.Height);

                pc = WriteDat(path, alpha, img, pc, false, W, H);

                GL.BindTexture(TextureTarget.Texture2D, ID);

                if (Alpha)
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
                }
                else
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, W, H, 0, PixelFormat.Rgb, PixelType.UnsignedByte, RawData);
                }

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                //    GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
            if (!Lut.ContainsKey(Path))
            {
                Lut.Add(Path, this);
            }

        }

        private byte[] LoadTexData(string lTex)
        {
            FileStream fs = new FileStream(lTex, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            Name = r.ReadString();
            r.ReadString();
            W = r.ReadInt16();
            H = r.ReadInt16();
            Alpha = r.ReadBoolean();
            int pc = 3;
            if (Alpha)
            {
                pc = 4;
            }

            byte[] LData = new byte[W * H * (Alpha ? 4 : 3)];
            r.Read(LData, 0, W * H * (Alpha ? 4 : 3));

            fs.Close();
            fs = null;

            return LData;
        }

        private int WriteDat(string path, bool alpha, Bitmap img, int pc, bool small, int ow, int oh)
        {
            RawData = new byte[ow * oh * pc];

            //GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.)

            int pi = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    Color pix = img.GetPixel(x, y);
                    RawData[pi++] = pix.R;
                    RawData[pi++] = pix.G;
                    RawData[pi++] = pix.B;
                    if (alpha)
                    {
                        RawData[pi++] = pix.A;
                    }
                }
            }

            FileStream fs;
            if (small)
            {
                fs = new FileStream(path + ".texDatSmall", FileMode.Create, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(path + ".texDat", FileMode.Create, FileAccess.Write);
            }

            BinaryWriter w = new BinaryWriter(fs);
            if (Name == null || Name == string.Empty)
            {
                Name = "Tex2D";
            }
            w.Write(Name);
            w.Write(Path);
            w.Write((short)ow);
            w.Write((short)oh);
            w.Write(Alpha);
            pc = 3;
            if (alpha)
            {
                pc = 4;
            }

            w.Write(RawData, 0, RawData.Length);
            fs.Flush();
            fs.Close();
            fs = null;
            return pc;
        }

        ~Texture2D()
        {
            //NewMethod();
        }

        public string Name
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public override void Bind(int texu)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            // GL.ClientActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }

        public void BindData()
        {
            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixs);
            GL.Disable(EnableCap.Texture2D);
        }

        public void CopyTex(int x, int y)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, ID);
            GL.CopyTexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, x, y, W, H);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Delete()
        {
            if (ID == -1) return;
            GL.DeleteTexture(ID);
            ID = -1;
        }

        public Vivid.Tex.Tex2D ToTex2D()
        {

            var nt = new Vivid.Tex.Tex2D();
            nt.ID = ID;
            nt.Width = W;
            nt.Height = H;
            nt.Path = Path;
            nt.RawData = RawData;
            nt.Alpha = Alpha;
            return nt;

        }
        public Texture2D(TextureRaw raw)
        {
            RawData = raw.Data;
            Alpha = raw.Alpha;
            W = raw.W;
            H = raw.H;

            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ID);

            if (Alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, W, H, 0, PixelFormat.Rgb, PixelType.UnsignedByte, RawData);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            // GL.GenerateMipmap ( GenerateMipmapTarget.Texture2D );

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void LoadFromString(string path)
        {

        }

        public void Read()
        {
            Alpha = Help.IOHelp.ReadBool();
            Path = Help.IOHelp.ReadString();
            Load(Path, Alpha);

            return;
            W = Help.IOHelp.ReadInt();
            H = Help.IOHelp.ReadInt();
            Alpha = Help.IOHelp.ReadBool();
            RawData = Help.IOHelp.ReadBytes();
            // GL.ActiveTexture(TextureUnit.Texture0);

            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ID);

            if (Alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, W, H, 0, PixelFormat.Rgb, PixelType.UnsignedByte, RawData);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            //  GL.GenerateMipmap ( GenerateMipmapTarget.Texture2D );
            bigDone = true;
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void ReadBitmap(BinaryReader r)
        {
            short bw = r.ReadInt16();
            short bh = r.ReadInt16();
            TexData = new Bitmap(bw, bh);
            pixs = r.ReadBytes(bw * bh * 4);
            W = bw;
            H = bh;
            Alpha = true;
            return;
            for (int y = 0; y < bh; y++)
            {
                for (int x = 0; x < bw; x++)
                {
                    byte[] col = r.ReadBytes(4);
                    System.Drawing.Color nc = System.Drawing.Color.FromArgb(col[3], col[0], col[1], col[2]);
                    TexData.SetPixel(x, y, nc);
                }
            }
            W = bw;
            H = bh;
        }

        public override void Release(int texu)
        {
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            // GL.ClientActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }

        public void SaveTex(string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return;
            }
            if (File.Exists(p))
            {
                return;
            }

            FileStream fs = new FileStream(p, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);

            Bitmap sd = new Bitmap(TexData, 32, 32);
            WriteBitmap(sd, w);
            WriteBitmap(TexData, w);

            fs.Flush();
            fs.Close();
        }

        public void SetPix()
        {
            pixs = new byte[W * H * 4];
            int loc = 0;
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    Color p = TexData.GetPixel(x, y);
                    pixs[loc++] = p.R;
                    pixs[loc++] = p.G;
                    pixs[loc++] = p.B;
                    pixs[loc++] = p.A;
                }
            }
        }

        public void SkipBitmap(BinaryReader r)
        {
            short bw = r.ReadInt16();
            short bh = r.ReadInt16();
            r.BaseStream.Seek(bw * bh * 4, SeekOrigin.Current);
        }

        public void T_LoadTex()
        {
            TexData = new Bitmap(Path);

            W = TexData.Width;
            H = TexData.Height;
            D = 1;
            Alpha = true;
            SetPix();
            SaveTex(Path + ".vtex");
            Loaded = true;
        }

        public void T_LoadVTex()
        {
            ReadBitmap(nr);

            //W = TexData.Width;
            //H = TexData.Height;
            D = 1;
            Alpha = true;
            //SetPix();
            //SaveTex(Path + ".vtex");
            Loaded = true;
            nf.Close();
            nf = null;
            nr = null;
        }

        public void Write()
        {
            Help.IOHelp.WriteBool(Alpha);
            Help.IOHelp.WriteString(Path);


        }

        public void WriteBitmap(Bitmap b, BinaryWriter w)
        {
            w.Write((short)b.Width);
            w.Write((short)b.Height);
            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    Color p = b.GetPixel(x, y);
                    w.Write(p.R);
                    w.Write(p.G);
                    w.Write(p.B);
                    w.Write(p.A);
                }
            }
        }

        private void GenTex(int w, int h, bool alpha, TextureFormat format = TextureFormat.Normal)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            Alpha = alpha;
            W = w;
            H = h;
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            if (alpha)
            {
                switch (format)
                {
                    case TextureFormat.Normal:
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, w, h, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
                        break;
                }
            }
            else
            {
                switch (format)
                {
                    case TextureFormat.Normal:
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
                        break;

                    case TextureFormat.RGB16F:
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb16f, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
                        break;
                }
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
        }
    }
}