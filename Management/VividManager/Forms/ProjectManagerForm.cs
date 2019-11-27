using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.Texture;
using Vivid;
using Vivid.App;

namespace VividManager.Forms
{
    public class ProjectManagerForm : WindowForm
    {

        ButtonForm NewProject = null;
        ButtonForm LoadProject = null;
        ButtonForm DeleteProject = null;

        public ProjectManagerForm()
        {

            AfterSet = () =>
            {

                Title.Text = "Vivid Project Manager";
                if (NewProject == null)
                {

                    NewProject = new ButtonForm().Set(5, 5, 120, 25, "New Project") as ButtonForm;

                    var projGroup = new GroupForm().Set(5, 5, 250, 400) as GroupForm;

                    Body.Add(projGroup);

                    projGroup.Add(NewProject);

                    NewProject.Click = (b) =>
                    {

                        var new_proj = new NewProjectForm().Set(AppInfo.W / 2 - 200, AppInfo.H / 2 - 150, 400, 300, "New Project") as NewProjectForm;

                        UI.CurUI.Top = new_proj;                      


                    };



                }

            };

        }

    }
}
