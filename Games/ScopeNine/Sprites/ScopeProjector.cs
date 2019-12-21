using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopeNine.Sprites
{
    public class ScopeProjector : Vivid.Scene.GraphSprite
     {

        public string Name = "";
        public Vivid.Texture.Texture2D HudImg = null;
        public Vivid.Tex.Tex2D Img;
        public Vivid.Scene.GraphLight Light1;
        public float XM, YM;
        public ScopeProjector() : base(32, 32)
        {

        }
        public virtual ScopeProjector GetNew()
        {
            return null;
        }
    }

    public class BigShotProjector : ScopeProjector
    {


        public override ScopeProjector GetNew()
        {
            return new BigShotProjector() as ScopeProjector;
            //return base.GetNew();

        }
        public BigShotProjector()
        {

            Name = "Big Shot";
            HudImg = new Vivid.Texture.Texture2D("Corona/img/icon/bigshoticon.png",Vivid.Texture.LoadMethod.Single ,true);
            Img = new Vivid.Tex.Tex2D("Corona/img/icon/bigshoticon.png", true);
            ImgFrame = Img;
            Light1 = new Vivid.Scene.GraphLight();
            Light1.X = X;
            Light1.Y = Y;
            Light1.Range = 300;
            Light1.Diffuse = new OpenTK.Vector3(0, 0.4f, 0.4f);


        }
        public override void Update()
        {
            
            base.Update();
            X = X + XM * 13;
            Y = Y + YM * 13;
            Light1.X = X;
            Light1.Y = Y;

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
