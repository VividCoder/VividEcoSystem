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
        public UIForm body, title;

        public WindowForm()
        {
            if (Shadow == null)
            {
                Shadow = new Texture2D("data/UI/Shadow1.png", LoadMethod.Single, true);
                TitleImg = new Texture2D("data/nxUI/window/title2.png", LoadMethod.Single, true);
                BodyImg = new Texture2D("data/nxUI/bg/winBody4.png", LoadMethod.Single, true);
                BodyNorm = new Texture2D("data/UI/normal/winnorm5.jpg", LoadMethod.Single, false);
            }

            title = new ButtonForm().Set(0, 0, W, 20, Text).SetImage(TitleImg);

            body = new ImageForm().Set(0, 20, W - 100, H - 22, "").SetImage(BodyImg, BodyNorm).SetPeak(true, false);
            body.Peak = false;
            body.Refract = false;

            //body.Blur = 0.1f;
            // body.RefractV = 0.72f;

            resize = (ButtonForm)new ButtonForm().Set(W - 14, H - 14, 14, 14, "");

            AfterSet=()=>{


                
                title.Text = Text;


            };

            void ResizeDrag(int x, int y)
            {
                if (Docked) return;
                if (LockedSize)
                {
                    return;
                }

                Set(X, Y, W + x, H + y, Text);
                body.Set(0, 22, W, H - 24, "");
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

            title.Drag = DragFunc;

            Add(title);
            Add(body);
            //  Add(resize);

            void ChangedFunc()
            {
               // title.Text = Text;
                title.W = W;
                title.H = 20;
                body.H = H - 26;
                body.W = W;
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