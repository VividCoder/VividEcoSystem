namespace Vivid.Scene
{
    public class GraphSprite : GraphNode
    {

      

        public static Tex.Tex2D NormalBlank=null;

         public float RealX
        {
            get
            {

                SyncCoords();

                return CenterX;



            }
        }

        public float RealY
        {
            get
            {
                SyncCoords();
                return CenterY;
            }
        }

        public float SmallX
        {
            get
            {
                SyncCoords();
                float rx = 30000;
                foreach(var p in DrawP)
                {
                    if (p.X < rx) rx = p.X;
                }
                return rx;

            }
        }

        public float SmallY
        {
            get
            {
                SyncCoords();
                float ry = 30000;
                foreach (var p in DrawP)
                {
                    if (p.Y < ry) ry = p.Y;
                }
                return ry;

            }
        }

        public float BigX
        {
            get
            {
                SyncCoords();
                float rx = -30000;
                foreach (var p in DrawP)
                {
                    if (p.X > rx) rx = p.X;
                }
                return rx;

            }
        }

        public float BigY
        {
            get
            {
                SyncCoords();
                float ry = -30000;
                foreach (var p in DrawP)
                {
                    if (p.Y > ry) ry = p.Y;
                }
                return ry;

            }
        }


        public float CenterX
        {
            get
            {
                var sx = SmallX;
                var bx = BigX;

                var nv = (bx - sx) / 2;
                return sx + nv;

            }
        }

        public float CenterY
        {
            get
            {
                var sy = SmallY;
                var by = BigY;
                var nv = (by - sy) / 2;
                return sy + nv;
            }
        }

        public GraphSprite(int w,int h)
        {

            CastShadow = false;
            W = w;
            H = h;
            if (NormalBlank == null)
            {
                NormalBlank = new Tex.Tex2D("data/tex/normblank.png", false);
            }
            NormalMap = NormalBlank;
           

        }

        public void SetImage(Tex.Tex2D img)
        {

            ImgFrame = img;

        }


        public GraphSprite(Tex.Tex2D img,Tex.Tex2D normal, int w = -1, int h = -1)
        {
            if (normal != null)
            {
                int vv = 5;
            }
            if (NormalBlank == null)
            {
                NormalBlank = new Tex.Tex2D("data/tex/normblank.png", false);
            }
            if (normal == null)
            {
                NormalMap = NormalBlank;
            }
            CastShadow = false;
            ImgFrame = img;
            if (normal != null)
            {
                NormalMap = normal;
            }
            if (w == -1)
            {
                W = ImgFrame.Width;
            }
            else
            {
                W = w;
            }
            if (h == -1)
            {
                H = ImgFrame.Height;
            }
            else
            {
                H = h;
            }
        }

        public GraphSprite(string path, int w = -1, int h = -1)
        {
            CastShadow = false;
            ImgFrame = new Tex.Tex2D(path, true);
            if (w == -1)
            {
                W = ImgFrame.Width;
            }
            else
            {
                W = w;
            }
            if (h == -1)
            {
                H = ImgFrame.Height;
            }
            else
            {
                H = h;
            }
        }
    }
}