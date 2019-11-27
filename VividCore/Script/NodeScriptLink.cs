using System.IO;
using Vivid.Scene;

namespace Vivid.Script
{
    public class NodeScriptLink
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

        public AnimEntity3D AnimEntity
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public dynamic Link
        {
            get;
            set;
        }

        public void LoadAndCompile()
        {
            Code = File.ReadAllText(FilePath);
            Compile();
        }

        public void Compile()
        {
        }

        public void SetNode(Node3D node)
        {
            Link.Node = node;
            if (node is Entity3D)
            {
                Link.Entity = node as Entity3D;
            }
        }

        public virtual void Begin()
        {
        }

        public virtual void End()
        {
        }

        public virtual void Pause()
        {
        }

        public virtual void Resume()
        {
        }

        public virtual void Update()
        {
            Link.Update();
        }

        public virtual void Draw()
        {
        }
    }
}