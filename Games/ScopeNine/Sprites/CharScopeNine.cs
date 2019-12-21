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
        public Tex2D PulseAimImg = null;
        public Texture2D WeaponHudImg;
        ImageForm[] WH = new ImageForm[3];
        UIForm HudTop = null;
        public Texture2D CurProjImg = null;
        public int CurSlot = 0;
        public ScopeProjector[] Slots = new ScopeProjector[3];
        public ImageForm CurSlotF;
        public void RebuildWeaponHud()
        {

            if(CurSlotF == null)
            {

                CurSlotF = new ImageForm().Set(0, 0, 44, 44).SetImage(CurProjImg) as ImageForm;
                UI.CurUI.Top.Add(CurSlotF);


            }

            for(int i = 0; i < Slots.Length; i++)
            {

                WH[i].Forms.Clear();
                if (Slots[i] != null)
                {

                    var pimg = Slots[i].HudImg;

                    var sf = new ImageForm().Set(3, 3, WH[i].W - 6, WH[i].H - 6).SetImage(pimg) as ImageForm;

                    WH[i].Add(sf);


                }
                if(CurSlot==i)
                {

                    CurSlotF.Set(WH[i].GX, WH[i].GY + 6, 44, 44);

                }

            }

        }

        public CharScopeNine() : base(86,86)
        {

            SetImage(new Tex2D("Corona/Entity/Chars/ScopeNine/Idle1.png",true));
            CastShadow = true;
            WeaponHudImg = new Texture2D("Corona/img/icon/weaponhud1.png",LoadMethod.Single, true);
            PulseAimImg = new Tex2D("Corona/img/icon/pulseaim1.png", true);
            CurProjImg = new Texture2D("Corona/img/icon/curproj.png", LoadMethod.Single, true);

            HudTop = new UIForm().Set(0, 0, AppInfo.W, AppInfo.H);

            WH[0] = new ImageForm().Set(20, 20, 44, 44).SetImage(WeaponHudImg) as ImageForm;
            WH[1] = new ImageForm().Set(66, 20, 44, 44).SetImage(WeaponHudImg) as ImageForm;
            WH[2] = new ImageForm().Set(110, 20, 44, 44).SetImage(WeaponHudImg) as ImageForm;


            UI.CurUI.Top = HudTop;



            HudTop.Add(WH[0]);
            HudTop.Add(WH[1]);
            HudTop.Add(WH[2]);

            Slots[0] = new BigShotProjector();
            Slots[1] = new LongShotProjector();


            RebuildWeaponHud();

            Z = 1;
            ShadowPlane = 1;
            AddAnim("Walk1", new SpriteAnim("Corona/Entity/Chars/ScopeNine/AnimWalk/",0.5f));
            AddAnim("Run1",new SpriteAnim("Corona/Entity/Chars/ScopeNine/AnimWalk/",1.0f));
            AddAnim("Idle1", new SpriteAnim("Corona/Entity/Chars/ScopeNine/AnimIdle1/", 0.35f));
            SetAnim("Idle1");
            ImgFrame = GetAnimFrame();
            Drag = 0.94f;


        }

        bool changedSlot = false;
        int lmz = 0;
        int lastJump = 0;
        public float aimAngle = 0;
        public GraphLight aimLight = null;
        bool usePad = true;
        public GraphSprite AimSprite = null;
        public override void Update()
        {

            if (AimSprite == null)
            {
                AimSprite = new GraphSprite(PulseAimImg, null);
                AimSprite.X = X;
                AimSprite.Y = Y;
                Graph.Add(AimSprite);
                aimLight = new GraphLight();
                aimLight.Diffuse = new OpenTK.Vector3(0, 0.2f, 0.2f);
                aimLight.Range = 150;

                Graph.Add(aimLight);




            }
            else
            {

                if (usePad)
                {

                    float xi = XIn.rightX();
                    float yi = XIn.rightY();

                    float dis = (float)Math.Sqrt((xi * xi) + (yi * yi));

                    if (dis > 0.5)
                    {

                        float ang =(float) Math.Atan2(-yi, xi);

                        aimAngle = OpenTK.MathHelper.RadiansToDegrees(ang);


                        //aimAngle = ang;


                        AimSprite.Rot = aimAngle;

                        float xo = (float)Math.Cos(ang);
                        float yo = (float)Math.Sin(ang);

                        xo = xo * 64;
                        yo = yo * 64;


                        AimSprite.X = X + xo;
                        AimSprite.Y = Y + yo;
                        aimLight.X = AimSprite.X;
                        aimLight.Y = AimSprite.Y;
                    }
                
                }
                else
                {
                    float rx = RealX;
                    float ry = RealY;

                    float mx = Vivid.Input.Input.MX;
                    float my = Vivid.Input.Input.MY;

                    float ang = (float)Math.Atan2(my - ry, mx - rx);

                    aimAngle = ang;

                    AimSprite.Rot = OpenTK.MathHelper.RadiansToDegrees(aimAngle);

                    //Console.WriteLine("Rot:" + AimSprite.Rot);

                    float xo = (float)Math.Cos(aimAngle);
                    float yo = (float)Math.Sin(aimAngle);

                    xo = xo * 64;
                    yo = yo * 64;


                    AimSprite.X = X + xo;
                    AimSprite.Y = Y + yo;
                    aimLight.X = AimSprite.X;
                    aimLight.Y = AimSprite.Y;
                }
            }


            if (!changedSlot)
            {
                if (XIn.DLeft())
                {
                    CurSlot--;
                    if(CurSlot<0)
                    {
                        if (Slots[2] == null)
                        {
                            CurSlot = 1;
                        }
                        else
                        {
                            CurSlot = 2;
                        }
                    }
                    changedSlot = true;
                    RebuildWeaponHud();
                }
                if (XIn.DRight())
                {
                    CurSlot++;
                    if(CurSlot == 3)
                    {
                        CurSlot = 0;
                    }
                    if (Slots[CurSlot] == null)
                    {
                        CurSlot = 0;
                    }
                    changedSlot = true;
                    RebuildWeaponHud();
                }
            }
            else
            {
                if(XIn.DLeft()==false && XIn.DRight() == false)
                {
                    changedSlot = false;
                }
            }

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

            if (XIn.bX() || Input.KeyIn(OpenTK.Input.Key.Space))
            {
                if (onGround && Environment.TickCount>(lastJump+500) )
                {

                    Move2D(0, -10);
                    onGround = false;
                    //State = ScopeState.Jumping;
                    Y = Y - 10;

                    lastJump = Environment.TickCount;
                }
            }

            if (XIn.leftB() || Input.KeyIn(OpenTK.Input.Key.ShiftLeft))
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



            if (Environment.TickCount > (lastJump + 500))
            {
                var hit = cm.RayCast(cx, cy, cx, cy + 60 * Graph.Z);

                if (hit == null)
                {
                    //X = hit.HitX;
                    Move2D(0, 0.3f);
                    onGround = false;
                }
                else
                {
                    float dis = hit.HitY - 50 * Graph.Z;

                    dis = Math.Abs((dis - Y));


                    if (dis < 32*Graph.Z)
                    {

                        Yi = 0;
                        //Y = cy; // hit.HitY - 32 * Graph.Z;
                        onGround = true;
                    }

                }
            }

            //Rot += 0.1f;
            
            //vironment.Exit(1);
            //base.Update();

        }

    }
}
