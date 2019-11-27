using System.Collections.Generic;
using Vivid.Data;

namespace Vivid.Visuals
{
    public class Renderer
    {
        public List<RenderLayer> Layers = new List<RenderLayer>();
        public RLDepth RLD = null;
        public RLNormalMap RLNM = null;
        public RLPositionMap RLPM = null;
        public RLSSRExtrasMap RLSE = null;

        public Renderer()
        {
            Init();
            RLD = new RLDepth();
            RLNM = new RLNormalMap();
            RLPM = new RLPositionMap();
            RLSE = new RLSSRExtrasMap();
        }

        public virtual void Init()
        {
        }

        public virtual void Bind(Mesh3D m)
        {
        }

        public virtual void Render(Mesh3D m)
        {
            foreach (RenderLayer rl in Layers)
            {
                rl.Render(m, m.Viz);
            }
        }

        public void RenderSSRExtrasMap(Mesh3D m)
        {
            RLSE.Render(m, m.Viz);
        }

        public void RenderPositionMap(Mesh3D m)
        {
            RLPM.Render(m, m.Viz);
        }

        public void RenderNormalMap(Mesh3D m)
        {
            RLNM.Render(m, m.Viz);
        }

        public virtual void RenderDepth(Mesh3D m)
        {
            RLD.Render(m, m.Viz);
        }

        public virtual void Release(Mesh3D m)
        {
        }
    }
}