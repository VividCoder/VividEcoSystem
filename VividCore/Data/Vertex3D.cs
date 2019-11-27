﻿using OpenTK;

namespace Vivid.Data
{
    public class Vertex3D : VertexData<float>
    {
        public Vertex3D(int vertexCount = 0, int indexCount = 0)
        {
            Init(vertexCount, 14, 14 * 4, 4, indexCount);
        }

        public float[] Get3D()
        {
            return Data;
        }

        public void SetIndex(int i, int v)
        {
            Index[i] = v;
        }

        public void SetVertex(int id, Vector3 p, Vector3 t, Vector3 b, Vector3 n, Vector2 uv)
        {
            int l = id * Components;
            l = SetData(p, l);
            l = SetData(t, l);
            l = SetData(b, l);
            l = SetData(n, l);

            Data[l++] = uv.X;
            Data[l++] = uv.Y;
        }

        private int SetData(Vector3 p, int l)
        {
            Data[l++] = p.X;
            Data[l++] = p.Y;
            Data[l++] = p.Z;
            return l;
        }
    }
}