﻿using Vivid.Data;

namespace Vivid.Visuals
{
    public class VSimple : Visualizer
    {
        public VSimple(int vc, int ic) : base(vc, ic)
        {
        }

        public override void SetData(VertexData<float> d)
        {
            dat = d;
        }

        public override void SetMesh(Mesh3D m)
        {
            md = m;
        }

        public override void Visualize()
        {
            /*
            md.Mat.Bind();
            GL.Begin(BeginMode.Triangles);
            var v = md.GetVerts();

            for(int i = 0; i < md.NumIndices; i += 3)
            {
                uint i1, i2, i3;
                i1 = md.Indices[i] * (uint)md.Data.Components;
                i2 = md.Indices[i + 1] * (uint)md.Data.Components;
                i3 = md.Indices[i + 2] * (uint)md.Data.Components;
                GL.TexCoord2(v[i1 + 12], v[i1 + 13]);
                GL.Vertex3(v[i1], v[i1 + 1], v[i1 + 2]);
                GL.TexCoord2(v[i2 + 12], v[i2 + 13]);
                GL.Vertex3(v[i2], v[i2 + 1], v[i2 + 2]);
                GL.TexCoord2(v[i3 + 12], v[i3 + 13]);
                GL.Vertex3(v[i3], v[i3 + 1], v[i3 + 2]);
            }

            GL.End();
            md.Mat.Release();
        */
        }
    }
}