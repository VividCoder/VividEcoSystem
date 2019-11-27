namespace Vivid.Composition.FrameTypes
{
    public class FrameNormalMap : FrameType
    {
        public FrameNormalMap() : base(Texture.TextureFormat.RGB16F)
        {
        }

        public override void Generate()
        {
            BindTarget();

            Graph.RenderNormalMap();

            ReleaseTarget();
        }
    }

    public class FramePositionMap : FrameType
    {
        public FramePositionMap() : base(Texture.TextureFormat.RGB16F)
        {
        }

        public override void Generate()
        {
            BindTarget();

            Graph.RenderPositionMap();

            ReleaseTarget();
        }
    }
}