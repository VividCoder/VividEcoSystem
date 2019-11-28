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
        ButtonForm CreateProject;
        string NewPath = "";

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

                    CreateProject = new ButtonForm().Set(20, 230, 130, 25, "Create Project") as ButtonForm;

                    Body.Add(CreateProject);

                    CreateProject.Click = (b) =>
                    {

                        Vivid.Project.VividProject new_proj = new Vivid.Project.VividProject();
                        new_proj.Path = NewPath;
                        new_proj.Name = ProjectName.Text;
                        switch(ProjectType.CurrentItem)
                        {
                            case "2D Map Game":
                                new_proj.Type = Vivid.Project.ProjectType.GameMap;
                                break;
                            case "3D Game":
                                new_proj.Type = Vivid.Project.ProjectType.Game3D;
                                break;
                        }

                        VividManager.States.ManagerMain.AddProject(new_proj);

                        UI.CurUI.Top = null;

                    };

                    BrowsePath.Click = (b) =>
                    {

                        var req = new RequestFileForm("Select a empty folder for the project..", "C:/", true);
                        UI.CurUI.Top.Add(req);

                        req.Selected = (path) =>
                        {

                            ProjectPath.Text = path;
                            NewPath = path;
                            UI.CurUI.Top.Forms.Remove(req);

                        };

                    };

                    Body.Add(ProjectName);
                    Body.Add(ProjectType);
                    Body.Add(ProjectPath);
                    Body.Add(BrowsePath);

                }
            };

        }
            
    }

}
