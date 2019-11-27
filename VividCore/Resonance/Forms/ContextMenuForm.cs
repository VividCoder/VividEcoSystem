using System.Collections.Generic;

namespace Vivid.Resonance.Forms
{
    public class ContextMenuForm : UIForm
    {
        public List<MenuItem> Items = new List<MenuItem>();
        public int MenuH = 0;
        public MenuItem OverItem = null;
        public UIForm Owner = null;
        public static Vivid.Texture.Texture2D BG;

        public ContextMenuForm()
        {
            W = 120;
            if (BG == null)
            {
                BG = new Texture.Texture2D("data/nxUI/bg/winBody4.png", Texture.LoadMethod.Single, true);
            }

            MouseMove = (x, y, dx, dy) =>
            {
                int h = 2;

                foreach (var sub_item in Items)
                {
                    //DrawText(sub_item.Text, 2, h, new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.9f));
                    if (x > 0 && x < W && y > h && y < (h + 25))
                    {
                        if (OverItem != null && OverItem != sub_item)
                        {
                            if (Forms.Contains(OverItem.Menu))
                            {
                                Forms.Remove(OverItem.Menu);
                            }
                        }
                        OverItem = sub_item;
                    }
                    h = h + 25;
                }
                if (OverItem != null)
                {
                    if (OverItem.Menu.Items.Count > 0)
                    {
                        OverItem.Menu.X = W;
                        OverItem.Menu.Y = 10;
                        OverItem.Menu.W = 120;
                        OverItem.Menu.H = OverItem.Menu.Items.Count * 25;
                        if (!Forms.Contains(OverItem.Menu))
                        {
                            Add(OverItem.Menu);
                        }
                    }
                }
            };
            MouseLeave = () =>
            {
                //  UI.CurUI.ActiveMenu = null;
            };

            Click = (b) =>
            {
                if (b == 0)
                {
                    if (OverItem != null)
                    {
                        OverItem.Click?.Invoke();
                        UI.CurUI.ActiveMenu = null;
                    };
                }
            };

            Draw = () =>
            {
                W = 120;
                H = Items.Count * 25;

                int bw = 0;
                foreach (var sub in Items)
                {
                    int w = UI.Font.Width(sub.Text) + 10;
                    if (w > bw)
                    {
                        bw = w;
                    }
                }
                W = bw;
                if (W < 80)
                {
                    W = 80;
                }
                DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.7f), -3, -3, W + 6, H + 6);
                DrawForm(BG, new OpenTK.Vector4(0.9f, 0.9f, 0.9f, 0.5f));
                int h = 2;

                foreach (var sub_item in Items)
                {
                    if (OverItem == sub_item)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.5f, 0.5f, 0.5f, 0.75f), 0, h, W, 25);
                    }
                    DrawText(sub_item.Text, 2, h, new OpenTK.Vector4(1f, 1f, 1f, 0.95f));
                    h = h + 25;
                }

                //DrawItem(GX, GY);
                //    DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.9f), -1, -1, W + 2, H + 2);
                //   DrawFormSolid(new OpenTK.Vector4(0.9f, 0.9f, 0.9f,0.9f));
            };
        }

        public MenuItem AddItem(string text, MenuClick click = null)
        {
            var new_item = new MenuItem();
            new_item.Text = text;
            new_item.Click = click;
            Items.Add(new_item);
            return new_item;
        }
    }

    public class MenuItem
    {
        public string Text = "";
        public MenuClick Click = null;
        public string Info = "";

        public ContextMenuForm Menu = new ContextMenuForm();
    }

    public delegate void MenuClick();
}