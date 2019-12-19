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
using Vivid.Input;
namespace ScopeNine.Sprites
{

    public enum ScopeState
    {
        Idle,Walking,Running,Jumping
    }

   public class CharScopeNine : Vivid.Game.GameSprite
    {

        public ScopeState State = ScopeState.Idle;

        bool onGround = false;

        public CharScopeNine() : base(86,86)
        {

            SetImage(new Tex2D("Corona/Entity/Chars/ScopeNine/Idle1.png",true));
            CastShadow = true;
            Z = 1;
            ShadowPlane = 1;
            AddAnim("Walk1", new SpriteAnim("Corona/Entity/Chars/ScopeNine/AnimWalk/",0.5f));
            AddAnim("Run1",new SpriteAnim("Corona/Entity/Chars/ScopeNine/AnimWalk/",1.0f));
            AddAnim("Idle1", new SpriteAnim("Corona/Entity/Chars/ScopeNine/AnimIdle1/", 0.35f));
            SetAnim("Idle1");
            ImgFrame = GetAnimFrame();
            Drag = 0.94f;


        }

        public override void Update()
        {

            switch (State)
            {
                case ScopeState.Idle:
                    if (CurAnim != Anims["Idle1"])
                    {
                        SetAnim("Idle1");
                    }
                    break;
                case ScopeState.Walking:
                    if (CurAnim != Anims["Walk1"])
                    {
                        SetAnim("Walk1");
                    }
                    break;
                case ScopeState.Running:
                    if (CurAnim != Anims["Run1"])
                    {
                        SetAnim("Run1");
                    }
                    break;
            }

            base.Update();
            ImgFrame = GetAnimFrame();

            float xm = XIn.LeftX();

            if (Vivid.Input.Input.KeyIn(OpenTK.Input.Key.A))
            {
                xm = -1;
            }

            if (Vivid.Input.Input.KeyIn(OpenTK.Input.Key.D))
            {
                xm = 1;
            }


            xm = xm * 0.4f;

            if (XIn.bX())
            {
                if (onGround)
                {

                    Move2D(0, -10);
                    onGround = false;
                    //State = ScopeState.Jumping;
                    Y = Y - 3;

                }
            }

            if (XIn.leftB())
            {
                xm = xm * 2;
                
                if(Math.Abs(xm)>0)
                {
                    State = ScopeState.Running;
                }
            }
            else
            {
                if(Math.Abs(xm)>0)
                {
                    State = ScopeState.Walking;
                }
                else
                {
                    State = ScopeState.Idle;
                }
            }

            if (Math.Abs(xm) < 0.2)
            {
                State = ScopeState.Idle;
            }
            else 
            {
                //State = ScopeState.Walking;
            }

            Move2D(xm*0.35f, 0);

            if (xm > 0)
            {
                FlipDrawX = false;
            }
            else if(xm<0)
            {
                FlipDrawX = true;
            }

            //X = X + xm;
            var cm = Graph.CreateCollisionMap(0.05f);


            float cx = this.RealX;
            float cy = this.RealY;



            var hit =  cm.RayCast(cx, cy, cx, cy + 60);

            if (hit == null)
            {
                //X = hit.HitX;
                Move2D(0, 0.3f);
                onGround = false;
            }
            else 
            {
               Y = hit.HitY - 62;
                onGround = true;


            }

            //Rot += 0.1f;
            
            //vironment.Exit(1);
            //base.Update();

        }

    }
}
