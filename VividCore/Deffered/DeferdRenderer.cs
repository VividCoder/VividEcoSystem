using OpenTK.Graphics.OpenGL4;
using Vivid.Effect;
using Vivid.FrameBuffer;
using Vivid.Scene;

namespace Vivid.Deffered
{
    public class DeferdRenderer
    {

        public Scene.SceneGraph3D Graph = null;
        public FrameBufferColor DiffuseFB;
        public FrameBufferColor ShadowFB;
        public FrameBufferColor NormalFB;
        public FrameBufferColor PositionFB;
        public FrameBufferColor SpecFB;
        public FrameBufferColor NormalsFB;
        public FrameBufferColor FinalFB;
        public DRDiffuse DiffFx;
        public DRNormal NormFx;
        public DRPosition PosFx;
        public DRNormals NormsFX;
        public DRFinalPass FinalFx;
        public int W, H;
        public DeferdRenderer(int w, int h)
        {
            W = w;
            H = h;
            Init(w, h);
            DiffFx = new DRDiffuse();
            NormFx = new DRNormal();
            PosFx = new DRPosition();
            NormsFX = new DRNormals();
            FinalFx = new DRFinalPass();
        }

        private void Init(int w, int h)
        {
            DiffuseFB = new FrameBufferColor(w, w);
            NormalFB = new FrameBufferColor(w, h);
            SpecFB = new FrameBufferColor(w, h);
            PositionFB = new FrameBufferColor(w, h);
            NormalsFB = new FrameBufferColor(w, h);
            FinalFB = new FrameBufferColor(w, h);
            W = w;

            H = h;
        }

        public void Render()
        {

            App.AppInfo.RW = W;
            App.AppInfo.RH = H;

            FXG.Cam = Graph.Cams[0];

            DiffuseFB.Bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderNode(Graph.Root, DiffFx);

            DiffuseFB.Release();

            NormalFB.Bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderNode(Graph.Root, NormFx);

            NormalFB.Release();

            PositionFB.Bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderNode(Graph.Root, PosFx);

            PositionFB.Release();

            NormalsFB.Bind();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderNode(Graph.Root, NormsFX);

            NormalsFB.Release();

            RenderFinal();



        }

        public void RenderFinal()
        {

            FinalFB.Bind();

            foreach (var light in Graph.Lights)
            {

                FinalFx.Light = light;

                Draw.IntelliDraw.BeginDraw();

                Draw.IntelliDraw.DrawImg(0, H, W, -H);

                Draw.IntelliDraw.Binder bind = () =>
                {

                    PositionFB.BB.Bind(0);
                    NormalFB.BB.Bind(1);
                    NormalsFB.BB.Bind(2);
                    DiffuseFB.BB.Bind(3);

                };

                Draw.IntelliDraw.EndDraw(FinalFx, bind);

                PositionFB.BB.Release(0);
                NormalFB.BB.Release(1);
                NormalsFB.BB.Release(2);
                DiffuseFB.BB.Release(3);

            }

            FinalFB.Release();

        }

        public void RenderNode(Node3D node, Effect3D fx)
        {
            if (node is Entity3D)
            {

                var ent = node as Entity3D;
                FXG.Local = ent.World;
                fx.Bind();


                foreach (var mesh in ent.Meshes)
                {
                    mesh.Material.Bind();
                    mesh.Viz.SetMesh(mesh);
                    mesh.Viz.Bind();
                    mesh.Viz.Visualize();
                    mesh.Viz.Release();
                    mesh.Material.Release();
                }
                fx.Release();
            }
            foreach (var s_node in node.Sub)
            {
                RenderNode(s_node, fx);
            }
        }

    }

    public class DRFinalPass : Effect3D
    {

        public Scene.Node.Light3D Light = null;

        public DRFinalPass() : base("", "data/fx/finalPassVS.glsl", "data/fx/finalPassFS.glsl")
        {

        }

        public override void SetPars()
        {
            SetMat("proj", OpenTK.Matrix4.CreateOrthographicOffCenter(0, Vivid.App.AppInfo.RW, Vivid.App.AppInfo.RH, 0, -1, 1000));
            SetTex("posMap", 0);
            SetTex("normMap", 1);
            SetTex("normsMap", 2);
            SetTex("colMap", 3);
            SetTex("tanMap", 4);
            SetVec3("viewPos", FXG.Cam.LocalPos);
            SetVec3("lightPos", Light.LocalPos);
            SetVec3("lightCol", Light.Diff);
            SetVec3("lightSpec", Light.Spec);
            SetFloat("lightRange", Light.Range);
        }

    }

    public class DRNormals : Effect3D
    {

        public DRNormals() : base("", "data/fx/normalsVS.glsl", "data/fx/normalsFS.glsl")
        {

        }
        public override void SetPars()
        {
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);
        }
    }

    public class DRPosition : Effect3D
    {
        public DRPosition() : base("", "data/fx/positionVS.glsl", "data/fx/positionFS.glsl")
        {

        }

        public override void SetPars()
        {
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);


        }

    }

    public class DRNormal : Effect3D
    {

        public DRNormal() : base("", "data/fx/normalVS.glsl", "data/fx/normalFS.glsl")
        {
        }

        public override void SetPars()
        {
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);

            SetInt("normMap", 1);
        }
    }

    public class DRDiffuse : Effect3D
    {

        public DRDiffuse() : base("", "data/fx/diffuseVS.glsl", "data/fx/diffuseFS.glsl")
        {

        }

        public override void SetPars()
        {

            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);

            SetInt("colMap", 0);

        }

    }




}
