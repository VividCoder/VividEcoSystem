using OpenTK.Graphics.OpenGL4;
using Vivid.Effect;
using Vivid.FrameBuffer;
using Vivid.Scene;

namespace Vivid.Deffered
{
    public class DefferedRendererMRT
    {

        public Scene.SceneGraph3D Graph = null;
        public FrameBufferColorMRT FB = null;
        public FrameBufferColor FinalFB = null;
        public DROutput OutFX = null;
        public DRFinalPass FinalFX = null;
        public int W, H;

        public DefferedRendererMRT(int w, int h)
        {
            W = w;
            H = h;
            Init(w, h);
            OutFX = new DROutput();
            FinalFX = new DRFinalPass();
        }

        public void Init(int w, int h)
        {

            FinalFB = new FrameBufferColor(w, h);
            FB = new FrameBufferColorMRT(5, w, h, Texture.TextureFormat.RGB16F);
            W = w;
            H = h;


        }

        public void Render()
        {

            App.AppInfo.RW = W;
            App.AppInfo.RH = H;

            FXG.Cam = Graph.Cams[0];

            FB.Bind();

            RenderNode(Graph.Root);

            FB.Release();

            RenderFinal();

        }

        public void RenderFinal()
        {

            FinalFB.Bind();

            bool first = true;
            bool second = true;
            foreach (var light in Graph.Lights)
            {

                if (first)
                {

                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.Zero);
                    first = false;

                }
                else if (second)
                {
                    GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                    second = false;
                }
                FinalFX.Light = light;

                Draw.IntelliDraw.BeginDraw();

                Draw.IntelliDraw.DrawImg(0, H, W, -H);

                Draw.IntelliDraw.Binder bind = () =>
                {

                    for (int i = 0; i < FB.Targets.Count; i++)
                    {

                        FB.Targets[i].Bind(i);

                    }
                    //PositionFB.BB.Bind(0);
                    //NormalFB.BB.Bind(1);
                    //NormalsFB.BB.Bind(2);
                    //DiffuseFB.BB.Bind(3);

                };

                Draw.IntelliDraw.EndDraw(FinalFX, bind);

                for (int i = 0; i < FB.Targets.Count; i++)
                {

                    FB.Targets[i].Release(i);

                }

            }

            FinalFB.Release();

        }


        public void RenderNode(Node3D node)
        {
            if (node is Entity3D)
            {

                var ent = node as Entity3D;
                FXG.Local = ent.World;
                OutFX.Bind();



                foreach (var mesh in ent.Meshes)
                {
                    mesh.Material.Bind();
                    mesh.Viz.SetMesh(mesh);
                    mesh.Viz.Bind();
                    mesh.Viz.Visualize();
                    mesh.Viz.Release();
                    mesh.Material.Release();
                }
                OutFX.Release();
            }
            foreach (var s_node in node.Sub)
            {
                RenderNode(s_node);
            }
        }



    }
    public class DROutput : Effect3D
    {

        public DROutput() : base("", "data/fx/drOutVS.glsl", "data/fx/drOutFS.glsl")
        {

        }

        public override void SetPars()
        {
            SetMat("model", Effect.FXG.Local);
            SetMat("view", FXG.Cam.CamWorld);
            SetMat("proj", FXG.Cam.ProjMat);

            SetTex("colMap", 0);
            SetTex("normMap", 1);
        }

    }

}
