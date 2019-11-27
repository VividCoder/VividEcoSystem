namespace Vivid.Resonance.Forms
{
    public class DockAreaForm : UIForm
    {
        public DockAreaForm()
        {
            AfterSet = () =>
            {
                foreach (var f in Forms)
                {
                    f.Set(0, 0, W, H, f.Text);
                    f.Docked = true;
                }
            };
            Draw = () =>
            {
                //DrawFormSolid(new OpenTK.Vector4(0.7f, 0, 0, 1));
            };
        }
    }
}