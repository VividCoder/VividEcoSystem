using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.PostProcess.Processes
{
    public class PPVirtualGrid : PostProcess
    {
        public Tex.Tex2D White = null;
        public FXGrid FX = null;
        public PPVirtualGrid()
        {
            FX = new FXGrid();
            FB1 = new FrameBuffer.FrameBufferColor(App.AppInfo.W, App.AppInfo.H);
        }
        public override Vivid.Tex.Tex2D Process(Vivid.Tex.Tex2D img)
        {
            if(White == null)
            {
                White = new Tex.Tex2D("Corona/Img/fx/vrgrid1.png",true);
            }
            Vivid.Draw.Render.Begin();
            Vivid.Draw.Render.Image(0, 0, App.AppInfo.W, App.AppInfo.H, White);

            FX.Bind();

            Vivid.Draw.Render.End2D();

            FX.Release();
            //return base.Process(img);//

            return null;

        }
        FrameBuffer.FrameBufferColor FB1;
        Effect.Effect3D GridFX;

    }
    public class FXGrid : Effect.Effect3D
    {

        public FXGrid() : base("","data/shader/pp/gridvs.glsl","data/shader/pp/gridfs.glsl")
        {

        }
        float xp = 1.5f;
        public override void SetPars()
        {
            base.SetPars();

            SetTex("tDiffuse", 0);
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
