﻿using OpenTK;
using Vivid.Data;
using Vivid.Scene;
using Vivid.Texture;

namespace Vivid.ParticleSystem
{
    public class Particle : Entity3D
    {
        public float Theta = 0;
        public Vector3 Pos = Vector3.Zero;
        public Vector3 Inertia = Vector3.Zero;
        public float Drag = 0.98f;
        public float Alpha = 1.0f;
        public float Life = 1.0f;
        public Texture2D Tex = null;
        public int W, H;

        public override void Init()
        {
            Renderer = new Visuals.RParticle();
            Lit = false;
            FaceCamera = true;
            CastShadows = false;
            CastDepth = false;
        }

        public Particle(Particle bp)
        {
            Name = "ParticleNode";
            Meshes.Add(bp.Meshes[0]);
            W = bp.W;
            H = bp.H;
            Alpha = bp.Alpha;
            Life = bp.Life;
            Tex = bp.Tex;
        }

        public Particle(int w, int h)
        {
            W = w;
            H = h;
            Mesh3D mesh = new Mesh3D(12, 4);

            Vector3 p1 = new Vector3(-w / 2, -h / 2, 0);
            Vector3 p2 = new Vector3(w / 2, -h / 2, 0);
            Vector3 p3 = new Vector3(w / 2, h / 2, 0);
            Vector3 p4 = new Vector3(-w / 2, h / 2, 0);

            Vector2 uv1 = new Vector2(0, 0);
            Vector2 uv2 = new Vector2(1, 0);
            Vector2 uv3 = new Vector2(1, 1);
            Vector2 uv4 = new Vector2(0, 1);

            Vector3 z = Vector3.Zero;

            mesh.SetVertex(0, p1, z, z, z, uv1);
            mesh.SetVertex(1, p2, z, z, z, uv2);
            mesh.SetVertex(2, p3, z, z, z, uv3);
            mesh.SetVertex(3, p4, z, z, z, uv4);

            mesh.SetIndex(0, 0);
            mesh.SetIndex(1, 1);
            mesh.SetIndex(2, 2);
            mesh.SetIndex(3, 2);
            mesh.SetIndex(4, 3);
            mesh.SetIndex(5, 0);

            mesh.SetIndex(6, 2);
            mesh.SetIndex(7, 1);
            mesh.SetIndex(8, 0);
            mesh.SetIndex(9, 0);
            mesh.SetIndex(10, 3);
            mesh.SetIndex(11, 2);

            mesh.Final();

            Meshes.Add(mesh);
        }

        public override void Update()
        {
            LocalPos = LocalPos + Inertia;
            LocalPos = LocalPos * Drag;
        }

        public override void Present(Cam3D c)
        {
            SetMats(c);
            Bind();
            PreRender();
            Render();
            PostRender();
            Release();

            // Console.WriteLine("Rendering Particle.");
        }
    }
}