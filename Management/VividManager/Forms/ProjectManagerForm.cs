﻿using System;
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
        TreeViewForm ProjectTree = null;

        public ProjectManagerForm()
        {

            AfterSet = () =>
            {

                Title.Text = "Vivid Project Manager";
                if (NewProject == null)
                {

                    LoadProject = new ButtonForm().Set(5, 35, 120, 25, "Edit Project") as ButtonForm;

                    NewProject = new ButtonForm().Set(5, 5, 120, 25, "New Project") as ButtonForm;

                    DeleteProject = new ButtonForm().Set(5, 65, 120, 25, "Delete Project") as ButtonForm;

                    var projGroup = new GroupForm().Set(5, 5, 250, 400) as GroupForm;

                    Body.Add(projGroup);

                    var projInfoGroup = new GroupForm().Set(260, 5, 405, 500) as GroupForm;

                    Body.Add(projInfoGroup);

                    projGroup.Add(NewProject);
                    projGroup.Add(LoadProject);
                    projGroup.Add(DeleteProject);


                    NewProject.Click = (b) =>
                    {

                        var new_proj = new NewProjectForm().Set(AppInfo.W / 2 - 200, AppInfo.H / 2 - 150, 400, 300, "New Project") as NewProjectForm;

                        UI.CurUI.Top = new_proj;


                    };

                    ProjectTree = new TreeViewForm().Set(AppInfo.W - 350, 10, 320, 450, "") as TreeViewForm;

                    ProjectTree.Root = new TreeNode("Projets");

                    Body.Add(ProjectTree);


                }

            };

        }

        public void Reset()
        {

            ProjectTree.Root = new TreeNode("Projects");

        }

        public void AddProject(Vivid.Project.VividProject proj)
        {

            ProjectTree.Root.Nodes.Add(new TreeNode(proj.Name + " Type:" + proj.Type.ToString()+" Path:"+proj.Path));

        }

        

    }
}
