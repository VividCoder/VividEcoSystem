using System.Collections.Generic;

namespace Vivid.Scene.Anim
{
    public class AnimGraph
    {

        public SceneGraph3D VisualGraph
        {
            get;
            set;
        }

        public List<AnimNode> Nodes = new List<AnimNode>();

        public double AnimLength = 30.0;

        public string Name = "New Animation";


    }
}
