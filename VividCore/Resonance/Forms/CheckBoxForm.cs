namespace Vivid.Resonance.Forms
{
    public delegate void Checked(bool check);

    public class CheckBoxForm : UIForm
    {
        public bool Checked = false;
        public Checked Check = null;

        public CheckBoxForm()
        {
            MouseDown = (b) =>
            {
                if (b == 0)
                {
                    Checked = Checked ? false : true;
                    Check?.Invoke(Checked);
                }
            };

            Draw = () =>
            {
                DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.9f));
                DrawFormSolid(new OpenTK.Vector4(0.9f, 0.9f, 0.9f, 0.8f), 2, 2, W - 4, H - 4);

                if (Checked)
                {
                    DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.9f), 4, 4, W - 8, H - 8);
                }

                DrawText(Text, W + 5, -4);
            };
        }
    }
}