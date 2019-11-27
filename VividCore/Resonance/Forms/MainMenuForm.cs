using System.Collections.Generic;

namespace Vivid.Resonance.Forms
{
    public class MenuForm : UIForm
    {
        public MenuForm RootMenu = null;
        public MenuForm OpenMenu = null;
        public bool SideWays = true;
        public List<MenuItem2> Items = new List<MenuItem2>();
        public static Texture.Texture2D BG = null;
        public MenuItem2 ActiveItem = null;

        public MenuForm()
        {
            //UI.CurUI.Overlay.Add(this);

            if (BG == null)
            {
                BG = new Texture.Texture2D("data/nxUI/bg/winBody4.png", Texture.LoadMethod.Single, true);
            }

            MouseLeave = () =>
             {
                 ActiveItem = null;
                 if (this.Root is Forms.MenuForm)
                 {
                     if (OpenMenu == null || OpenMenu.Items.Count == 0)
                     {
                         this.Open = false;
                     }
                 }
             };

            MouseDown = (b) =>
            {
                if (b == 0)
                {
                    if (ActiveItem != null)
                    {
                        ActiveItem.Click?.Invoke(0);
                    }
                }
            };

            MouseMove = (mX, mY, dX, dY) =>
            {
                if (UI.CurUI.ActiveMenu2 == null)
                {
                    UI.CurUI.ActiveMenu2 = this;
                }
                if (true)
                {
                    if (OpenMenu != null)
                    {
                        Forms.Remove(OpenMenu);
                        OpenMenu = null;
                    }
                    if (SideWays)
                    {
                        int mx = 5;

                        foreach (var item in Items)
                        {
                            int nx = mx + UI.Font.Width(item.Name) + 5;

                            if (mX > mx && mX < nx && mY > 0 && mY < H)
                            {
                                ActiveItem = item;
                                if (OpenMenu != null)
                                {
                                    Forms.Remove(OpenMenu);
                                }
                                //UI.UIChanged = true;

                                OpenMenu = item.Menu;
                                OpenMenu.SideWays = false;
                                OpenMenu.RootMenu = this;
                                OpenMenu.Open = true;
                                int bw = 5;
                                int my = 5;

                                foreach (var ci in OpenMenu.Items)
                                {
                                    int w = UI.Font.Width(ci.Name);
                                    if (w > bw)
                                    {
                                        bw = w;
                                    }

                                    my = my + 25;
                                }

                                OpenMenu.W = bw + 10;
                                OpenMenu.H = my;
                                OpenMenu.X = nx - 35;
                                OpenMenu.Y = 15;

                                if (OpenMenu.Items.Count > 0)
                                {
                                    Add(OpenMenu);
                                }
                            }

                            mx = nx + 5;
                        }
                    }
                    else
                    {
                        int my = 5;
                        int hi = 0;
                        foreach (var item in Items)
                        {
                            //DrawText(item.Name, 5, my, new OpenTK.Vector4(1, 1, 1, 1));
                            hi++;
                            if ((mX > 0 && mX < W && mY > my && mY < my + 25))
                            {
                                ActiveItem = item;

                                ActiveItem = item;
                                if (OpenMenu != null)
                                {
                                    Forms.Remove(OpenMenu);
                                }
                                //UI.UIChanged = true;

                                OpenMenu = item.Menu;
                                OpenMenu.SideWays = false;
                                OpenMenu.RootMenu = this;
                                OpenMenu.Open = true;
                                int bw = 5;
                                int my2 = 5;

                                foreach (var ci in OpenMenu.Items)
                                {
                                    int w = UI.Font.Width(ci.Name);
                                    if (w > bw)
                                    {
                                        bw = w;
                                    }

                                    my2 = my2 + 25;
                                }

                                OpenMenu.W = bw + 10;
                                OpenMenu.H = my2;
                                OpenMenu.X = 25;
                                OpenMenu.Y = 5 + (20 * (hi - 1));
                                if (OpenMenu.Items.Count > 0)
                                {
                                    Add(OpenMenu);
                                }
                            }

                            my = my + 25;
                        }
                    }
                }
            };

            Draw = () =>
            {
                // DrawForm(BG);
                DrawForm(BG);
                int mx = 5;

                if (SideWays)
                {
                    foreach (var item in Items)

                    {
                        if (item == ActiveItem)
                        {
                            DrawFormSolid(new OpenTK.Vector4(0.5f, 0.5f, 0.5f, 0.8f), mx, 0, UI.Font.Width(item.Name), H);
                        }
                        DrawText(item.Name, mx, 1, new OpenTK.Vector4(1f, 1f, 1f, 1f));

                        mx = mx + UI.Font.Width(item.Name) + 5;

                        DrawFormSolid(new OpenTK.Vector4(1f, 1f, 1f, 1f), mx, 0, 1, H);

                        mx = mx + 5;
                    }
                }
                else
                {
                    int my = 5;

                    foreach (var item in Items)
                    {
                        if (item == ActiveItem)
                        {
                            DrawFormSolid(new OpenTK.Vector4(0.5f, 0.5f, 0.5f, 0.8f), 5, my, W - 5, 25);
                        }
                        DrawText(item.Name, 5, my, new OpenTK.Vector4(1, 1, 1, 1));

                        my = my + 25;
                    }
                }
            };
        }

        public MenuItem2 AddItem(string name, Click click = null)
        {
            var ni = new MenuItem2();
            ni.Name = name;
            ni.Click = click;
            ni.Menu = new MenuForm();
            Items.Add(ni);
            return ni;
        }
    }

    public class MenuItem2
    {
        public string Name = "";
        public Click Click = null;
        public MenuForm Menu = null;
    }
}