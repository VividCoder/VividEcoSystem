using System.Collections.Generic;

namespace Vivid.Resonance
{
    public delegate void IconClicked();

    public class DockIconForm : UIForm
    {
        public float GlowT = 0.55f;
        public float Glow = 0.55f;
        public IconClicked Click = null;

        public DockIconForm()
        {
            Draw = () =>
            {
                DrawForm(CoreTex, new OpenTK.Vector4(Glow, Glow, Glow, 0.8f));
            };

            MouseDown = (b) =>
            {
                GlowT = 1.0f;
                Click?.Invoke();
            };

            MouseUp = (b) =>
            {
                GlowT = 0.8f;
            };

            Update = () =>
            {
                Glow = Glow + (GlowT - Glow) * 0.1f;
            };

            MouseEnter = () =>
             {
                 GlowT = 0.8f;
                 Root.Lock = true;
             };

            MouseLeave = () =>
            {
                GlowT = 0.55f;
                Root.Lock = false;
                UIDock dd = Root as UIDock;
                dd.Shown = false;
            };
        }
    }

    public class UIDock : UIForm
    {
        public int cw = 5;

        public DockIconForm AddIcon(Texture.Texture2D img, int w)
        {
            var icon = new DockIconForm().SetImage(img) as DockIconForm;
            Icons.Add(icon);
            icon.X = cw;
            icon.W = w;

            cw = cw + w + 8;
            icon.H = H - 5;
            icon.Y = 5;
            Add(icon);

            return icon;
        }

        public List<DockIconForm> Icons = new List<DockIconForm>();

        public UIDock()
        {
            AfterSet = () =>
            {
                ShownY = Y + 10;
                Shown = false;
            };
            var wintex = new Texture.Texture2D("data/tex/win1.jpg", Texture.LoadMethod.Single, true);
            Draw = () =>
            {
                //                Y = 20;
                DrawFormSolid(new OpenTK.Vector4(0.7f, 0.7f, 0.7f, 0.7f));

                /*
                int x = 5;
                int y = 5;

                var win_list = UI.CurUI.UpdateWindowList();

                foreach (var node in win_list) {
                    DrawFormSolid(new OpenTK.Vector4(0.5f, 0.5f, 0.5f, 0.7f), x, y, 64, 75);
                    DrawForm(wintex, new OpenTK.Vector4(1, 1, 1, 0.7f), x + 4, y + 4, 56, 67);
                    DrawText(node.Text, x , y + 65, new OpenTK.Vector4(0.1f, 0.1f, 0.1f, 0.9f));
                    if (node.Open)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0, 0.2f, 0.5f, 0.7f), x, y, 64, 15);
                    }
                    x = x + 100;
                }

                */
            };
            DoubleClick = (b) =>
            {

            };
            MouseDown = (b) =>
            {
                int x = 5;
                int y = 5;

                var win_list = UI.CurUI.UpdateWindowList();

                foreach (var node in win_list)
                {
                    if (UI.MX > (GX + x))
                    {
                        if (UI.MY > (GY + y))
                        {
                            if (UI.MX < (GX + x + 64))
                            {
                                if (UI.MY < (GY + y + 75))
                                {
                                    //  UI.CurUI.WinTop.Forms.Remove(node);
                                    //   UI.CurUI.WinTop.Forms.Add(node);
                                }
                            }
                        }
                    }
                    x = x + 100;
                }
            };

            MouseEnter = () =>
            {
                Shown = true;
            };
            MouseLeave = () =>
            {
                Shown = false;
            };

            Update = () =>
            {
                if (Lock)
                {
                    Shown = true;
                }
                if (Shown)
                {
                    if (Y > ShownY)
                    {
                        Y = (int)((float)(Y + (ShownY - Y) * 0.1f));
                    }
                }
                else
                {
                    if (Y < Vivid.App.AppInfo.H - 30)
                    {
                        Y = Y + (int)((float)((Vivid.App.AppInfo.H - 5) - Y) * 0.1f);
                    }
                }
            };

            Reset();
        }

        public void Reset()
        {
            Set(App.AppInfo.W / 2 - 250, App.AppInfo.H - 100, 500, 90, "Dock");
        }

        public bool Shown = false;
        public int ShownY = 0;
    }
}