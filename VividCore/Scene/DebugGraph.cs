using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Scene
{
    public class DebugGraph
    {
        SceneGraph2D Graph = null;
        public DebugGraph(SceneGraph2D graph)
        {

        }



    }

    public class DebugPoint : GraphSprite
    {
        static Vivid.Tex.Tex2D PointImg;
        public DebugPoint() : base(64,64)
        {
            if(PointImg == null)
            {

            }
            //ImgFrame =
                
        }
    }

}
