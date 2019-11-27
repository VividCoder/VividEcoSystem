using System;

namespace Vivid.Scene.Flare
{
    public class FlareBloom : Flare3D
    {
        public static Texture.Texture2D BaseImg = null;

        public FlareBloom()
        {
            if (BaseImg == null)
            {
                BaseImg = new Texture.Texture2D("data/engine/flare/baseflare1.png", Texture.LoadMethod.Single, true);
            }

            Console.WriteLine("Flare created.");
        }

        public override void Render(OpenTK.Vector2 pos, float dis)
        {
            Draw.Pen2D.BlendMod = Draw.PenBlend.Alpha;

            float fW = 4690;
            float fH = 3300;

            float df = dis / 180000;
            df = 1.0f - df;

            if (df < 0) df = 0.0f;
            if (df == 0.0f) return;

            fW = fW * df;
            fH = fH * df;

            Draw.Pen2D.Rect(pos.X - fW / 2, pos.Y - fH / 2, fW, fH, BaseImg, new OpenTK.Vector4(1, 1, 1, 0.5f));
        }
    }
}