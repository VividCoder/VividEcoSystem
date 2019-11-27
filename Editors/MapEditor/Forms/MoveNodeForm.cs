using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.Texture;
namespace MapEditor.Forms
{
    public class MoveNodeForm : UIForm
    {

        public static Texture2D up, down, left, right;
        public Vivid.Scene.GraphNode Node;
        public SpaceEngine.Forms.MapViewForm View;
        public void SetNode(Vivid.Scene.GraphNode n)
        {
            Node = n;
        }
        public MoveNodeForm()

        {
          
            if(up == null)
            {
                up = new Texture2D("content/edit/up.png", LoadMethod.Single, true);
                down = new Texture2D("content/edit/down.png", LoadMethod.Single, true);
                left = new Texture2D("content/edit/left.png", LoadMethod.Single, true);
                right = new Texture2D("content/edit/right.png", LoadMethod.Single, true);
            }

            ImageForm uform = new ImageForm().Set(0, 0, 32, 32).SetImage(up) as ImageForm ;
            ImageForm lform = new ImageForm().Set(0, 0, 32, 32).SetImage(left) as ImageForm;
            ImageForm rform = new ImageForm().Set(0, 0, 32, 32).SetImage(right) as ImageForm;
            ImageForm dform = new ImageForm().Set(0, 0, 32, 32).SetImage(down) as ImageForm;

            Add(uform);
            Add(lform);
            Add(rform);
            Add(dform);
            bool mup = false;
            bool mleft = false;
            bool mright = false;
            bool mdown = false;


            rform.MouseDown = (b) =>
            {
                mright = true;
            };
            rform.MouseUp = (b) =>
            {
                mright = false;
            };

            rform.MouseMove = (x, y, xd, yd) =>
            {
                if (Node != null && mright)
                {
                    Node.X = Node.X + xd;
                    View.UpdateGraph();
                }
            };


            dform.MouseDown = (b) =>
            {
                mdown = true;
            };
            dform.MouseUp = (b) =>
            {
                mdown = false;
            };

            dform.MouseMove = (x, y, xd, yd) =>
            {
                if (Node != null && mdown)
                {
                    Node.Y = Node.Y + yd;
                    View.UpdateGraph();
                }
            };


            lform.MouseDown = (b) =>
            {
                mleft = true;
            };
            lform.MouseUp = (b) =>
            {
                mleft = false;
            };

            lform.MouseMove = (x, y, xd, yd) =>
            {
                if (Node != null && mleft)
                {
                    Node.X = Node.X + xd;
                    View.UpdateGraph();
                }
            };

            uform.MouseDown = (b) =>
            {
                mup = true;
            };
            uform.MouseUp = (b) =>
            {
                mup = false;
            };

            uform.MouseMove = (x, y, xd, yd) =>
            {
                if (Node != null && mup)
                {
                    Node.Y = Node.Y + yd;
                    View.UpdateGraph();
                }
            };

            AfterSet = () =>
            {

                uform.Set(32, 0, 32, 32);
                lform.Set(0, 32, 32, 32);
                rform.Set(64, 32, 32, 32);
                dform.Set(32, 64, 32, 32);

            };

        }

    }
}
