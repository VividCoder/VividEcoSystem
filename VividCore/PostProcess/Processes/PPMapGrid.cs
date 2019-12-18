using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Tex;

namespace Vivid.PostProcess.Processes
{
    public class PPMapGrid : PostProcess
    {
        public Map.Map Map;
        public Scene.SceneGraph2D Graph;
        public FXMapGrid FX = null;
        public FrameBuffer.FrameBufferColor FB1;
        public Tex2D White = null;
        public PPMapGrid()
        {
            FX = new FXMapGrid();
            FB1 = new FrameBuffer.FrameBufferColor(App.AppInfo.W, App.AppInfo.H);
        }
        public override Tex2D Process(Tex2D img)
        {

            if (Graph == null)
            {
                Graph = Map.UpdateGraph(64, 64);
            }

            if (White == null)
            {
                White = new Tex.Tex2D("Corona/Img/fx/vrgrid1.png", true);
            }
            FB1.Bind();

            //Graph.Draw(false);
            Graph.DrawShapes(0.1f);

            FB1.Release();

            //return null;

            var b2 = FB1.BB.ToTex2D();

            Graph.Draw(true);

            Vivid.Draw.Render.Begin();
            Vivid.Draw.Render.Image(0, App.AppInfo.H, App.AppInfo.W, -App.AppInfo.H,b2,White);

            FX.Bind();

            Vivid.Draw.Render.End2D();

            FX.Release();

            return base.Process(img);
        }
    }

    public class FXMapGrid : Effect.Effect3D
    {
        public FXMapGrid() : base("","data/shader/pp/mapgridvs.glsl","data/shader/pp/mapgridfs.glsl")
        {

        }
        float xp = 0.5f;
        public override void SetPars()
        {

            SetTex("tDiffuse", 0);
            SetTex("tGrid", 2);
            SetMat("proj", OpenTK.Matrix4.CreateOrthographicOffCenter(0, Vivid.App.AppInfo.RW, Vivid.App.AppInfo.RH, 0, -1, 1000));
            SetFloat("xP", xp);
            xp = xp - 0.01f;
            if (xp < 0)
            {
                xp = 1.5f;
            }
        }
    }
}
