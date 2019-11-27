using Vivid.Data;
using Vivid.Effect;

using Vivid.Scene.Node;

namespace Vivid.Visuals
{
    public class RLMultiPassAnim : RenderLayer
    {
        public FXMultiPass3D fx = null;

        public override void Init()
        {
            fx = new FXMultiPass3D();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            m.Material.Bind();
            if (Light3D.Active != null)
            {
                Light3D.Active.ShadowFB.Cube.Bind(2);
            }
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Bind();
            }
            else
            {
                fx.Bind();
            }
            v.SetMesh(m);
            v.Bind();
            for (int i = 0; i < m.Subs.Count; i++)
            {
                v.Visualize(i);
            }
            v.Release();
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Release();
            }
            else
            {
                fx.Release();
            }
            if (Light3D.Active != null)
            {
                Light3D.Active.ShadowFB.Cube.Release(2);
            }
            m.Material.Release();
        }
    }

    public class RLColorOnly : RenderLayer
    {
        public FXColorOnly FX = null;

        public override void Init()
        {
            FX = new FXColorOnly();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            FX.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            FX.Release();
        }
    }

    public class RLMultiPass : RenderLayer
    {
        public FXMultiPass3D fx = null;

        public override void Init()
        {
            fx = new FXMultiPass3D();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            m.Material.Bind();
            if (Light3D.Active != null)
            {
                Light3D.Active.ShadowFB.Cube.Bind(2);
            }
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Bind();
            }
            else
            {
                fx.Bind();
            }
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Release();
            }
            else
            {
                fx.Release();
            }
            if (Light3D.Active != null)
            {
                Light3D.Active.ShadowFB.Cube.Release(2);
            }
            m.Material.Release();
        }
    }

    public class RLTerrain : RenderLayer
    {
        public FXTerrain fx = null;

        public override void Init()
        {
            fx = new FXTerrain();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            m.Material.Bind();
            if (Light3D.Active != null)
            {
                Light3D.Active.ShadowFB.Cube.Bind(2);
            }
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Bind();
            }
            else
            {
                fx.Bind();
            }
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Release();
            }
            else
            {
                fx.Release();
            }
            if (Light3D.Active != null)
            {
                Light3D.Active.ShadowFB.Cube.Release(2);
            }
            m.Material.Release();
        }
    }

    public class RLLightMap : RenderLayer
    {
        public FXLightMap FX;

        public override void Init()
        {
            FX = new FXLightMap();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            m.Material.BindLightmap();
            /* if (Lighting.GraphLight3D.Active != null)
             {
                 Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
             }
             */
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Bind();
            }
            else
            {
                FX.Bind();
            }
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Release();
            }
            else
            {
                FX.Release();
            }
            /*
            if (Lighting.GraphLight3D.Active != null)
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            }
            */
            m.Material.ReleaseLightmap();
        }
    }

    public class RLNoFX : RenderLayer
    {
        public FXNoFX FX;

        public override void Init()
        {
            FX = new FXNoFX();
        }

        public override void Render(Mesh3D m, Visualizer v)
        {
            m.Material.Bind();
            /* if (Lighting.GraphLight3D.Active != null)
             {
                 Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
             }
             */
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Bind();
            }
            else
            {
                FX.Bind();
            }
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            if (FXG.FXOverride != null)
            {
                FXG.FXOverride.Release();
            }
            else
            {
                FX.Release();
            }
            /*
            if (Lighting.GraphLight3D.Active != null)
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            }
            */
            m.Material.Release();
        }
    }
}