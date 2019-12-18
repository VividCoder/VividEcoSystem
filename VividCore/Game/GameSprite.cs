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
namespace Vivid.Game
{
    public class GameSprite : GraphSprite 
    {

        public Dictionary<string, SpriteAnim> Anims = new Dictionary<string, SpriteAnim>();
        public SpriteAnim CurAnim;
        public int CurFrame = 0;
        public float CurTime = 0.0f;
        public float XI, Yi, ZI;
        public float Drag = 0;
        public float GravY = 0;
        public bool FlipX = false;

        public GameSprite(int w,int h) : base(w,h)
        {
            XI = Yi = ZI = 0;
            Drag = 0.97f;
        }

        public virtual void Move2D(float x,float y)
        {
            XI = XI + x;
            Yi = Yi + y;
        }

        public void SetAnim(string name)
        {
            CurAnim = Anims[name];
            CurFrame = 0;
        }

        public Tex2D GetAnimFrame()
        {
            if(CurFrame>=CurAnim.Frames.Count-1)
            {
                CurFrame = 0;
            }
            if (CurAnim.Frames.Count == 1)
            {
                CurFrame = 0;
            }
            return CurAnim.GetFrame(CurFrame);
        }

        public override void Update()
        {
            if (CurAnim != null)
            {
                CurTime += CurAnim.Speed;
                CurFrame = (int)CurTime;
                if (CurFrame >= CurAnim.Frames.Count-1)
                {
                    CurTime = 0;
                }
            }
            X += XI;
            Y += Yi;
            XI = XI * Drag;
            Yi = Yi * Drag;
        }

        public void AddAnim(string name,SpriteAnim a)
        {
            Anims.Add(name, a);
        }

    }
}
