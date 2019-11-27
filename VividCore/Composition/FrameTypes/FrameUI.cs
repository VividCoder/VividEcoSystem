namespace Vivid.Composition.FrameTypes
{
    public class FrameUI : FrameType
    {
        public Vivid.Resonance.UI GUI = null;

        public override void Generate()
        {
            if (Regenerate == false)
            {
                //    return;
            }

            BindTarget();

            GUI.Render();

            ReleaseTarget();
        }
    }
}