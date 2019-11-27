using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using Vivid.Font;
using Vivid.Resonance.Forms;



namespace Vivid.Resonance
{
    public class UI
    {
        public static MenuForm Menu = null;
        public static Texture.Texture2D WhiteTex = null;
        public static UIForm Active = null;
        public static Font.Font2D Font = null;
        public static int MX, MY, MXD, MYD;
        public static UIForm TopForm = null;
        public int clicks = 0;
        public bool FirstMouse = true;
        //public Logics Graphics = new Logics();
        public int lastClick = 0;
        public OpenTK.Input.Key LastKey = OpenTK.Input.Key.LastKey;
        //public Logics Logics = new Logics();
        public int NextKey = 0;
        public UIForm[] Pressed = new UIForm[32];
        public List<UIForm> RenderList = new List<UIForm>();
        public UIForm Root = new UIForm();
        public UIForm Top = null;
        public List<UIForm> UpdateList = new List<UIForm>();
        public static float BootAlpha = 0.0f;
        public Vivid.Texture.Texture2D CursorImg;
        public UIDock Dock = new UIDock();
        public UIForm WinTop = null;
        public List<UIForm> Overlay = new List<UIForm>();
        private readonly int ux;
        public Forms.MenuForm ActiveMenu2 = null;
        private readonly int uy;

        private Texture.Texture2D Black = null;

        private bool sdrag = true;

        private int sdx;

        private int sdy;

        private float TopB = 0.0f;
        public static UI CurUI = null;

        public UI FullScreen = null;

        public bool FullScreenOn
        {
            get;
            set;
        }

        public Forms.ContextMenuForm ActiveMenu = null;

        public Forms.DragObject DragObj = null;

        // public Audio.VSoundSource WindRush = null;
        // public Audio.VSound WindSound = null;

        public List<EventForm> Events = new List<EventForm>();

        public static void Reset()
        {
            UI.Active = null;
            UI.BootAlpha = 0.0f;
            UI.CurUI = null;
            UI.TarAlpha = 1.0f;
            UI.TopForm = null;

        }

        public UI()
        {
            FullScreenOn = false;
            if (WhiteTex == null)
            {
                WhiteTex = new Texture.Texture2D("data/ui/skin/white.png", Texture.LoadMethod.Single, true);
            }
            CurUI = this;
            InitUI();
            for (int i = 0; i < 32; i++)
            {
                Pressed[i] = null;
            }
            Root = new UIForm().Set(0, 0, App.AppInfo.W, App.AppInfo.H);
            //   WindRush = new Audio.VSoundSource("data/audio/wind1.wav");
            //  WindSound = WindRush.Play2D(true);
            //  WindSound.Pitch = 0;
            //  WindSound.Vol = 0;
        }

        public void InitUI()
        {
            Black = new Texture.Texture2D("data/ui/black.png", Texture.LoadMethod.Single, false);
            Font = new Font2D("data/font/times.ttf.vf");
            CursorImg = new Texture.Texture2D("data/ui/cursor1.png", Vivid.Texture.LoadMethod.Single, true);
        }
        float MZD, MZ;
        public void Update()
        {

            if (DragObj != null)
            {
                DragObj.X = MX - 5;
                DragObj.Y = MY - 5;
            }

            if (BootAlpha != TarAlpha)
            {
                BootAlpha += (TarAlpha - BootAlpha) * 0.07f;
            }

            if (FirstMouse)
            {
                MX = Input.Input.MX;
                MY = Input.Input.MY;
                MZ = Input.Input.MZ;
                FirstMouse = false;
            }


            MXD = Input.Input.MX - MX;
            MYD = Input.Input.MY - MY;
            MZD = Input.Input.MZ - MZ;
            MX = Input.Input.MX;
            MY = Input.Input.MY;
            MZ = Input.Input.MZ;


            //UpdateList.Clear();



            var list = new List<UIForm>();
            UpdateList = list;

            AddToList(list, Root);
            AddToList(list, ActiveMenu);
            AddToList(list, DragObj);
            foreach (var form in Overlay)
            {
                AddToList(list, form);
            }
            //AddToList(list, Dock);
            AddToList(list, Dock);
            AddToList(list, Top);
            if (Menu != null)
            {
                AddToList(list, Menu);
            }
            list.Reverse();

            foreach (var form in list)
            {
                form.Update?.Invoke();
            }



            UpdateInput();

        }
        public void Render()
        {

            var list = GenRenderList();
            AddToList(list, ActiveMenu);
            AddToList(list, DragObj);
            foreach (var form in Overlay)
            {
                AddToList(list, form);
            }
            AddToList(list, Dock);
            AddToList(list, Top);
            if (Menu != null)
            {
                AddToList(list, Menu);
            }
            foreach (var form in list)
            {
                form.PreDraw?.Invoke();
            }


            //Vivid.Draw.IntelliDraw.BeginDraw();

            //GL.Viewport(0, 0, App.AppInfo.W, App.AppInfo.H);
            Vivid.Draw.IntelliDraw.Draw_Z = 0.001f;

            foreach (var form in list)
            {

                //  GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                if (form.Clip)
                {
                    //      GL.Enable(EnableCap.ScissorTest);
                }
                else
                {
                    //     GL.Disable(EnableCap.ScissorTest);
                }


                //                form.PreDraw?.Invoke();
                Vivid.Draw.IntelliDraw.BeginDraw(true);
                form.Draw?.Invoke();
                Vivid.Draw.IntelliDraw.EndDraw();

            }

            //            Vivid.Draw.IntelliDraw.EndDraw();

        }

        public List<UIForm> GenRenderList()
        {
            List<UIForm> rl = new List<UIForm>();

            AddToList(rl, Root);

            return rl;
        }


        public void AddToList(List<UIForm> list, UIForm node)
        {
            if (node == null) return;
            list.Add(node);
            foreach (var sub_form in node.Forms)
            {
                AddToList(list, sub_form);
            }

        }

        public void Render2()
        {
            if (FullScreenOn)
            {
                FullScreen.Root.Set(0, 0, App.AppInfo.W, App.AppInfo.H);
                FullScreen.Render();

                return;
            }

            // Vivid.Draw.IntelliDraw.BeginDraw();

            if (Top != null)
            {
                TopB = TopB + 0.045f;
                if (TopB > 0.8f)
                {
                    TopB = 0.8f;
                }
            }
            else
            {
                TopB = TopB - 0.088f;
                if (TopB < 0)
                {
                    TopB = 0;
                }
            }

            UIForm prev = null;

            RenderList.Clear();

            if (Top != null)
            {

                UpdateRenderList(Root);
                if (ActiveMenu != null)
                {
                    UpdateRenderList(ActiveMenu);
                }

                foreach (UIForm form in RenderList)
                {
                    GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                    //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    GL.Enable(EnableCap.ScissorTest);
                    Vivid.Draw.IntelliDraw.BeginDraw();
                    form.Draw?.Invoke();
                    Vivid.Draw.IntelliDraw.EndDraw();
                }
                if (ActiveMenu != null)
                {
                    var form = ActiveMenu;
                    GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                    //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    GL.Enable(EnableCap.ScissorTest);
                    Vivid.Draw.IntelliDraw.BeginDraw();
                    //form.Draw?.Invoke();

                    ActiveMenu.Draw.Invoke();
                    Vivid.Draw.IntelliDraw.EndDraw();
                }
                Texture.Texture2D ntex = new Texture.Texture2D(Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);

                ntex.CopyTex(0, 0);
                //  OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit);
                //  Iris3D.Draw.Pen2D.RectBlur2(0, 0, Iris3D.App.IrisApp.W, Iris3D.App.IrisApp.H, ntex, new OpenTK.Vector4(1, 1, 1, 1), TopB);

                RenderList.Clear();

                foreach (UIForm form in RenderList)
                {

                    GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                    //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    GL.Enable(EnableCap.ScissorTest);
                    Vivid.Draw.IntelliDraw.BeginDraw();
                    form.Draw?.Invoke();
                    Vivid.Draw.IntelliDraw.EndDraw();
                }
                GL.Disable(EnableCap.ScissorTest);

                RenderList.Clear();
                foreach (var f in Overlay)
                {
                    UpdateRenderList(f);
                }


                UpdateRenderList(Top);

                // RenderList.Reverse();
                GL.Disable(EnableCap.ScissorTest);
                foreach (UIForm form in RenderList)
                {

                    //form.ViewH= Vivid.App.AppInfo.H/
                    //Vivid.App.SetVP.Set(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    // GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                    //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    //   GL.Enable(EnableCap.ScissorTest);
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    Vivid.Draw.IntelliDraw.BeginDraw();
                    form.Draw?.Invoke();
                    Vivid.Draw.IntelliDraw.EndDraw();

                }


                // Vivid.App.SetVP.Set(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);

                ntex.Delete();
            }
            else
            {
                UpdateRenderList(Root);
                if (ActiveMenu != null)
                {
                    UpdateRenderList(ActiveMenu);
                }


                if (DragObj != null)
                {
                    UpdateRenderList(DragObj);
                }

                GL.DepthMask(false);

                foreach (UIForm form in RenderList)
                {

                    //form.ViewH= Vivid.App.AppInfo.H/
                    //Vivid.App.SetVP.Set(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                    //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    if (form.Clip)
                    {
                        GL.Enable(EnableCap.ScissorTest);
                    }
                    else
                    {
                        GL.Disable(EnableCap.ScissorTest);
                    }
                    // GL.Disable(EnableCap.ScissorTest);
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    GL.DepthMask(false);
                    Vivid.Draw.IntelliDraw.BeginDraw();
                    form.Draw?.Invoke();
                    Vivid.Draw.IntelliDraw.EndDraw();
                }

                GL.DepthMask(true);

                RenderList.Clear();
                foreach (var f in Overlay)
                {
                    UpdateRenderList(f);
                }
                // RenderList.Reverse();
                GL.Disable(EnableCap.ScissorTest);
                foreach (UIForm form in RenderList)
                {

                    //form.ViewH= Vivid.App.AppInfo.H/
                    //Vivid.App.SetVP.Set(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    GL.Scissor(form.ViewX, Vivid.App.AppInfo.H - (form.ViewY + form.ViewH), form.ViewW, form.ViewH);
                    //  // GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    // GL.Scissor(0, 0, App.AppInfo.W, App.AppInfo.H);
                    //  GL.Scissor(form.ViewX, form.ViewY, form.ViewW, form.ViewH);
                    //   GL.Enable(EnableCap.ScissorTest);
                    if (form.Clip)
                    {
                        GL.Enable(EnableCap.ScissorTest);
                    }
                    else
                    {
                        GL.Disable(EnableCap.ScissorTest);
                    }
                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    Vivid.Draw.IntelliDraw.BeginDraw();
                    form.Draw?.Invoke();
                    Vivid.Draw.IntelliDraw.EndDraw();

                }

                GL.Disable(EnableCap.ScissorTest);
                // Vivid.App.SetVP.Set(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);


                if (TopB > 0)
                {
                    Texture.Texture2D ntex = new Texture.Texture2D(Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);
                    ntex.CopyTex(0, 0);
                    OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit);
                    // Iris3D.Draw.Pen2D.RectBlur2(0, 0, Iris3D.App.IrisApp.W, Iris3D.App.IrisApp.H, ntex, new OpenTK.Vector4(1, 1, 1, 1), TopB);
                    ntex.Delete();
                }
            }

            RenderList.Clear();
            UpdateRenderList(Dock);
            Vivid.Draw.IntelliDraw.BeginDraw();
            foreach (var f in RenderList)
            {

                f?.Draw();
            }
            Vivid.Draw.IntelliDraw.EndDraw();
            //Dock.Draw?.Invoke();

            // Vivid.Draw.IntelliDraw.EndDraw();

            return;

            GL.Clear(ClearBufferMask.DepthBufferBit);

            Vivid.Draw.IntelliDraw.BeginDraw();

            Vivid.Draw.IntelliDraw.DrawImg(MX, MY, 24, 24, CursorImg, new Vector4(1, 1, 1, 1));

            Vivid.Draw.IntelliDraw.EndDraw();

            return;

            //  GL.Disable(EnableCap.ScissorTest);
            Vivid.Draw.Pen2D.SetProj(0, 0, Vivid.App.AppInfo.W, Vivid.App.AppInfo.H);
            Vivid.Draw.Pen2D.BlendMod = Vivid.Draw.PenBlend.Alpha;
            Vivid.Draw.Pen2D.Rect(MX, MY, 24, 24, CursorImg, new OpenTK.Vector4(1, 1, 1, 1));

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            Vivid.Draw.Pen2D.Unbindv();
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
        }

        public void CompleteDrag()
        {
            if (DragObj == null) return;

            UpdateList.Clear();
            foreach (var f in Overlay)
            {
                UpdateUpdateList(f);
            }

            UpdateUpdateList(Root);

            var over = GetTopForm(MX, MY);

            if (over == null) return;

            over.DraggedObj?.Invoke(DragObj);

            Console.WriteLine("Dragged to:" + over.Text + " Type:" + over.GetType().Name);
            if (over.DraggedObj != null)
            {
                Console.WriteLine("Had event");
            }
            else
            {
                Console.WriteLine("Did not have event.");
            }
        }

        public static float TarAlpha = 1.0f;

        public void ResizeAll(float w, float h)
        {



        }

        public void Update2()
        {
            //Per-Event type processing. Custom class for each processing of the active ui. Update/Events etc.

            if (FullScreenOn)
            {
                FullScreen.Root.Set(0, 0, App.AppInfo.W, App.AppInfo.H);
                FullScreen.Update();

                return;
            }

            if (DragObj != null)
            {
                DragObj.X = MX - 5;
                DragObj.Y = MY - 5;
            }

            if (BootAlpha != TarAlpha)
            {
                BootAlpha += (TarAlpha - BootAlpha) * 0.07f;
            }

            if (FirstMouse)
            {
                MX = Input.Input.MX;
                MY = Input.Input.MY;
                FirstMouse = false;
            }

            MXD = Input.Input.MX - MX;
            MYD = Input.Input.MY - MY;
            MX = Input.Input.MX;
            MY = Input.Input.MY;
            UpdateList.Clear();

            foreach (var ev in Events)
            {
                ev.Update?.Invoke();
            }

            UpdateUpdateList(Dock);

            if (ActiveMenu != null)
            {
                //UpdateList.Add(ActiveMenu);
                UpdateUpdateList(ActiveMenu);
            }

            foreach (var f in Overlay)
            {
                if (!UpdateList.Contains(f))
                {
                    UpdateUpdateList(f);
                }
            }

            if (Top != null)
            {
                UpdateUpdateList(Top);
            }
            else
            {
                UpdateUpdateList(Root);
            }


            foreach (UIForm form in UpdateList)
            {
                form.Update?.Invoke();
            }
            UpdateInput();
            // WindSound.Pitch = 1.0f + (-0.2f + WindS * 0.2f);
            //    WindSound.Vol = WindS;

            //   WindS = WindS + (0.0f - WindS) * 0.08f;
        }

        private void UpdateInput()
        {
            UIForm top = GetTopForm(MX, MY);

            if (top != null)
            {
                if (TopForm != top)
                {
                    if (TopForm != null)
                    {
                        if (TopForm != Pressed[0])
                        {
                            TopForm.MouseLeave?.Invoke();
                        }
                    }

                    top.MouseEnter?.Invoke();
                }
            }
            bool am = false;
            if (top != null)
            {
                if (top == TopForm)
                {
                    //    am = true;
                    //     top.MouseMove?.Invoke(MX - top.GX, MY - top.GY, MXD, MYD);
                }
            }
            bool pm = false;
            for (int i = 0; i < 32; i++)
            {
                if (Pressed[i] != null)
                {
                    pm = true;
                    Pressed[i].MouseMove?.Invoke(MX - Pressed[i].GX, MY - Pressed[i].GY, MXD, MYD);
                    Pressed[i].MouseWheelMoved?.Invoke(MZD);
                    break;
                }
            }
            if (TopForm != null && pm == false)
            {
                TopForm.MouseMove?.Invoke(MX - TopForm.GX, MY - TopForm.GY, MXD, MYD);
                TopForm.MouseWheelMoved?.Invoke(MZD);
            }

            if (top == null)
            {
                if (TopForm != null)
                {
                    if (TopForm != Pressed[0])
                    {
                        TopForm.MouseLeave?.Invoke();
                    }
                }
            }
            TopForm = top;

            if (Active != null)
            {
                Active.KeysIn?.Invoke(Input.Input.KeysIn());

                OpenTK.Input.Key key = Input.Input.KeyIn();
                if (key != OpenTK.Input.Key.LastKey)
                {
                    if (key == OpenTK.Input.Key.LastKey)
                    {
                        LastKey = OpenTK.Input.Key.LastKey;
                        NextKey = 0;
                    }
                    if (key == LastKey)
                    {
                        bool shift = false;
                        if (Input.Input.KeyIn(OpenTK.Input.Key.ShiftLeft))
                        {
                            shift = true;
                        }
                        if (Input.Input.KeyIn(OpenTK.Input.Key.ShiftRight))
                        {
                            shift = true;
                        }
                        if (Environment.TickCount > NextKey)
                        {
                            Active.KeyPress?.Invoke(key, shift);
                            NextKey = Environment.TickCount + 90;
                        }
                    }
                    else
                    {
                        bool shift = false;
                        if (Input.Input.KeyIn(OpenTK.Input.Key.ShiftLeft))
                        {
                            shift = true;
                        }
                        if (Input.Input.KeyIn(OpenTK.Input.Key.ShiftRight))
                        {
                            shift = true;
                        }
                        LastKey = key;
                        Active.KeyPress?.Invoke(key, shift);
                        NextKey = Environment.TickCount + 250;
                    }
                }
            }

            if (Input.Input.AnyButtons())
            {
                int bn = Input.Input.ButtonNum();
                if (TopForm != ActiveMenu)
                {
                    //ActiveMenu = null;
                }
                if (TopForm != null)
                {
                    if (Pressed[bn] == null)
                    {
                        if (bn == 0)
                        {
                            //if (ActiveMenu)
                            if (ActiveMenu != null)
                            {
                                if (TopForm == ActiveMenu.Owner)
                                {
                                    ActiveMenu = null;
                                }
                            }
                        }
                        if (bn == 1)
                        {
                            if (TopForm.ContextMenu != null)
                            {
                                ActiveMenu = TopForm.ContextMenu;
                                ActiveMenu.X = MX - 10;
                                ActiveMenu.Y = MY - 10;
                                ActiveMenu.Owner = TopForm;
                                goto SkipClick;
                            }
                        }

                        if (Environment.TickCount < lastClick + 300)
                        {
                            clicks++;
                            if (clicks == 2)
                            {
                                TopForm.DoubleClick?.Invoke(bn);
                            }
                        }
                        else
                        {
                            clicks = 1;
                            //TopForm.Click?.Invoke(0);
                        }
                        lastClick = Environment.TickCount;
                        TopForm.MouseDown?.Invoke(bn);

                        Pressed[bn] = TopForm;
                        if (Active != TopForm)
                        {
                            if (Active != null)
                            {
                                Active.Deactivate?.Invoke();
                            }
                        }
                        Active = TopForm;
                        TopForm.Activate?.Invoke();
                        UIForm root = TopForm;
                        if (bn == 0)
                        {
                            while (true)
                            {
                                if (root.GetType().IsSubclassOf(typeof(Forms.WindowForm)))
                                {
                                    if (root.Root != null)
                                    {
                                        root.Root.Forms.Remove(root);
                                        root.Root.Forms.Add(root);
                                    }
                                    break;
                                }
                                root = root.Root;
                                if (root == null)
                                {
                                    break;
                                }
                            }
                        }
                        if (sdrag)
                        {
                            sdx = MX;
                            sdy = MY;
                        }
                    }
                    else if (Pressed[bn] == TopForm)
                    {
                        TopForm.MousePressed?.Invoke(bn);
                    }
                    else if (Pressed[bn] != TopForm)
                    {
                        Pressed[bn].MousePressed?.Invoke(bn);
                    }
                }
                else
                {
                    if (Pressed[bn] != null)
                    {
                        Pressed[bn].MousePressed?.Invoke(0);
                        //  Pressed[bn].Click?.Invoke(0);
                    }
                }

                if (Pressed[bn] != null)
                {
                    // Console.WriteLine("MX:" + MX + " MY:" + MY + " SDX:" + sdx + " SDY:" + sdy);
                    int mvx = MX - sdx;
                    int mvy = MY - sdy;
                    if (mvx != 0 || mvy != 0)
                    {
                        Pressed[bn].Drag?.Invoke(mvx, mvy);
                        Pressed[bn].PostDrag?.Invoke(mvx, mvy);
                    }
                    sdx = MX;

                    sdy = MY;
                    //Console.WriteLine(@)

                    //sdx = MX-Pressed[0].GY;

                    //sdy = MY-Pressed[0].GY;
                }
            }
            else
            {
                for (int i = 0; i < 32; i++)
                {
                    //Console.WriteLine("Wop");
                    if (Pressed[i] != null)
                    {
                        if (Pressed[i].InBounds(MX, MY) == false)
                        {
                            Pressed[i].MouseLeave?.Invoke();
                        }

                        Pressed[i].MouseUp?.Invoke(i);
                        if (!(Pressed[i] is Resonance.Forms.ButtonForm))
                        {
                            Pressed[i].Click?.Invoke(i);
                        }
                        Pressed[i] = null;
                        sdrag = true;
                    }
                }
            }
            for (int i = 0; i < 32; i++)
            {
                if (Pressed[i] != null)
                {
                    WindS += (float)(Math.Abs(MXD + MYD)) / 150.0f;
                    if (WindS > 1.5f) WindS = 1.5f;
                }
            }
        SkipClick:

            return;
        }

        public float WindS = 0.0f;

        private void AddNodeBackward(List<UIForm> forms, UIForm form)
        {
            int fc = form.Forms.Count;
            if (fc > 0)
            {
                while (true)
                {
                    fc--;
                    UIForm af = form.Forms[fc];
                    AddNodeBackward(forms, af);
                    if (fc == 0)
                    {
                        break;
                    }
                }
            }
            UIForm root = form;
            while (true)
            {
                root = root.Root;
                if (root == null)
                {
                    break;
                }
                if (root.Open == false)
                {
                    return;
                }
            }
            if (form.Open)
            {
                if (!forms.Contains(form))
                {
                    forms.Add(form);
                }
            }
        }

        private void AddNodeForward(List<UIForm> forms, UIForm form)
        {
            if (form.Open == false) return;
            RenderList.Add(form);
            foreach (UIForm nf in form.Forms)
            {
                AddNodeForward(forms, nf);
            }
        }

        private OpenTK.Input.Key KeyDown = OpenTK.Input.Key.LastKey;

        private UIForm GetTopForm(int mx, int my)
        {
            foreach (UIForm form in UpdateList)
            {
                if (form.CheckBounds == true)
                {
                    if (form.InBounds(mx, my))
                    {
                        // Console.WriteLine("Form:" + form.Text);
                        return form;
                    }
                }
            }
            return null;
        }

        public List<UIForm> UpdateWindowList()
        {
            List<UIForm> list = new List<UIForm>();
            AddWinForward(list, Root);
            return list;
        }

        public void AddWinForward(List<UIForm> list, UIForm root)
        {
            if (root.GetType().IsSubclassOf(typeof(Forms.WindowForm)))
            {
                list.Add(root);
            }
            foreach (var node in root.Forms)
            {
                AddWinForward(list, node);
            }
        }

        private void UpdateRenderList(UIForm begin)
        {
            AddNodeForward(RenderList, begin);
        }

        private void UpdateUpdateList(UIForm begin)
        {
            //  UpdateList.Clear();

            AddNodeBackward(UpdateList, begin);
        }
    }
}