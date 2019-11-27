using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using Vivid.Resonance;
using Vivid.Resonance.Forms;

namespace VividManager.Forms
{
    public class NewProjectForm : WindowForm
    {

        TextBoxForm ProjectPath;
        TextBoxForm ProjectName;
        DropDownListForm ProjectType;

        public NewProjectForm()
        {

            AfterSet = () =>
            {

                Title.Text = "New Project";

                if (ProjectName == null)
                {
                    var pnlab = new LabelForm().Set(5, 5, 120, 20, "Project Name");
                    var ptlab = new LabelForm().Set(5, 35, 120, 20, "Project Type");
                    var pplab = new LabelForm().Set(5, 95, 120, 20, "Project Path");

                    Body.Add(pnlab);
                    Body.Add(ptlab);
                    Body.Add(pplab);

                    ProjectName = new TextBoxForm().Set(115, 10, 220, 20) as TextBoxForm;
                    ProjectType = new DropDownListForm().Set(115, 35, 220, 20) as DropDownListForm;
                    ProjectPath = new TextBoxForm().Set(115, 100, 220, 20) as TextBoxForm;

                    ProjectType.AddItem("2D Map Game");
                    ProjectType.AddItem("3D Game");

                    var BrowsePath = new ButtonForm().Set(280, 125, 80, 25, "Browse") as ButtonForm;

                    Body.Add(ProjectName);
                    Body.Add(ProjectType);
                    Body.Add(ProjectPath);
                    Body.Add(BrowsePath);

                }
            };

        }
            
    }

}
