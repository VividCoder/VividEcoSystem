using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Tex;
namespace SpaceEngine.Map.Tile
{
    public class Tile
    {

        public string Name
        {
            get;
            set;
        }
        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public Tex2D ColorImage
        {
            get;
            set;
        }

        public Tex2D NormalImage
        {
            get;
            set;
        }

            
        public Tile(string imagePath)
        {
            ImagePath = imagePath;
            ColorImage = new Tex2D(imagePath, true);
            Name = "Tile";
        }
        Tex2D LoadImage(System.IO.BinaryReader r)
        {
            bool alpha = r.ReadBoolean();
            int w = r.ReadInt32();
            int h = r.ReadInt32();
            int bpp = 3;
            if (alpha)
            {
                bpp = 4;
            }
            byte[] dat = r.ReadBytes(w * h * bpp);
            return new Tex2D(dat, alpha, w, h);
        }
        void WriteImage(System.IO.BinaryWriter w, Tex2D tex)
        {


            w.Write(tex.Alpha);

            w.Write(tex.Width);
            w.Write(tex.Height);

            w.Write(tex.RawData);


        }
        public Tile(System.IO.BinaryReader r)
        {

            if (r.ReadBoolean())
            {
                int v = 5;
                ColorImage = LoadImage(r);
            }
            if (r.ReadBoolean())
            {
                NormalImage = LoadImage(r);
            }

        }
        public void Write(System.IO.BinaryWriter w)
        {

            w.Write(ColorImage != null);
            if (ColorImage != null)
            {
                WriteImage(w, ColorImage);
            }
            w.Write(NormalImage != null);
            if (NormalImage != null)
            {
                WriteImage(w, NormalImage);
            }

        }
        public Tile(Tex2D tex)
        {

            ColorImage = tex;
            Name = "Tile";

        }

    }
}
