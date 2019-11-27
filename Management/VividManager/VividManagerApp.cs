using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Animation;
using Vivid;
using Vivid.App;
using Vivid.State;

namespace VividManager
{
    public class VividManagerApp : VividApp
    {

        public static void InitApp()
        {

            InitState = new States.ManagerMain();

            var app = new VividManagerApp();

            app.Run();

        }

        public VividManagerApp() : base("Vivid Manager",1024,600,false)
        {

           // InitState = new States.ManagerMain();

           

        }

    }
}
