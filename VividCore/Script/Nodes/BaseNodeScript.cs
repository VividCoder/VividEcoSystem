using Vivid.Scene;

namespace Vivid.Script.Nodes
{
    public class BaseNodeScript
    {
        public Node3D Node
        {
            get;
            set;
        }

        public Entity3D Entity
        {
            get;
            set;
        }

        public virtual void Update()
        {
        }
    }
}