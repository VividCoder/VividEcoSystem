using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Project
{
    public enum ProjectType
    {
        GameMap,Game3D,Game2D,App2D,App3D
    }
    public class VividProject
    {

        public string Path
        {
            get;
            set;
        }

        public ProjectType Type
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public VividProject()
        {
            Path = "";
            Type = ProjectType.GameMap;
            Name = "";
        }



    }
}
