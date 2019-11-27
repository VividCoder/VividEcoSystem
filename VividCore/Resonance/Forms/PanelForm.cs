using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public class PanelForm : UIForm
    {
        public static Texture2D Tex = null;

        public PanelForm()
        {
            if (Tex == null)
            {
                Tex = new Texture2D("data/UI/panel.png", LoadMethod.Single, false);
            }
            void DrawFunc()
            {
                DrawForm(Tex);
            }

            Draw = DrawFunc;
        }
    }
}