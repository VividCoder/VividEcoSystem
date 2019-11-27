using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using MapEditor.States;
namespace MapEditor
{
    public class MapEditorApp : VividApp
    {

        public MapEditorApp() : base("Map Editor",1370,768,false)
        {

        }

        public static void InitMapEditor()
        {

            var app = new MapEditorApp();
            InitState = new EditMapState();
            app.Run();


        }

    }
}
