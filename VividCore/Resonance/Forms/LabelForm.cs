namespace Vivid.Resonance.Forms
{
    public class LabelForm : UIForm
    {
        public LabelForm()
        {
            void EV_Draw()
            {
                DrawText(Text, 3, 3, new OpenTK.Vector4(1, 1, 1, 1));
            }

            Draw = EV_Draw;
        }
    }
}