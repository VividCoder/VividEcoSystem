using Vivid.Data;
using Vivid.Effect;

namespace Vivid.Visuals
{
    public class RLSSRExtrasMap : RenderLayer
    {
        public FXSSRExtrasMap fx = null;

        public override void Init()
        {
            fx = new FXSSRExtrasMap();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            if (m.Material == null) return;
            m.Material.ExtraMap.Bind(0);
            fx.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            fx.Release();
            m.Material.ExtraMap.Release(0);
        }
    }

    public class RLPositionMap : RenderLayer
    {
        public FXPositionMap fx = null;

        public override void Init()
        {
            fx = new FXPositionMap();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            fx.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            fx.Release();
        }
    }

    public class RLNormalMap : RenderLayer
    {
        public FXNormalMap fx = null;

        public override void Init()
        {
            fx = new FXNormalMap();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            fx.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            fx.Release();
        }
    }

    public class RLDepth : RenderLayer
    {
        public FXDepth3D fx = null;

        public override void Init()
        {
            fx = new FXDepth3D();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            // m.Mat.Bind();
            fx.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            fx.Release();
            //m.Mat.Release();
        }
    }
}