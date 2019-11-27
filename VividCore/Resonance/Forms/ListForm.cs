﻿using OpenTK;
using System.Collections.Generic;
using Vivid.Texture;

namespace Vivid.Resonance.Forms
{
    public class ListForm : UIForm
    {
        public ScrollBarV Scroller = null;
        public List<ItemForm> Items = new List<ItemForm>();

        public ListForm()
        {
            PushArea = true;
            Col = new Vector4(0.8f, 0.8f, 0.8f, 0.5f);

            void DrawFunc()
            {
                DrawFormSolid(Col);
            }

            void ChangedFunc()
            {
                Scroller.X = W - 10;
                Scroller.Y = 2;
                Scroller.W = 10;
                Scroller.H = H;

                Scroller.Changed?.Invoke();
                foreach (ItemForm item in Items)
                {
                    Forms.Remove(item);
                }

                float sh = (Scroller.H - Scroller.ScrollBut.H);
                float mh = Items.Count * 20;

                float dh = sh / mh;
                if (dh < 0.03f)
                {
                    //Scroller.Max = Scr
                    //    sm1 = (float)Scroller.H * 0.03f;
                }

                Scroller.ScrollBut.H = (int)(dh * Scroller.H);
                if (dh < 0.1f)
                {
                    //Scroller.ScrollBut.H = 6;
                    //Scroller.Max
                    //Scroller.Max = Scroller.Max - 10;
                }

                Scroller.Max = Scroller.H;
                float ly = Scroller.Cur / Scroller.Max;
                float mh2 = ly * ((Items.Count + 1) * 22);

                if (Scroller.ScrollBut.H > H)
                {
                    Scroller.ScrollBut.H = H;
                }
                //ly = -(ly * H);
                ly = -(mh2);
                //ly = ly - 20;

                foreach (ItemForm item in Items)
                {
                    //var newi = new ItemForm().Set(5, (int)ly, W - 15, 20, item.Text) as ItemForm;
                    ItemForm newi = item;
                    //newi.Pic = item.Pic;
                    if (ly > H - 22 || ly < 0)
                    {
                        newi.Render = false;
                        newi.CheckBounds = false;
                    }
                    else
                    {
                        newi.CheckBounds = true;
                        newi.Render = true;
                    }
                    newi.Y = (int)ly;
                    ly = ly + 22;
                    //  newi.ViewX = 0;
                    //  newi.ViewY = 0;
                    // newi.ViewW = W;
                    // newi.ViewH = H;
                    Add(newi);
                }

                if (Scroller.ScrollBut.H < 5)
                {
                    Scroller.ScrollBut.H = 5;
                }

                //Scroller.ScrollBut.Drag(0, 15);
            }

            Changed = ChangedFunc;

            Draw = DrawFunc;

            Scroller = new ScrollBarV();

            void PostDragFunc(int x, int y)
            {
                //Scroller.Cur = Scroller.ScrollBut.Y / Scroller.H;
                //float my = Scroller.Max / Scroller.H;
                //Scroller.Cur = Scroller.Cur * my;

                Scroller.Cur = Scroller.ScrollBut.Y;
                Changed?.Invoke();
            }

            Scroller.ScrollBut.PostDrag = PostDragFunc;

            Add(Scroller);

            //PostDragFunc(0, 5);
            //Scroller.ScrollBut.Drag(0, -5);
        }

        public void Clear()
        {
            foreach (ItemForm i in Items)
            {
                Forms.Remove(i);
            }
            Items.Clear();
        }

        public ItemForm AddItem(ItemForm item)
        {
            Items.Add(item);
            return item;
            //Changed?.Invoke();
        }

        public ItemForm AddItem(string text, Texture2D pic)
        {
            ItemForm nitem = new ItemForm
            {
                Text = text,
                Pic = pic,
                W = W - 20,
                H = 20
            };
            Items.Add(nitem);
            return nitem;
            //Changed?.Invoke();
        }
    }
}