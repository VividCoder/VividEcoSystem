using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using Vivid.State;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using VividManager.Forms;
using System.IO;

namespace VividManager.States
{


    public class ManagerMain : VividState
    {

        public static ProjectManagerForm ProjectManager;

        public static List<Vivid.Project.VividProject> Projects = new List<Vivid.Project.VividProject>();

        public static void AddProject(Vivid.Project.VividProject proj)
        {

            Projects.Add(proj);
            ProjectManager.AddProject(proj);
            SaveProjectList();

        }

        public static void ReadProjectList()
        {

            Projects.Clear();
            
            FileStream fs = new FileStream("projs.list", FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            int pc = r.ReadInt32();

            for(int i = 0; i < pc; i++)
            {

                string path = r.ReadString();
                string name = r.ReadString();
                int type = r.ReadInt32();

                var proj = new Vivid.Project.VividProject();

                proj.Path = path;
                proj.Name = name;
                proj.Type = (Vivid.Project.ProjectType)type;

                Projects.Add(proj);

            }

            r.Close();
            fs.Close();
            ProjectManager.Reset();

            foreach (var proj in Projects)
            {

                ProjectManager.AddProject(proj);

            }

          
        }

        public static void SaveProjectList()
        {

            FileStream fs = new FileStream("projs.list", FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);

            w.Write(Projects.Count);

            foreach(var proj in Projects)
            {

                w.Write(proj.Path);
                w.Write(proj.Name);
                w.Write((int)proj.Type);

            }

            w.Flush();
            fs.Flush();
            w.Close();
            fs.Close();

        }

        public override void InitState()
        {
            //    base.InitState();
            SUI = new UI();

            SUI.Root = new UIForm().Set(0, 0, AppInfo.W, AppInfo.H);

            ProjectManager = new ProjectManagerForm().Set(0, 0, AppInfo.W , AppInfo.H) as ProjectManagerForm;

            SUI.Root.Add(ProjectManager);

            ReadProjectList();

        }

        public override void UpdateState()
        {
            //base.UpdateState();
            SUI.Update();
            Vivid.Texture.Texture2D.UpdateLoading();

        }

        public override void DrawState()
        {
            //base.DrawState();
            SUI.Render();

        }

    }
}
