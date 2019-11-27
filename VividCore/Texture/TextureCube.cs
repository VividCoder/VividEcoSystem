using OpenTK.Graphics.OpenGL4;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Vivid.Texture
{
    public class TextureCube : TextureBase
    {
        public byte[] px, py, pz, nx, ny, nz;

        public TextureCube(int w, int h, byte[] f0, byte[] f1, byte[] f2, byte[] f3, byte[] f4, byte[] f5)
        {
            W = w;
            H = h;
            D = 1;
            //   GL.ActiveTexture ( TextureUnit.Texture0 );
            //    GenMap ( );
            List<byte[]> ll = new List<byte[]>
            {
                f1 ,
                f2 ,
                f3 ,
                f4 ,
                f5 ,
                f5
            };

            px = f3;
            py = f0;
            pz = f4;
            nx = f1;
            ny = f5;
            nz = f2;

            // GL.TexImage2D ( TextureTarget.TextureCubeMapNegativeZ, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f2 );
            // GL.TexImage2D ( TextureTarget.TextureCubeMapPositiveZ, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f4 );
            // GL.TexImage2D ( TextureTarget.TextureCubeMapNegativeX, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f1 );
            // GL.TexImage2D ( TextureTarget.TextureCubeMapPositiveX, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f3 );
            // GL.TexImage2D ( TextureTarget.TextureCubeMapNegativeY, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f5 );
            // GL.TexImage2D ( TextureTarget.TextureCubeMapPositiveY, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f0 );

            //for (int i = 0; i < 6; i++)
            // {
            //   GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, ll[i]);
            //}
        }

        public void Write(string path)
        {
            var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);

            w.Write((short)W);
            w.Write((short)H);

            WriteFace(px, fs);
            WriteFace(py, fs);
            WriteFace(pz, fs);
            WriteFace(nx, fs);
            WriteFace(ny, fs);
            WriteFace(nz, fs);

            fs.Flush();
            fs.Close();
            fs = null;
        }

        public void WriteFace(byte[] face, FileStream fs)
        {
            if (face.Length != (W * H * 3))
            {
                Console.WriteLine("WriteFace faliure");
                Console.WriteLine("*3=:" + (W * H * 3) + " O:" + face.Length);
                Console.WriteLine("*4=:" + (W * H * 4) + " O:" + face.Length);
                while (true)
                {
                }
            }
            fs.Write(face, 0, face.Length);
        }

        public TextureCube(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            Path = path;
            W = r.ReadInt16();
            H = r.ReadInt16();
            D = 1;

            px = ReadFace(fs);
            py = ReadFace(fs);
            pz = ReadFace(fs);

            nx = ReadFace(fs);
            ny = ReadFace(fs);
            nz = ReadFace(fs);

            int w = W, h = H;

            GL.ActiveTexture(TextureUnit.Texture0);
            GenMap();
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, nz);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, pz);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, nx);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, px);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, ny);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, py);

            fs.Close();
            fs = null;
        }

        public byte[] ReadFace(FileStream fs)
        {
            byte[] rs = new byte[W * H * 3];
            fs.Read(rs, 0, W * H * 3);
            return rs;
        }

        public TextureCube(int w, int h)
        {
            W = w;
            H = h;
            D = 1;
            GL.ActiveTexture(TextureUnit.Texture0);
            GenMap();
            for (int i = 0; i < 6; i++)
            {
                //TextureTarget.
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            }
        }

        public byte[] Dat(string pf)
        {
            Bitmap TexData = new Bitmap(pf);
            W = TexData.Width;
            H = TexData.Height;
            byte[] pixs = new byte[W * H * 4];
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
            return pixs;
        }

        private void GenMap()
        {
            GL.Enable(EnableCap.TextureCubeMap);
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, ID);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        }

        public override void Bind(int texu)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texu);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.BindTexture(TextureTarget.TextureCubeMap, ID);
        }

        public override void Release(int texu)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texu);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            GL.Disable(EnableCap.TextureCubeMap);
        }
    }
}