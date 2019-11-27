using Vivid.Resonance.Forms;
using Vivid.Texture;

namespace Vivid.Resonance
{
    public class SimpleUI
    {
        public static void Begin()
        {
            Vivid.Draw.IntelliDraw.BeginDraw();
        }

        public static void Image(int x, int y, int w, int h, Texture2D img)
        {
            var tmp_img = new ImageForm().Set(x, y, w, h).SetImage(img);
            tmp_img.Draw();
        }

        public static void End()
        {
            Vivid.Draw.IntelliDraw.EndDraw();
        }
    }
}