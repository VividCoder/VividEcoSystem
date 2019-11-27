using OpenTK;
using System;
using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public delegate void ValueChanged(float v);

    public class ScrollBarV : UIForm
    {
        public float Cur = 0;
        public float Max = 1;
        public float CurView = 0.1f;
        public static Texture2D But;
        public ButtonForm ScrollBut;
        public ValueChanged ValueChange = null;

        public void SetMax(float max)
        {
            Max = max;
            float ys = H / Max;

            float by = H * ys;
            ScrollBut.H = (int)by;
            if (Max < H)
            {
                NotUse = true;
            }
            else
            {
                NotUse = false;
            }
        }

        private bool NotUse = false;
        public static Texture2D ScrollTex = null;
        public ScrollBarV()
        {
            if (ScrollTex == null)
            {
                ScrollTex = new Texture2D("data/nxUI/slider/slider1.png", LoadMethod.Single, true);
            }
            if (But == null)
            {
                But = new Texture2D("data/UI/Skin/but_normal.png", LoadMethod.Single, true);
            }
            ScrollBut = new ButtonForm().Set(0, 0, 10, 10, "/\\") as ButtonForm;
            ScrollBut.CoreTex = ScrollTex;

            void DrawFunc()
            {
                float DY = (GY + Y);

                float AY = Cur / Max;

                DrawFormSolid(new Vector4(0.3f, 0.3f, 0.3f, 0.8f));
            }

            void ChangedFunc()
            {
                ScrollBut.X = 0;
                //ScrollBut.Y = 0;

                ScrollBut.W = W;
                ScrollBut.H = 20;
            }

            Changed = ChangedFunc;

            Draw = DrawFunc;
            Add(ScrollBut);

            void DragFunc(int x, int y)
            {
                if (NotUse) return;
                ScrollBut.Y += y;
                Console.WriteLine("Y:" + y + " SY:" + ScrollBut.Y);

                if (ScrollBut.Y < 0)
                {
                    ScrollBut.Y = 0;
                }

                if (ScrollBut.Y > (H - ScrollBut.H))
                {
                    ScrollBut.Y = (H - ScrollBut.H);
                }

                float xs = (float)ScrollBut.Y / ((float)H - (float)ScrollBut.H);

                Cur = Max * xs;

                ValueChange?.Invoke(Cur);
            }

            ScrollBut.Drag = DragFunc;
        }
    }
}