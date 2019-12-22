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
    public class InGamePlatform : VividState
    {

       ScopeNine.Hosts.ScopeNinePlatformer  GameHost;
        public Graph2DRain RainFX = null;

        public InGamePlatform()
        {

            SUI = new UI();

            GameHost = new Hosts.ScopeNinePlatformer();
               
           GameHost.SetMap("Corona/Map/test3");

            GameHost.SetMusic("Corona/Song/GameLevelOne.mp3");

            ScopeNine.Sprites.CharScopeNine Player = new Sprites.CharScopeNine();

            GameHost.AddNode(Player as GraphNode);

           // Player.Update();

            var marks = GameHost.GetMarkers("Spawn");

            Player.X = marks[0].X + 32;
            Player.Y = marks[0].Y;

            Player.X = 42;
            Player.Y = 32;

            //Vivid.

            GameHost.Graph.X -= 200;


        
            GameHost.Graph.Z = 1.6f;
            GameHost.Init();

            RainFX = new Graph2DRain();
            RainFX.Graph = GameHost.Graph;
            RainFX.Init(1280);



        }

        public override void InitState()
        {
            base.InitState();
       
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Texture2D.UpdateLoading();
            GameHost.Update();
            SUI.Update();
            RainFX.Update();
        }

        public override void DrawState()
        {
            base.DrawState();

            GameHost.Render();
            SUI.Render();
        }

    }
}
