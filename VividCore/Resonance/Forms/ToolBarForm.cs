using System.Collections.Generic;

namespace Vivid.Resonance.Forms
{
    public class ToolBarForm : UIForm
    {
        public List<ToolItem> Items = new List<ToolItem>();
        public ToolItem OverItem = null;
        private bool clicked = false;

        public ToolBarForm()
        {
            Clip = false;
            MouseLeave = () =>
            {
                OverItem = null;
            };
            MouseUp = (b) =>
            {
                clicked = false;
            };
            MouseDown = (b) =>
            {
                if (OverItem != null && clicked == false)
                {
                    clicked = true;
                    OverItem.Click?.Invoke();
                }
            };

            Resized = () =>
            {

                if (Root != null)
                {
                    W = App.AppInfo.W;

                }

            };

            MouseMove = (x, y, dx, dy) =>
            {
                int dx1 = 3;

                foreach (var item in Items)
                {
                    int iw = Font.Width(item.Text);

                    iw += 8;

                    if (x >= dx1 && y >= 1 && x < dx1 + iw && y <= H - 2)
                    {
                        OverItem = item;
                        break;
                    }
                    //   DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.8f), dx, 1, iw, H - 2);
                    //  DrawFormSolid(new OpenTK.Vector4(0.5f, 0.5f, 0.5f, 0.8f), dx + 2, 3, iw - 4, H - 6);

                    // DrawText(item.Text, dx + 4, 2, new OpenTK.Vector4(0.9f, 0.9f, 0.9f, 0.8f));

                    dx1 = dx1 + iw;
                }
            };

            Draw = () =>
            {
                DrawFormSolid(new OpenTK.Vector4(0.25f, 0.25f, 0.25f, 0.8f));

                int dx = 3;

                foreach (var item in Items)
                {
                    int iw = Font.Width(item.Text);

                    iw += 8;

                    DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.8f), dx, 1, iw, H - 2);
                    DrawFormSolid(new OpenTK.Vector4(0.3f, 0.3f, 0.3f, 0.8f), dx + 2, 3, iw - 4, H - 6);

                    if (OverItem == item)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.5f, 0.5f, 0.5f, 0.8f), dx + 2, 3, iw - 4, H - 6);
                    }

                    DrawText(item.Text, dx + 4, 2, new OpenTK.Vector4(0.9f, 0.9f, 0.9f, 0.8f));

                    dx = dx + iw;
                }
            };
        }

        public ToolItem AddItem(string text)
        {
            var item = new ToolItem();
            item.Text = text;
            Items.Add(item);
            return item;
        }
    }

    public class ToolItem
    {
        public string Text = "";
        public ToolClick Click = null;
    }

    public delegate void ToolClick();
}