﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using Vivid.Texture;
using Vivid.Tex;
using Vivid.Scene;
using Vivid.Scene.Node;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.State;
using Vivid.Map;
using System.IO;
//using ScopeNine.State;
namespace Vivid.Game.Platformer
{
    public class GamePlatformHost : GameHost
    {

        public Map.Map CurMap = null;
        public Vivid.Scene.SceneGraph2D Graph = null;
        public List<GraphMarker> GetMarkers(string type,string sub="",int index=0)
        {

            List<GraphMarker> marks = new List<GraphMarker>();
            foreach(var m in Graph.Markers)
            {
                if(m.Type == type)
                {
                    if (sub != "")
                    {
                        if (sub == m.SubType)
                        {
                            if (index == m.Index)
                            {
                                marks.Add(m);
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    marks.Add(m);
                }
            }
            return marks;
        }

        public void SetMap(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            CurMap = new Map.Map();

            r.ReadInt32();

            CurMap.Read(r);

            r.Close();
            fs.Close();

            Graph = CurMap.UpdateGraph(64, 64);
            Graph.X = (AppInfo.W / 2)-32;
            Graph.Y = (AppInfo.H / 2)-32;


        }

        public void AddNode(GraphNode node)
        {

            node.Graph = Graph;
            node.Root = Graph.Root;
            Graph.Root.Nodes.Add(node);


        }

        bool light = false;
        public void RenderMap()
        {
            if (light == false)
            {
                foreach (var l in Graph.Lights)
                {
                    l.CheckShadowSize(AppInfo.W, AppInfo.H);
                    l.RenderShadowBuffer(Graph);
                }
                light = true;
            }
            Graph.Draw(true);

        }


    }
}