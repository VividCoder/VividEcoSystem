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
using ScopeNine.State;

namespace ScopeNine
{
    public class ScopeNineApp : VividApp
    {

        public ScopeNineApp() : base("ScopeNine Alpha",1024,700,false)
        {

            InitState = new ScopeNineIntro();

            Run();

        }

    }
}
