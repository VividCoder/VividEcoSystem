﻿using System;
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
    public class ScopeNineIntro : VividState
    {

        Vivid.PostProcess.Processes.PPVirtualGrid FXGrid;
        public override void InitState()
        {
            base.InitState();

            FXGrid = new Vivid.PostProcess.Processes.PPVirtualGrid();

            SUI = new UI();

            var vid = new VideoForm().Set(0, 0, AppInfo.W, AppInfo.H) as VideoForm;
            Console.WriteLine("Play video");
            vid.SetVideo("Corona/Video/intro1.mov");
            SUI.Root = vid;
            SUI.FadeUI = false;
            SUI.FadeAlpha = 0.0f;
            vid.Click = (b) =>
            {
                vid.Stop();
                VividApp.PushState(new ScopeNineMenu());

            };

            
           

        }

        public override void UpdateState()
        {
            base.UpdateState();
            //return;
            Texture2D.UpdateLoading();
            SUI.Update();
            Console.WriteLine("Testing!");

        }

        public override void DrawState()
        {
            //return;

            base.DrawState();
            SUI.Render();
            FXGrid.Process(null);

        }

    }
}
