using OpenTK;

namespace Vivid.Resonance.Forms
{
    public class HorizontalSplitterForm : UIForm
    {
        public int SplitY = 0;

        public OpenTK.Vector4 TopRect
        {
            get
            {
                return new Vector4(0, 0, W, SplitY - 4);
            }
        }

        public OpenTK.Vector4 BottomRect
        {
            get
            {
                return new Vector4(0, SplitY + 4, W, (SplitY + 4) - H);
            }
        }

        public UIForm TopForm = null;
        public UIForm BottomForm = null;

        public DockAreaForm TopDock, BotDock;

        public bool DragSplit = false;
        public int DefS = 0;

        public void SetTop(UIForm form)
        {
            TopForm = form;
            TopDock.Add(form);
            TopDock.AfterSet.Invoke();
        }

        public void SetBottom(UIForm form)
        {
            BottomForm = form;
            BotDock.Add(form);
            BotDock.AfterSet.Invoke();
            //form.Set(GX , GY + SplitY + 4, W,H-(SplitY-4), form.Text);
            //form.Docked = true;
        }

        public void SetSplit(int y)
        {
            SplitY = y;
            TopDock.Set(0, 0, W, SplitY - 4);
            BotDock.Set(0, SplitY + 4, W, H - (SplitY + 4));
            if (TopForm != null)
            {
                TopDock.Set(0, 0, W, SplitY - 4);
            }
            if (BottomForm != null)
            {
                BotDock.Set(0, SplitY + 4, W, H - (SplitY + 4));
                //    BottomForm.Set(GX, GY + SplitY + 4, W, H - (SplitY - 4), BottomForm.Text);
            }
            if (TopForm != null)
            {
                TopForm.AfterSet?.Invoke();
            }
            if (BottomForm != null)
            {
                BottomForm.AfterSet?.Invoke();
            }
        }

        public HorizontalSplitterForm()
        {
            Clip = false;
            TopDock = new DockAreaForm();
            BotDock = new DockAreaForm();
            Add(TopDock);
            Add(BotDock);
            AfterSet = () =>
            {
                //SplitY = DefS;

                TopDock.Set(0, 0, W, SplitY - 4);
                BotDock.Set(0, SplitY + 4, W, H - (SplitY + 8));
                //  W = Root.W;
                //   H = Root.H;
                // SplitY = H / 2;
            };

            Resized = () =>
            {
                if (Root != null)
                {
                    W = Root.W;
                    H = Root.H;
                }
            };

            MouseDown = (b) =>
            {
                if (b == 0)
                {
                    if (UI.MX >= (GX))
                    {
                        if (UI.MX <= (GX + W))
                        {
                            if (UI.MY >= (GY + SplitY - 12))
                            {
                                if (UI.MY <= (GY + SplitY + 8))
                                {
                                    DragSplit = true;
                                }
                            }
                        }
                    }
                   ;
                }
            };

            MouseMove = (x, y, dx, dy) =>
            {
                if (DragSplit)
                {
                    SplitY += dy;

                    TopDock.Set(0, 0, W, SplitY - 4);
                    BotDock.Set(0, SplitY + 4, W, H - (SplitY + 4));
                }
            };

            MouseUp = (b) =>
            {
                if (b == 0)
                {
                    DragSplit = false;
                }
            };

            Draw = () =>
            {
                //   DrawFormSolid(new Vector4(0.3f, 0.3f, 0.3f, 1.0f));
                DrawFormSolid(new Vector4(0.35f, 0.35f, 0.35f, 1.0f), 0, SplitY - 2, W, 4);
            };
        }
    }
}