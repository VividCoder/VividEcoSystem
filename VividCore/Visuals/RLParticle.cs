using Vivid.Data;
using Vivid.Effect;

namespace Vivid.Visuals
{
    public class RLParticle : RenderLayer
    {
        public FXParticle fx = null;

        public override void Init()
        {
            fx = new FXParticle();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            m.Material.Bind();
            // Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
            fx.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            fx.Release();
            //Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            m.Material.Release();
        }
    }
}