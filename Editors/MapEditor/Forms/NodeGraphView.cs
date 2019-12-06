using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Forms
{
    public class NodeGraphView : Vivid.Resonance.Forms.WindowForm
    {

        public SpaceEngine.Map.Map map = null;

        public void SetMap(SpaceEngine.Map.Map m)
        {

            map = m;
            Rebuild();

        }

        public void Rebuild()
        {
            tree.Root = new Vivid.Resonance.Forms.TreeNode("Map Root");

            var menu = tree.ContextMenu = new Vivid.Resonance.Forms.ContextMenuForm();


            var m_nodes=menu.AddItem("Nodes");

            var m_logic = m_nodes.Menu.AddItem("Logic");

            var l_marker = m_logic.Menu.AddItem("Marker");

            var n_lights=m_nodes.Menu.AddItem("Lights");

            var l_pointL = n_lights.Menu.AddItem("Point Light");

            l_marker.Click = () =>
            {

                var gm = new Vivid.Scene.GraphMarker();
                gm.SetPos(300, 300);
                GameGlobal.EditMap.AddMarker(gm);
                
                

            };

            l_pointL.Click = () =>
            {

                var nl = new Vivid.Scene.GraphLight();

                nl.SetPos(250, 250);
                nl.Range = 800;
                nl.Z = 0.1f;
                map.AddLight(nl);
                Console.WriteLine("Added light to scene.");

                

            };


             

        }
        Vivid.Resonance.Forms.TreeViewForm tree;
        public NodeGraphView()
        {


            tree = new Vivid.Resonance.Forms.TreeViewForm();
            Body.Add(tree);
            AfterSet = () =>
            {

                tree.W = Body.W;
                tree.H = Body.H;
                Title.Text = "Node View";

            };

        }

    }
}
