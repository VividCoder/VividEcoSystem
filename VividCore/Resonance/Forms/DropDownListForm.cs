using System.Collections.Generic;

namespace Vivid.Resonance.Forms
{
    public class DropDownListForm : UIForm
    {

        public string CurrentItem = "";

        public List<string> Items = new List<string>();
        public bool Open = false;
        public SelectItem SelectedItem = null;

        public DropDownListForm()
        {

            Draw = () =>
            {

                DrawFormSolid(new OpenTK.Vector4(1, 1, 1, 1));
                DrawText(CurrentItem, 5, 3);


            };

            MouseDown = (b) =>
            {
                if (Open)
                {
                    Open = false;
                    Forms.Clear();
                }
                else
                {
                    int y = 0;
                    Open = true;
                    foreach (var item in Items)
                    {
                        var ib = new ButtonForm().Set(0, H + y, W, 25, item) as ButtonForm;
                        y = y + 25;
                        Add(ib);
                        ib.Click = (bt) =>
                        {
                            CurrentItem = item;
                            Open = false;
                            Forms.Clear();
                            SelectedItem?.Invoke(item);
                        };
                    }
                }
            };

        }

        public void AddItem(string item)
        {
            if (Items.Count == 0) CurrentItem = item;
            Items.Add(item);
        }

    }
    public delegate void SelectItem(string item);
}
