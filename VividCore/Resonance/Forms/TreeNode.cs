using System.Collections.Generic;

namespace Vivid.Resonance.Forms
{
    public class TreeNode
    {
        public string Name = "";
        public object Obj = null;
        public dynamic DyObj = null;
        public List<TreeNode> Nodes = new List<TreeNode>();
        public bool Open = true;
        public TreeNode Root = null;
        public Click Click = null;
        public string[] TextData = new string[255];
        public TreeNode(string name)
        {
            Name = name;
        }
    }
}