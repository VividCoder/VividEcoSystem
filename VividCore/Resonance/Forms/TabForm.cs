using System.Collections.Generic;


namespace Vivid.Resonance.Forms
{
    public class TabForm : UIForm
    {

        public List<TabPageForm> Pages = new List<TabPageForm>();
        TabPageForm Active;
        TabPageForm Over;
        public TabPageForm Shown = null;
        public TabForm()
        {
            MouseDown = (b) =>
            {
                if (Over != null)
                {
                    if (Shown != null && Shown != Over)
                    {
                        this.Forms.Remove(Shown);
                    }
                    Active = Over;
                    Shown = Active;
                    this.Forms.Add(Shown);
                }
            };
            MouseMove = (x, y, xd, yd) =>
            {

                int cx = 5;

                Over = null;

                foreach (var p in Pages)
                {

                    if (x >= cx && x <= (cx + 80))
                    {
                        if (y > 0 && y <= 20)
                        {
                            Over = p;
                            break;
                        }
                    }

                    cx = cx + 85;
                }

            };

            Draw = () =>
            {

                int x = 5;

                foreach (var p in Pages)
                {

                    if (Active == p)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.7f, 0.9f, 0.9f, 1.0f), x, 0, 80, 20);

                    }
                    else if (Over == p)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.7f, 0.7f, 0.7f, 1.0f), x, 0, 80, 20);

                    }
                    else
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.4f, 0.4f, 0.4f, 1.0f), x, 0, 80, 20);
                    }
                    DrawText(p.PageName, x + 5, 0);

                    x = x + 85;
                }


            };

        }

        public void AddPage(TabPageForm page)
        {

            Pages.Add(page);
            Active = page;
            Forms.Clear();
            Add(Active);
            page.Y = page.Y + 20;
            Shown = Active;
            //Forms.Add(Active);

        }

    }
    public class TabPageForm : UIForm
    {
        public string PageName = "Tab1";

        public dynamic[] Objs = new dynamic[255];

        public TabPageForm(string name)
        {

            PageName = name;


        }

    }

}
