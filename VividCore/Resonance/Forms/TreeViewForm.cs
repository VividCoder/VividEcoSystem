using OpenTK;
using OpenTK.Graphics.OpenGL4;

using System;

namespace Vivid.Resonance.Forms
{
    public delegate void NodeSelected(TreeNode node);

    public class TreeViewForm : UIForm
    {
        public TreeNode Selected = null;
        public TreeNode Root = null;
        public NodeSelected Select = null;
        public static Texture.Texture2D BodyImg = null;

        public TreeViewForm()
        {

            if (BodyImg == null)
            {
                BodyImg = new Texture.Texture2D("data/nxUI/bg/winBody4.png", Texture.LoadMethod.Single, true);
            }


            Draw = () =>
            {
                GL.Enable(EnableCap.ScissorTest);
                GL.Scissor(GX, GY, W, H);

                DrawForm(BodyImg);
                int y = 5;
                Over = OverNode(Root, ref y, 5);
                y = 5;
                DrawNode(Root, ref y, 5);
                if (ActNode != null)
                {
                    DrawText(ActNode.Name, UI.MX - GX, UI.MY - GY);
                };

                GL.Disable(EnableCap.ScissorTest);
            };

            DoubleClick = (b) =>
            {
                if (Selected != null && Selected.Click != null)
                {
                    Selected.Click(b);
                }
                if (Selected != null)
                {
                    Console.WriteLine("Double:" + Selected.Name);
                }
            };

            MouseDown = (b) =>
              {
                  Console.WriteLine("TreeClick");
                  int y = 5;
                  ClickNode(Root, ref y, 5);
                  y = 5;
                  ActNode = OverNode(Root, ref y, 5);
                  Selected = ActNode;
                  Select?.Invoke(Selected);
              };
            MouseUp = (b) =>
            {
                if (Over != null && ActNode != null)
                {
                    if (ActNode != Over)
                    {
                        if (ActNode.Root != null && ActNode.Root != Over && Over.Root != ActNode)
                        {
                            ActNode.Root.Nodes.Remove(ActNode);
                            Over.Nodes.Add(ActNode);
                            ActNode.Root = Over;
                        }
                    }
                }
                ActNode = null;
            };
        }

        private TreeNode ActNode = null;
        private TreeNode Over = null;

        public TreeNode OverNode(TreeNode node, ref int y, int x = 5)
        {
            if (node == null) return null;
            //DrawText(node.Name, x + 4, y);
            if (UI.MX >= GX + 20)
            {
                if (UI.MY >= GY + y)
                {
                    if (UI.MX <= (GX + 150))
                    {
                        if (UI.MY <= (GY + y + 20))
                        {
                            return node;
                        }
                    }
                }
            }
            foreach (var snode in node.Nodes)
            {
                y = y + 25;
                var on = OverNode(snode, ref y, x + 15);
                if (on != null) return on;
            }
            return null;
        }

        public void ClickNode(TreeNode node, ref int y, int x = 5)
        {
            if (node == null) return;

            if (node.Open)
            {
                if (UI.MX >= (GX + (x - 5)))
                {
                    if (UI.MY >= (GY + (y)))
                    {
                        if (UI.MX <= (GX + (x + 5)))
                        {
                            if (UI.MY <= (GY + (y + 20)))
                            {
                                node.Open = false;
                            }
                        }
                    }
                }
                //DrawLine(x - 4, y + 10, x + 4, y + 10, new Vector4(0, 0, 0, 1));

                //y = y + 25;

                foreach (var sn in node.Nodes)
                {
                    y = y + 25;
                    ClickNode(sn, ref y, x + 15);
                }
            }
            else
            {
                if (UI.MX >= (GX + (x - 5)))
                {
                    if (UI.MY >= (GY + (y)))
                    {
                        if (UI.MX <= (GX + (x + 5)))
                        {
                            if (UI.MY <= (GY + (y + 20)))
                            {
                                node.Open = true;
                            }
                        }
                    }
                }
            }
        }

        public void DrawNode(TreeNode node, ref int y, int x = 5)
        {
            if (node == null) return;
            if (node == Over)
            {
                DrawFormSolid(new Vector4(0.4f, 0.4f, 0.4f, 0.6f), x + 4, y, 150, 20);
            }
            if (node == Selected)
            {
                DrawFormSolid(new Vector4(0.4f, 0.4f, 0.4f, 0.6f), x + 4, y, 150, 20);
            }

            DrawText(node.Name, x + 4, y, new Vector4(1, 1, 1, 1));
            if (node.Open)
            {
                DrawLine(x - 4, y + 10, x + 4, y + 10, new Vector4(0, 0, 0, 1));

                // y = y + 25;
                foreach (var sn in node.Nodes)
                {
                    y = y + 25;
                    DrawNode(sn, ref y, x + 15);
                }
            }
            else
            {
                DrawLine(x - 4, y + 10, x + 4, y + 10, new Vector4(0, 0, 0, 1));
                DrawLine(x, y + 5, x, y + 15, new Vector4(0, 0, 0, 1));
            }
        }
    }
}