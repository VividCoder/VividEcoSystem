using Vivid.Data;

namespace Vivid.Visuals
{
    public class RLSimple : RenderLayer
    {
        public override void Init()
        {
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            v.SetMesh(m);
            v.Visualize();
        }
    }
}