using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using Vivid.Draw;
using Vivid.State;
using NLua;

namespace Vivid.App
{
    public static class AppInfo
    {
        public static int W, H;
        public static bool Full;
        public static string App;
        public static int RW, RH;
    }

    public class VForm : VividApp
    {
        public static void SetSize(int w, int h)
        {
            AppInfo.W = w;
            AppInfo.H = h;
            AppInfo.RW = w;
            AppInfo.RH = h;
            SetVP.Set(0, 0, w, h);

            GL.Scissor(0, 0, w, h);
            Pen2D.SetProj(0, 0, w, h);
        }

        private static bool done = false;

        public static void Set(int w, int h)
        {
            GL.ClearColor(System.Drawing.Color.AliceBlue);
            GL.Enable(EnableCap.DepthTest);
            AppInfo.W = w;
            AppInfo.H = h;
            AppInfo.RW = w;
            AppInfo.RH = h;
            AppInfo.Full = false;
            AppInfo.App = "GLApp";
            if (!done)
            {
                Import.Import.RegDefaults();
            }
            SetVP.Set(0, 0, w, h);
            GL.Scissor(0, 0, w, h);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ScissorTest);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthRange(0, 1);

            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Less);
            // UI.UISys.ActiveUI.OnResize(Width, Height);
            Pen2D.SetProj(0, 0, w, h);
            if (!done)
            {
                Pen2D.InitDraw();
            }
            AppInfo.W = w;
            AppInfo.H = h;
            AppInfo.RW = w;
            AppInfo.RH = h;

            SetVP.Set(0, 0, w, h);
            GL.Scissor(0, 0, w, h);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ScissorTest);
            // GL.Disable(EnableCap.Lighting);

            //GL.DepthFunc(DepthFunction.Greater);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthRange(0, 1);

            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Lequal);

            Pen2D.SetProj(0, 0, w, h);

            done = true;
        }
    }

    public class VividApp : GameWindow
    {
        public static VividApp Link = null;
        private string _Title = "";
        private OpenTK.Graphics.Color4 _BgCol = OpenTK.Graphics.Color4.Black;
        public static int W, H;
        //        public static int RW, RH;


        public static VividState InitState = null;

        public static Stack<VividState> States = new Stack<VividState>();

        public static VividState ActiveState
        {
            get
            {
                if (States.Count == 0)
                {
                    return null;
                }

                return States.Peek();
            }
        }

        public VividApp()
        {
        }

        public static void PushState(VividState state, bool start = true)
        {
            States.Push(state);
            VividApp.Link.StateInited = false;
        }

        public static void PopState()
        {
            VividState ls = States.Pop();
            ls.StopState();
            ls.Running = false;
        }

        public OpenTK.Graphics.Color4 BgCol
        {
            get => _BgCol;
            set => _BgCol = value;
        }

        public string AppName
        {
            get => _Title;
            set { _Title = value; Title = value; }
        }

        public void MakeFixed()
        {
            WindowBorder = WindowBorder.Fixed;
        }

        public void MakeWindowless()
        {
            WindowBorder = WindowBorder.Hidden;
        }

        public void MakeFullscreen()
        {
            WindowState = WindowState.Fullscreen;
        }

        public VividApp(string app, int width, int height, bool full) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, app, full ? GameWindowFlags.Fullscreen : GameWindowFlags.Default, DisplayDevice.Default)

        {
            Link = this;
            _Title = app;
            AppInfo.W = width;
            AppInfo.H = height;
            AppInfo.RW = width;
            AppInfo.RH = height;
            AppInfo.Full = full;
            AppInfo.App = app;
            AppInfo.W = width;
            AppInfo.H = height;
            AppInfo.RW = width;
            AppInfo.RH = height;
            Import.Import.RegDefaults();
            Pen2D.InitDraw();
            //CursorVisible = false;
            CursorVisible = true;
            VSync = VSyncMode.Adaptive;
            Font2.OrchidFont.Init();
            Physics.PhysicsManager.InitSDK();
            Lua state = new Lua();
            object res = state.DoString("return 1+1")[0];
            Console.WriteLine("Returned Version:" + res);

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            int bid = 0;
            bid = GetBID(e);
            Input.Input.MB[bid] = true;
        }

        private static int GetBID(MouseButtonEventArgs e)
        {
            int bid = 0;
            switch (e.Button)
            {
                case MouseButton.Left:
                    bid = 0;
                    break;

                case MouseButton.Right:
                    bid = 1;
                    break;

                case MouseButton.Middle:
                    bid = 2;
                    break;
            }

            return bid;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            int bid = GetBID(e);
            Input.Input.MB[bid] = false;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Input.Input.MZ = e.ValuePrecise;
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Input.Input.MX = e.Position.X;
            Input.Input.MY = e.Position.Y;

        }

        private readonly bool fs = true;
        private MouseState lm;

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Input.Input.SetKey(e.Key, true);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Input.Input.SetKey(e.Key, false);
        }

        protected override void OnResize(EventArgs e)
        {
            SetGL();
            AppInfo.W = Width;
            AppInfo.H = Height;
            if (States.Count > 0)
            {
                VividState us = States.Peek();
                us.ResizeState(Width, Height);
                //us.UpdateState();
                // us.InternalUpdate();
            }
        }

        private void SetGL()
        {
            AppInfo.W = Width;
            AppInfo.H = Height;
            AppInfo.RW = Width;
            AppInfo.RH = Height;
            //Console.WriteLine("W:" + Width + " H:" + Height);

            SetVP.Set(0, 0, Width, Height);
            GL.Scissor(0, 0, Width, Height);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.StencilTest);
            GL.Disable(EnableCap.ScissorTest);
            // GL.Disable(EnableCap.Lighting);

            //GL.DepthFunc(DepthFunction.Greater);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthRange(0, 1);

            GL.ClearDepth(1.0f);
            GL.DepthFunc(DepthFunction.Lequal);
        }

        protected override void OnLoad(EventArgs e)
        {
            //CursorVisible = true;
            Pen2D.SetProj(0, 0, Width, Height);
            SetGL();
            InitApp();
        }

        public virtual void InitApp()
        {
        }

        public virtual void UpdateApp()
        {
        }

        public virtual void DrawApp()
        {
        }
        public bool StateInited = false;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (InitState != null)
            {
                PushState(InitState);
                InitState = null;
            }

            GC.Collect();

            //Pen2D.UpdateCache();

            UpdateApp();
            Physics.PhysicsManager.Update(0.1f);
            if (States.Count > 0)
            {
                VividState us = States.Peek();
                if (StateInited == false)
                {
                    us.InitState();
                    us.StartState();
                    StateInited = true;
                    GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
                    GL.Disable(EnableCap.ScissorTest);
                }
                us.UpdateState();
                us.InternalUpdate();
            }
        }

        public int fpsL = 0, fps = 0, frames = 0;

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            CursorVisible = true;
            bool changefps = false;
            if (Environment.TickCount > fpsL + 1000)
            {
                fpsL = Environment.TickCount + 1000;
                fps = frames;
                frames = 0;
                changefps = true;
            }
            frames++;
            if (changefps)
            {
                Title = AppName;
                Title += " FPS:" + fps;
            }
            _BgCol = new OpenTK.Graphics.Color4(0, 0, 0, 1.0f);
            GL.ClearColor(_BgCol);

            // GL.DepthMask(true);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawApp();
            if (States.Count > 0)
            {
                VividState rs = States.Peek();
                if (StateInited == false)
                {
                    rs.InitState();
                    rs.StartState();
                    StateInited = true;
                    GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
                    GL.Disable(EnableCap.ScissorTest);
                }
                rs.DrawState();
            }

            SwapBuffers();
        }
    }

    public static class SetVP
    {
        public static int X = 0, Y = 0, W = 0, H = 0;

        public static void Set(int x, int y, int w, int h)
        {

            GL.Viewport(x, y, w, h);
            X = x;
            Y = y;
            W = w;
            H = h;

        }
    }
}