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

namespace VividManager.States
{


    public class ManagerMain : VividState
    {

        public ProjectManagerForm ProjectManager;

        public override void InitState()
        {
            //    base.InitState();
            SUI = new UI();

            SUI.Root = new UIForm().Set(0, 0, AppInfo.W, AppInfo.H);

            ProjectManager = new ProjectManagerForm().Set(0, 0, AppInfo.W , AppInfo.H) as ProjectManagerForm;

            SUI.Root.Add(ProjectManager);

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
