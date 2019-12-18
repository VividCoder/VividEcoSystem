using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using Vivid.Texture;
using Vivid.Tex;
using Vivid.Scene;
using Vivid.Scene.Node;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.State;
using Vivid.Game;
namespace ScopeNine.State
{
    public class ScopeNineMenu : VividState
    {
        Vivid.PostProcess.Processes.PPVirtualGrid FXGrid;
        public override void InitState()
        {
            SUI = new UI();
            FXGrid = new Vivid.PostProcess.Processes.PPVirtualGrid();

            var vid = new VideoForm().Set(0, 0, AppInfo.W, AppInfo.H) as VideoForm;

            vid.SetVideo("Corona/Video/menuseq3.mov");
            SUI.Root = vid;

            vid.Click = (b) =>
            {

            //    vid.Pause();
             //   VividApp.PushState(new ScopeNineMenu());

            };

            var newGame = new ButtonForm().Set(AppInfo.W / 2 - 140, AppInfo.H - 160, 280, 30, "Begin Assignment") as ButtonForm;
            var config = new ButtonForm().Set(AppInfo.W / 2 - 140, AppInfo.H - 120, 280, 30, "Configure Environment") as ButtonForm;
            var exit = new ButtonForm().Set(AppInfo.W / 2 - 140, AppInfo.H - 80, 280, 30, "Exit") as ButtonForm;
           vid.Add(newGame);
            vid.Add(config);
            vid.Add(exit);

            var powerDown = Vivid.Audio.Songs.LoadSound("Corona/Sound/Misc/powerdown2.mp3");

            newGame.Click = (b) =>
            {

                vid.Stop();
                VividApp.PushState(new InGamePlatform());

            };

            if (Vivid.Input.XIn.Start())
            {

                vid.Stop();
                VividApp.PushState(new InGamePlatform());

            }

            exit.Click = (b) =>
            {

                vid.StopAudio();
                powerSnd = Vivid.Audio.Songs.PlaySource(powerDown);
                SUI.FadeUI = true;
                SUI.FadeAlpha = 1.0f;

            };






        }
        Vivid.Audio.VSound powerSnd = null;
        public override void UpdateState()
        {
            base.UpdateState();
            SUI.Update();
            Texture2D.UpdateLoading();
            if (powerSnd != null)
            {
                if(powerSnd.Playing == false)
                {
                    Environment.Exit(0);
                }
            }
        }

        public override void ResizeState(int w, int h)
        {
           
        }

        public override void DrawState()
        {
            base.DrawState();
            SUI.Render();
            FXGrid.Process(null);
        }

    }

}
