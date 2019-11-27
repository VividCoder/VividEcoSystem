namespace Vivid.Material
{
    public class MaterialParticle3D : Material3D
    {
        public override void Bind()
        {
            Active = this;
            ColorMap.Bind(0);
        }

        public override void Release()
        {
            ColorMap.Release(0);
            Active = null;
        }
    }
}