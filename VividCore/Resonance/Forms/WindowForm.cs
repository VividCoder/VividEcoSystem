using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public class WindowForm : UIForm
    {
        public static Texture2D TitleImg = null;
        public static Texture2D BodyImg = null;
        public static Texture2D BodyNorm = null;
        private ButtonForm resize;

        public bool LockedPos = false;
        public bool LockedSize = false;
        public static Texture2D Shadow = null;
        public UIForm Body, Title;

        public WindowForm()
        {
            if (Shadow == null)
            {
                Shadow = new Texture2D("data/UI/Shadow1.png", LoadMethod.Single, true);
                TitleImg = new Texture2D("data/nxUI/window/title3.png", LoadMethod.Single, true);
                BodyImg = new Texture2D("data/nxUI/bg/winBody5.png", LoadMethod.Single, true);
                BodyNorm = new Texture2D("data/UI/normal/winnorm5.jpg", LoadMethod.Single, false);
            }

            Title = new ButtonForm().Set(0, 0, W, 20, Text).SetImage(TitleImg);

            Body = new ImageForm().Set(0, 20, W - 100, H - 22, "").SetImage(BodyImg, BodyNorm).SetPeak(true, false);
            Body.Peak = false;
            Body.Refract = false;

            //body.Blur = 0.1f;
            // body.RefractV = 0.72f;

            resize = (ButtonForm)new ButtonForm().Set(W - 14, H - 14, 14, 14, "");

            AfterSet=()=>{


                
                Title.Text = Text;


            };

            void ResizeDrag(int x, int y)
            {
                if (Docked) return;
                if (LockedSize)
                {
                    return;
                }

                Set(X, Y, W + x, H + y, Text);
                Body.Set(0, 22, W, H - 24, "");
                resize.X = W - 14;
                resize.Y = H - 14;
            }

            resize.Drag = ResizeDrag;

            void DragFunc(int x, int y)
            {
                if (LockedPos)
                {
                    return;
                }
                if (Docked) return;
                X = X + x;
                Y = Y + y;
            }

            Title.Drag = DragFunc;

            Add(Title);
            Add(Body);
            //  Add(resize);

            void ChangedFunc()
            {
               // title.Text = Text;
                Title.W = W;
                Title.H = 20;
                Body.H = H - 26;
                Body.W = W;
                resize.X = W - 14;
                resize.Y = H - 20;
                SubChanged?.Invoke();
            }

            Changed = ChangedFunc;

            void DrawFunc()
            {
                // DrawFormBlur ( Shadow, 0.1f, new Vector4 ( 0.9f, 0.9f, 0.9f, 0.98f ), 5, 5, W + 30, H + 30 );
                //DrawForm(TitleImg, 0, 0, W, 20);
            }

            Draw = DrawFunc;
        }

        public WindowForm NoResize()
        {
            Forms.Remove(resize);
            return this;
        }
    }
}