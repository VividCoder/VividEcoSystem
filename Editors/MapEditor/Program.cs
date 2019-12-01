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

            Console.WriteLine("Editing Project:" + args[0]);
            GameGlobal.ContentPath = args[0];
            GameGlobal.ProjectPath = args[0];
            MapEditorApp.InitMapEditor();

        }
    }
}
