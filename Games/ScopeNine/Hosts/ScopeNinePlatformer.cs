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
using Vivid.PostProcess;
using Vivid.PostProcess.Processes;
namespace ScopeNine.Hosts
{
    public class ScopeNinePlatformer : Vivid.Game.Platformer.GamePlatformHost
    {

        PPMapGrid VGrid = null;

        public void Init()
        {

            VGrid = new PPMapGrid();
            VGrid.Map = CurMap;
            VGrid.Graph = Graph;

        }

        public void Render()
        {
            RenderMap();


            VGrid.Process(null);
        }

    }
}
