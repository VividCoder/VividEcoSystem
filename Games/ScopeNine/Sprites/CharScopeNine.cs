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
using Vivid.Map;
using System.IO;
using Vivid.Game;
using Vivid.Game.Platformer;

namespace ScopeNine.Sprites
{
   public class CharScopeNine : Vivid.Game.GameSprite
    {

        public CharScopeNine() : base(86,86)
        {

            SetImage(new Tex2D("Corona/Entity/Chars/ScopeNine/Idle1.png",true));
            CastShadow = true;
            Z = 1;
            ShadowPlane = 1;

        }

    }
}
