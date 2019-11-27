using OpenTK;

namespace Vivid.Resonance.Forms
{
    public class VerticalSplitterForm : UIForm
    {
        public int SplitX = 0;

        public OpenTK.Vector4 LeftRect
        {
            get
            {
                return new Vector4(0, 0, SplitX - 2, H);
            }
        }

        public OpenTK.Vector4 RightRect
        {
            get
            {
                return new Vector4(SplitX + 2, 0, W - (SplitX + 2), H);
            }
        }

        public UIForm LeftForm = null;
        public UIForm RightForm = null;

        public bool DragSplit = false;

        public void SetLeft(UIForm form)
        {
            LeftForm = form;
            LeftDock.Add(form);
            LeftDock.AfterSet.Invoke();
            form.Set(0, 0, SplitX - 4, H);

        }

        public void SetSplit(int x)
        {
            SplitX = x;
            LeftDock.Set(0, 0, SplitX - 4, H);
            RightDock.Set(SplitX + 4, 0, W - (SplitX + 4), H);
            if (LeftForm != null)
            {
                LeftDock.Set(0, 0, SplitX - 8, H);
            }
            if (RightForm != null)
            {
                RightDock.Set(SplitX + 8, 0, W - (SplitX + 8), H);
            }
            if (LeftForm != null)
            {
                LeftForm.AfterSet?.Invoke();
            }
            if (RightForm != null)
            {
                RightForm.AfterSet?.Invoke();
            }
        }

        public void SetRight(UIForm form)
        {
            RightForm = form;
            RightDock.Add(form);
            RightDock.AfterSet.Invoke();
        }

        public DockAreaForm LeftDock, RightDock;
        public int DefS = 0;

        public VerticalSplitterForm()
        {
            LeftDock = new DockAreaForm();
            RightDock = new DockAreaForm();
            Clip = false;
            Add(LeftDock);
            Add(RightDock);
            AfterSet = () =>
            {
                //SplitX = DefS;

                LeftDock.Set(0, 0, SplitX - 4, H);
                RightDock.Set(SplitX + 4, 0, W - (SplitX + 4), H);
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
                    if (UI.MX >= (GX + SplitX - 16))
                    {
                        if (UI.MX <= (GX + SplitX + 16))
                        {
                            if (UI.MY >= (GY))
                            {
                                if (UI.MY <= (GY + H))
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
                    SplitX += dx;
                    LeftDock.Set(0, 0, SplitX - 8, H);
                    RightDock.Set(SplitX + 8, 0, W - (SplitX + 8), H);
                    LeftDock.Resize();
                    RightDock.Resize();
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
                DrawFormSolid(new Vector4(0.3f, 0.3f, 0.3f, 1.0f));
                DrawFormSolid(new Vector4(0.35f, 0.35f, 0.35f, 1.0f), SplitX - 2, 0, 4, H);
            };
        }
    }
}