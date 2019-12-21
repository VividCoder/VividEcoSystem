using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeNine.Sprites
{
    public class ScopeProjector
    {

        public string Name = "";
        public Vivid.Texture.Texture2D HudImg = null;

    }

    public class BigShotProjector : ScopeProjector
    {

        

        public BigShotProjector()
        {

            Name = "Big Shot";
            HudImg = new Vivid.Texture.Texture2D("Corona/img/icon/bigshoticon.png",Vivid.Texture.LoadMethod.Single ,true);

        }
    }
    public class LongShotProjector : ScopeProjector
    {

        public LongShotProjector()
        {
            Name = "Long Shot";
            HudImg = new Vivid.Texture.Texture2D("Corona/img/icon/longshoticon.png", Vivid.Texture.LoadMethod.Single, true);
        }

    }
}
