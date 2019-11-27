namespace Vivid.Resonance.Forms
{
    public class DragZoneForm : UIForm
    {

        public DragZoneForm()
        {

            Draw = () =>
            {

                DrawFormSolid(new OpenTK.Vector4(0, 0, 0, 1));
                DrawFormSolid(new OpenTK.Vector4(1, 1, 1, 1), 2, 2, W - 4, H - 4);

            };

        }

    }
}
