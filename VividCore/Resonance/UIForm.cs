using OpenTK;
using System.Collections.Generic;
using Vivid.Draw;
using Vivid.Texture;

namespace Vivid.Resonance
{
    public delegate void Draw();

    public delegate void Update();

    public delegate void MouseEnter();

    public delegate void MouseLeave();

    public delegate void MouseMove(int x, int y, int mx, int my);

    public delegate void MouseWheel(float z);

    public delegate void MouseDown(int but);

    public delegate void MouseUp(int but);

    public delegate void MousePressed(int but);

    public delegate void FormLogic();

    public delegate void Click(int b);

    public delegate void Activate();

    public delegate void Deactivate();

    public delegate void KeyPressed(OpenTK.Input.Key key, bool shift);

    public delegate void Drag(int x, int y);

    public delegate void ChangedInfo();

    public delegate void KeysIn(List<OpenTK.Input.Key> keys);

    public delegate void DraggedObj(Forms.DragObject obj);

    public class UIForm
    {
        public static Font2.OrchidFont Font = null;
        public bool Clip = true;
        public bool CheckBounds = true;
        public bool PushArea = false;


        public Vector4 Col = new Vector4(1, 1, 1, 0.7f);
        public float Blur = 0.4f;
        public float RefractV = 0.4f;

        public FormResized Resized = null;

        public ChangedInfo Changed = null;
        public ChangedInfo SubChanged = null;
        public ChangedInfo AfterSet = null;
        public Draw Draw = null;
        public Update PreDraw = null;
        public Update Update = null;
        public MouseEnter MouseEnter = null;
        public MouseLeave MouseLeave = null;
        public MouseMove MouseMove = null;
        public MouseDown MouseDown = null;
        public MouseUp MouseUp = null;
        public MouseWheel MouseWheelMoved = null;
        public MousePressed MousePressed = null;
        public FormLogic FormLogic = null;
        public Click Click = null;
        public Drag Drag = null;
        public Click DoubleClick = null;
        public Drag PostDrag = null;
        public Activate Activate = null;
        public Deactivate Deactivate = null;
        public Texture2D CoreTex = null;
        public Texture2D NormTex = null;
        public KeyPressed KeyPress = null;
        public KeysIn KeysIn = null;
        public int X = 0, Y = 0;
        public int W = 0, H = 0;
        //public 

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                System.Console.WriteLine("Text:" + this.GetType().Name + ":" + value);
                _text = value;
                
            }
            

        }
        private string _text = "";
        public DockMethod DockStyle = DockMethod.None;
        public UIForm Root = null;
        public List<UIForm> Forms = new List<UIForm>();
        public bool Docked = false;
        public DraggedObj DraggedObj = null;

        public FrameBuffer.FrameBufferColor FormFB = null;

        public int OffX = 0, OffY = 0;

        public bool Peak = false;
        public bool Refract = false;

        public bool Open = true;

        public bool CanDrag = false;
        public bool CanDrop = false;

        public Forms.ContextMenuForm ContextMenu = null;

        public int ViewX = 0, ViewY = 0, ViewW = 0, ViewH = 0;

        public static Texture2D WhiteTex = null;
        public static Vivid.Tex.Tex2D WhiteTex2D = null;

        public UIForm SetPeak(bool peak, bool refract)
        {
            Peak = peak;
            Refract = refract;
            return this;
        }

        public UIForm Add(UIForm form)
        {
            foreach (var cform in Forms)
            {
                if (cform == form) return form;
            }
            Forms.Add(form);
            form.Root = this;
            return form;
        }

        public void Resize()
        {

            Resized?.Invoke();

            foreach (var f in Forms)
            {
                f.Resize();
            }

        }

        public UIForm Add(params UIForm[] forms)
        {
            foreach (var f in forms)
            {
                Add(f);
            }

            return forms[0];
        }

        public int GX
        {
            get
            {
                int x = 0;
                if (Root != null)
                {
                    x = x + Root.GX;
                }
                x = x + X;
                return x;
            }
        }

        public int GY
        {
            get
            {
                int y = 0;
                if (Root != null)
                {
                    y = y + Root.GY;
                }
                else
                {
                    if (UI.Menu != null & UI.Menu != this)
                    {
                        y = y + 20;

                    }
                }
                y = y + Y;
                return y;
            }
        }

        public virtual void DesignUI()
        {
        }

        public bool InBounds(int x, int y)
        {
            if (x >= GX + OffX && y >= GY + OffY && x <= (GX + OffX + W) && y <= (GY + OffY + H))
            {
                return true;
            }
            return false;
        }

        public void DrawLine(int x, int y, int x2, int y2, OpenTK.Vector4 col)
        {
            // Pen2D.BlendMod = PenBlend.Solid;

            //  Pen2D.Line(GX + x, GY + y, GX + x2, GY + y2);
        }

        public void DrawForm(Texture2D tex, int x = 0, int y = 0, int w = -1, int h = -1, bool flipuv = false)
        {
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if (w != -1)
            {
                dw = w;
                dh = h;
            }
            Vivid.Draw.IntelliDraw.DrawImg(GX + x + OffX, GY + y + OffY, dw, dh, tex, Col, flipuv);

            // Pen2D.Rect(GX + x + OffX, GY + y + OffY, dw, dh, tex, Col * UI.BootAlpha);
        }

        public void DrawFormBlur(Texture2D tex, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            DrawFormBlur(tex, Blur, Col, x, y, w, h);
        }

        public void DrawFormBlurRefract(Texture2D tex, Texture2D norm, float blur, Vector4 col, float refract, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            return;
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if (w != -1)
            {
                dw = w;
                dh = h;
            }

            Texture2D btex = new Texture2D(dw, dh);

            btex.CopyTex(GX + x + OffX, App.VividApp.H - ((GY + y + OffY) + dh));

            Pen2D.RectBlurRefract(GX + x + OffX, GY + y + OffY, dw, dh, tex, btex, norm, col * UI.BootAlpha, col * UI.BootAlpha, blur, refract);

            btex.Delete();
        }

        public void SetVP()
        {
            return;
            if (ViewW > 0)
            {
                //Console.WriteLine("W:" + App.AppInfo.W + " H:" + App.AppInfo.H);
                OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ScissorTest);
                OpenTK.Graphics.OpenGL4.GL.Scissor(ViewX, App.AppInfo.H - (ViewY + ViewH), ViewW, ViewH);

                // GL.Viewport(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
            }
        }

        public void DrawFormSolid(Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            if (w == -1)
            {
                w = W;
                h = H;
            }
            Vivid.Draw.IntelliDraw.DrawImg(GX + x + OffX, GY + y + OffY, w, h, WhiteTex, col * UI.BootAlpha);
            //Pen2D.Rect(GX + x + OffX, GY + y + OffY, w, h, col * UI.BootAlpha);
        }

        public void DrawFormSolidFree(Vector4 col, int x, int y, int w, int h)
        {
            Vivid.Draw.IntelliDraw.DrawImg(x, y, w, h, WhiteTex, col);

            //Pen2D.Rect(x, y, w, h, col);
        }

        public void DrawFormBlur(Texture2D tex, float blur, Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            return;
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if (w != -1)
            {
                dw = w;
                dh = h;
            }

            Texture2D btex = new Texture2D(dw, dh);

            btex.CopyTex(GX + x + OffY, App.VividApp.H - ((GY + y + OffY) + dh));

            Pen2D.RectBlur(GX + x + OffX, GY + y + OffY, dw, dh, tex, btex, col * UI.BootAlpha, col * UI.BootAlpha, blur);

            btex.Delete();
        }

        public void DrawForm(Texture2D tex, Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            Pen2D.BlendMod = PenBlend.Alpha;

            int dw = W;
            int dh = H;

            if (w != -1)
            {
                dw = w;
                dh = h;
            }
            Vivid.Draw.IntelliDraw.DrawImg(GX + x + OffX, GY + y + OffY, dw, dh, tex, col * UI.BootAlpha);
        }

        public void DrawText(string txt, int x, int y)
        {
            DrawText(txt, x, y, new Vector4(0, 0, 0, 1) * UI.BootAlpha);
        }

        public void SetClip(int x, int y, int w, int h)
        {
            ViewX = x;
            ViewY = y;
            ViewW = w;
            ViewH = h;
            foreach (var c in Forms)
            {
                c.SetClip(x, y, w, h);
            }
        }

        public void DrawText(string txt, int x, int y, Vector4 col)
        {
            if (txt == "") return;
            if (Font == null)
            {
                Font = new Font2.OrchidFont("data/font/font1.ttf", 12);
            }

            var tex = Font.GenString(txt);
            if (tex != null)
            {
                DrawForm(tex, col, x, y + tex.H / 2, tex.W, tex.H);
            }

            //FontRenderer.Draw(UI.Font, txt, GX + x + OffX, GY + y + OffY, col * UI.BootAlpha);
        }

        public void DrawTextFree(string text, int x, int y, Vector4 col)
        {
            //FontRenderer.Draw(UI.Font, text, x, y, col);
        }

        public UIForm Set(int x, int y, int w, int h, string text = "")
        {
            if (WhiteTex == null)
            {
                WhiteTex = new Texture2D("data/ui/whitetex.png", LoadMethod.Single, true);
                WhiteTex2D = new Tex.Tex2D("data/ui/whitetex.png", false);
            }
            X = x;
            Y = y;
            W = w;
            H = h;




            Text = text;
            Changed?.Invoke();
            if (!designed)
            {
                designed = true;
                DesignUI();
            }
            AfterSet?.Invoke();
            // FormFB = new FrameBuffer.FrameBufferColor(W, H);
            Resize();
            return this;
        }

        private bool designed = false;

        public UIForm()
        {
            ViewX = 0;
            ViewY = 0;
            ViewW = App.AppInfo.W;
            ViewH = App.AppInfo.H;
            //   UI.UIChanged = true;
        }

        public UIForm SetImage(Texture2D tex, Texture2D norm = null)
        {
            CoreTex = tex;
            NormTex = norm;
            return this;
        }

        public bool Lock = false;
    }

    public delegate void FormResized();
    public enum DockMethod
    {
        None, Fill, Left, Right, Top, Bottom
    }
}