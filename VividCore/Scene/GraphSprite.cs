namespace Vivid.Scene
{
    public class GraphSprite : GraphNode
    {

      

        public static Tex.Tex2D NormalBlank=null;

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