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

        public InGamePlatform()
        {

            GameHost = new Hosts.ScopeNinePlatformer();
               
           GameHost.SetMap("Corona/Map/test3");

            GameHost.SetMusic("Corona/Song/GameLevel1.mp3");

            ScopeNine.Sprites.CharScopeNine Player = new Sprites.CharScopeNine();

            GameHost.AddNode(Player as GraphNode);

            var marks = GameHost.GetMarkers("Spawn");

            Player.X = marks[0].X;
            Player.Y = marks[0].Y;

            GameHost.Init();


        }

        public override void InitState()
        {
            base.InitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Texture2D.UpdateLoading();
        }

        public override void DrawState()
        {
            base.DrawState();

            GameHost.Render();

        }

    }
}
