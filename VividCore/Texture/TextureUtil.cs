﻿using System.Drawing;
using System.IO;

namespace Vivid.Texture
{
    public class TextureUtil
    {
        public static Bitmap b;
        public static FileStream fs = null;
        public static BinaryWriter wr = null;
        public static BinaryReader br = null;
        public static int fw, fh;

        public static TextureCube LoadCubeMap(string p)
        {
            fs = new FileStream(p, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);
            fw = br.ReadInt32();
            fh = br.ReadInt32();
            byte[] f1 = ReadFace();
            byte[] f2 = ReadFace();
            byte[] f3 = ReadFace();
            byte[] f4 = ReadFace();
            byte[] f5 = ReadFace();
            byte[] f6 = ReadFace();

            TextureCube tc = new TextureCube(fw, fh, f1, f2, f3, f4, f5, f6);

            return tc;
        }

        public static byte[] ReadFace()
        {
            byte[] p = new byte[fw * fh * 3];
            for (int y = 0; y < fh; y++)
            {
                for (int x = 0; x < fw; x++)
                {
                    int l = (y * fw * 3) + x * 3;
                    p[l++] = br.ReadByte();
                    p[l++] = br.ReadByte();
                    p[l] = br.ReadByte();
                }
            }
            return p;
        }

        public static void ConvertCubeMap(string p)
        {
            b = new Bitmap(p);
            int bw, bh;
            bw = b.Width;
            bh = b.Height;
            fs = new FileStream(p + ".cube", FileMode.Create, FileAccess.Write);

            wr = new BinaryWriter(fs);
            wr.Write(bw / 4);
            wr.Write(bh / 3);
            Write(bw / 4, 0, bw / 4, bh / 3);
            Write(0, bh / 3, bw / 4, bh / 3);
            Write(bw / 4, bh / 3, bw / 4, bh / 3);
            Write(bw / 2, bh / 3, bw / 4, bh / 3);
            Write(bw - bw / 4, bh / 3, bw / 4, bh / 3);
            Write(bw / 4, bh - bh / 3, bw / 4, bh / 3);
            fs.Flush();
            fs = null;
            wr = null;
        }

        private static int cf = 0;

        public static void Write(int x, int y, int w, int h)
        {
            Bitmap np = new Bitmap(w, h);
            for (int dy = 0; dy < h; dy++)
            {
                for (int dx = 0; dx < w; dx++)
                {
                    Color p = b.GetPixel(x + dx, y + dy);
                    np.SetPixel(dx, dy, p);
                    wr.Write(p.R);

                    wr.Write(p.G);
                    wr.Write(p.B);
                }
            }
            np.Save("CubeFace" + cf + ".png", System.Drawing.Imaging.ImageFormat.Png);
            cf++;
        }
    }
}