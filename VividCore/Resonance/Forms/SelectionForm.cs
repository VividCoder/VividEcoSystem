using System.Collections.Generic;
using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public class SelectionForm : UIForm
    {
        public List<SelectionItem> Items = new List<SelectionItem>();
        public float Begin = 10;
        public SelectionItem ActiveItem = null;

        public SelectionForm()
        {
            void DrawFunc()
            {
                DrawFormBlur(UI.WhiteTex, 0.2f, new OpenTK.Vector4(0.7f, 0.7f, 0.7f, 1));

                float draw_y = Begin;

                foreach (var item in Items)
                {
                    if (ActiveItem == item)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0, 0.8f, 0.8f, 1.0f), 0, (int)draw_y, W, 24);
                    }

                    if (item.Image != null)
                    {
                        DrawForm(item.Image, 2, (int)draw_y, 16, 24);
                        DrawText(item.Name, 20, (int)draw_y, new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.8f));
                    }
                    else
                    {
                        DrawText(item.Name, 2, (int)draw_y, new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.8f));
                    }

                    draw_y += 26;
                }
            }

            void MouseDownFunc(int b)
            {
                if (b == 0)
                {
                    if (ActiveItem != null)
                    {
                        Selected?.Invoke(ActiveItem);
                        ActiveItem.Selected?.Invoke(ActiveItem);
                    }
                }
            }

            MouseDown = MouseDownFunc;

            void MouseMoveFunc(int x, int y, int mx, int my)
            {
                float draw_y = Begin;

                foreach (var item in Items)
                {
                    if (item.Image != null)
                    {
                        //DrawForm(item.Image, 2, (int)draw_y, 16, 24);
                        // DrawText(item.Name, 20, (int)draw_y);

                        if (x > 0 && x <= W && y >= draw_y && y <= draw_y + 24)
                        {
                            ActiveItem = item;
                        }
                    }
                    else
                    {
                        //   DrawText(item.Name, 2, (int)draw_y);
                    }

                    draw_y += 26;
                }
            }

            MouseMove = MouseMoveFunc;

            Draw = DrawFunc;
        }

        public SelectionItem AddItem(string text, Texture2D image = null, dynamic obj = null)
        {
            var new_item = new SelectionItem(text, image, obj);

            Items.Add(new_item);

            return new_item;
        }

        public SelectedItem Selected = null;
    }

    public delegate void SelectedItem(SelectionItem item);

    public class SelectionItem
    {
        public string Name = "";
        public Texture2D Image = null;
        public dynamic Obj = null;

        public SelectionItem(string text, Texture2D image, dynamic obj)
        {
            Name = text;
            Image = image;
            Obj = obj;
        }

        public SelectedItem Selected = null;
    }
}