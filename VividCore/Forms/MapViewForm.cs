﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Resonance.Forms;
using Vivid.Resonance;
using OpenTK;
using Vivid;
using Vivid.App;
using Vivid.Scene;
using Vivid.Tex;
namespace Vivid.Forms
{

    

    public class MapViewForm : UIForm
    {

        public Map.Map Map
        {
            get;
            set;
        }

        public Vivid.Scene.SceneGraph2D Graph
        {
            get;
            set;
        }

        public Vivid.Scene.GraphNode ActiveNode = null;

        //public List<HighLightTile> Highlights = new List<HighLightTile>();

        public Vivid.FrameBuffer.FrameBufferColor MapFrame;


  //      public void HighlightTile(int x,int y,int z)
//        {

        public void UpdateGraphHL()
        {

            //Map.AddHL();
        }
        float gr, gz;
        public void UpdateGraph()
        {
            float gx, gy;

            //Console.WriteLine("Updating graph.");
            if (Graph != null)
            {
                gr = Graph.Rot;
                gx = Graph.X;
                gy = Graph.Y;
                gz = Graph.Z;

                //var sb  SceneGraph.ShadowBuf;
                if (Map.Markers.Count > 0)
                {

                    int vv = 5;
                }
                Graph = Map.UpdateGraph(Map.Layers[0].Width,Map.Layers[0].Height);
                Graph.X = gx;
                Graph.Y = gy;
                Graph.Rot = gr;
                Graph.Z = gz;
               // if (Graph.ShadowBuf == null)
                {
                  //  Graph.ShadowBuf = sb;
                    //Graph.CreateShadowBuf(W, H);
                }
                // Graph.X = -32 + W / 2;
                // Graph.Y = -32 + H / 2;
            }
            else
            {
                Graph = Map.UpdateGraph(Map.Layers[0].Width,Map.Layers[0].Height);
                //if (Graph.ShadowBuf == null)
                {
                //    Graph.CreateShadowBuf(W, H);
                }
                //Graph.X = -32 + W / 2;
                //Graph.Y = -32 + H / 2;
            }
            Changed = true;
  
            //Graph.X -= 370;
            //Graph.Y -= 170;
        }
        public bool Changed = true;
        bool shadows = false;
        GraphSprite LightSprite;
        public GraphSprite ActiveNodeSprite;
        GraphSprite[] ActiveNodeArrow = new GraphSprite[4];
        GraphSprite ActiveNodePivot = null;
        GraphSprite nodeUp, nodeLeft, nodeDown, nodeRight;
        GraphSprite nodeMarker;

        public GraphNode PickObj(int x,int y)
        {

            foreach(var m in Graph.Markers)
            {
                m.Graph = Graph;
                var rm = Graph.PickNode(m, x, y);
                if(rm == m)
                {
                    return m;
                }
            }
            foreach(var l in Graph.Lights)
            {
                float oz = l.Z;
                l.Z = 1.0f;
                var n = Graph.PickNode(l, x, y);
                l.Z = oz;
               if(n == l)

                {
                    return n;
                }
                else
                {
                    int vv = 5;
                }
            }
            return Graph.PickNode(Graph.Root, x, y);

            return null;

        }

        public void SetMap(Map.Map map)
        {
            Map = map;


        }

        public bool LockCam = false;
        public int LockX, LockY;
        public void Center()
        {
            if (Map.Layers.Count > 0)
            {
                UpdateGraph();
                if (MapFrame == null) return;
                Graph.X = (MapFrame.IW / 2) - 32;
                Graph.Y = (MapFrame.IH / 2) - 32;
            }
        }
        public MapViewForm(Map.Map map, bool Shadows = true)
        {
            shadows = Shadows;
            Map = map;

            // ContextMenu = new ContextMenuForm();

            //var add_menu = ContextMenu.AddItem("Add");
            // add_menu.Menu.AddItem("Point Light");

            LightSprite = new GraphSprite(new Tex2D("content/edit/light.png", true), null,64, 64);
            ActiveNodeSprite = new GraphSprite(new Tex2D("content/edit/activenode.png", true),null, 64, 64);
            ActiveNodePivot = new GraphSprite(new Tex2D("content/edit/nodepivot.png", true),null, 16, 16);
            nodeLeft = new GraphSprite(new Tex2D("content/edit/left.png", true),null, 32, 32);
            nodeUp = new GraphSprite(new Tex2D("content/edit/up.png", true),null, 32, 32);
            nodeRight = new GraphSprite(new Tex2D("content/edit/right.png", true),null, 32, 32);
            nodeDown = new GraphSprite(new Tex2D("content/edit/down.png", true),null, 32, 32);
            nodeMarker = new GraphSprite(new Tex2D("content/edit/marker.png", true),null, 64, 64);
            if (MapFrame == null)
            {

                //MapFrame = new Vivid.FrameBuffer.FrameBufferColor()


            }

            AfterSet = () =>
            {

                if (W < 5 || H < 5) return;
                MapFrame = new Vivid.FrameBuffer.FrameBufferColor (W, H);
                Center();
                Changed = true;
            };

            PreDraw = () =>
            {

                if (Map.sceneChanged)
                {
                //    Graph = map.UpdateGraph()
                }

                if (Graph != null)
                {
                    if (shadows)
                    {

                        foreach (var l in Graph.Lights)
                        {
                            l.CheckShadowSize(MapFrame.IW, MapFrame.IH);
                            l.RenderShadowBuffer(Graph);
                        }
                    }

                    // Graph.GenShadow();

                    //shadows = false;

                    //return;
                    MapFrame.Bind();
                    Changed = false;
                    //Console.WriteLine("Rendering map");
                    // AppInfo.RW = AppInfo.RW;
                    //AppInfo.RH = AppInfo.RH;
                    if (LockCam)
                    {
                        Graph.X = LockX;
                        Graph.Y = LockY;
                    }
                     
                    Graph.Draw(shadows);

                    if (Graph.Root.Nodes[0].DrawP != null)
                    {
                        Console.WriteLine("X:" + Graph.Root.Nodes[0].DrawP[0].X + " Y:" + Graph.Root.Nodes[0].DrawP[0].Y);
                    }
                    foreach(var l in Graph.Lights)
                    {

                        LightSprite.X = l.X;
                        LightSprite.Y = l.Y;
                        LightSprite.W = 64;
                        LightSprite.H = 64;
                        LightSprite.Graph = Graph;
                        Graph.DrawSingleNode(LightSprite);

                    }

                    foreach(var m in Graph.Markers)
                    {

                        nodeMarker.X = m.X;
                        nodeMarker.Y = m.Y;
                        nodeMarker.W = 64;
                        nodeMarker.H = 64;
                        nodeMarker.Graph = Graph;
                        Graph.DrawSingleNode(nodeMarker);
                        //Environment.Exit(1);

                    }
               //     Environment.Exit(1);
                    //foreach()



                    if (ActiveNode != null)
                    {
                        SetActiveSprite();
                        Graph.DrawSingleNode(ActiveNodeSprite);

                        ActiveNodePivot.X = ActiveNode.X + 32;
                        ActiveNodePivot.Y = ActiveNode.Y - 32;
                        ActiveNodePivot.W = 16;
                        ActiveNodePivot.H = 16;
                        ActiveNodePivot.Graph = Graph;
                        Graph.DrawSingleNode(ActiveNodePivot);

                    }

                    //Graph.X += 1;
                    //Graph.Y += 1;
                    //Graph.;

                    //Graph.Z = 0.2f;
                    //Graph.Rot += 1.0f;
                    MapFrame.Release();

                }
              



            };
            float r = 1.0f;
            Draw = () =>
            {

                DrawFormSolid(new Vector4(1, 0.8f, 0.8f, 1.0f));
                Col = new Vector4(1, 1, 1, 1);
                if (shadows)
                {
                    int bv = 2;

                }
                DrawForm(MapFrame.BB,0,0,-1,-1,true);
                if (Graph != null)
                {
                    //if (SceneGraph2D.ShadowBuffer2 != null)
                    //{
                    if (Graph.Lights.Count > 0 && Graph.Lights[0].SB1 != null)
                    {
                     
                        // DrawForm(Graph.Lights[0].SB1.BB, 0,0, 256, 256);
                       // DrawForm(Graph.Lights[0].SB2.BB, 0, 256, 256, 256);
                        //Graph.Rot = r;

                        //    }
                    }
                    r = r + 1;
                    //Changed = true;
                }


            };

            

        }

        public void SetActiveSprite()
        {
            ActiveNodeSprite.X = ActiveNode.X;
            ActiveNodeSprite.Y = ActiveNode.Y;
            ActiveNodeSprite.W = 64;
            ActiveNodeSprite.H = 64;
           
            ActiveNodeSprite.Graph = Graph;
            ActiveNodeSprite.SyncCoords();
        }

        public void Update()
        {



        }
      

    }
}
