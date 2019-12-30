using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor
{
    class Program
    {
        static void Main(string[] args)
        {

            //Console.WriteLine("Editing Project:" + args[0]);

            string path = "C:\\Projects\\VividEcoSystem\\Games\\ScopeNine\\bin\\x64\\Debug\\Corona\\";
            GameGlobal.ContentPath = path;// args[0];
            GameGlobal.ProjectPath = path;// args[0];
            MapEditorApp.InitMapEditor();

        }
    }
}
