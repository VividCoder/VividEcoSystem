namespace Vivid.Composition.FrameTypes
{
    public class FrameSSRExtras : FrameType
    {
        public FrameSSRExtras() : base(Texture.TextureFormat.Normal)
        {
        }

        public override void Generate()
        {
            BindTarget();

            Graph.RenderSSRExtras();

            ReleaseTarget();
        }
    }
}