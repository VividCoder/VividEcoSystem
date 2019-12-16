﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid.Map.Layer;
using Vivid.Scene;
using Vivid.Tex;
namespace Vivid.Map
{
    public class Map
    {
        public class HighLightTile
        {

            public int X
            {
                get;
                set;
            }

            public int Y
            {
                get;
                set;
            }

           

        }
        public List<MapLayer> Layers
        {
            get;
            set;
        }

        public List<GraphLight> Lights
        {
            get;
            set;
        }

        public List<GraphMarker> Markers
        {
            get;
            set;
        }

        public Map()
        {

            Layers = new List<MapLayer>();
            TileWidth = TileHeight = 64;
            Lights = new List<GraphLight>();
            Markers = new List<GraphMarker>();
            sceneChanged = true;
        }
        public void Read(System.IO.BinaryReader r)
        {

            TileWidth = r.ReadInt32();
            TileHeight = r.ReadInt32();
            int lc = r.ReadInt32();
            for(int i = 0; i < lc; i++)
            {
                var l = new Vivid.Scene.GraphLight();
                l.Read(r);
                Lights.Add(l);
            }
            int mc = r.ReadInt32();
            for (int i = 0; i < mc; i++)
            {
                var m = new Vivid.Scene.GraphMarker();
                m.Read(r);
                Markers.Add(m);
            }
            int layc = r.ReadInt32();
            Layers = new List<MapLayer>();
            for(int l = 0; l < layc; l++)
            {

                var ml = new MapLayer(1,1,this);
                ml.Read(r);
                Layers.Add(ml);



            }
            
        }
        public void Write(System.IO.BinaryWriter w)
        {

            w.Write(TileWidth);
            w.Write(TileHeight);
            w.Write(Lights.Count);
            foreach(Vivid.Scene.GraphLight l in Lights)
            {
                l.Write(w);
            }
            w.Write(Markers.Count);
            foreach(Vivid.Scene.GraphMarker m in Markers)
            {
                m.Write(w);
            }
            w.Write(Layers.Count);
            foreach(var lay in Layers)
            {
                lay.Write(w);
            }

        }

        public void AddLight(GraphLight l)
        {

            Lights.Add(l);
            sceneChanged = true;
        }

        public void AddMarker(GraphMarker m)
        {
            Markers.Add(m);
            sceneChanged = true;
        }

        public int TileWidth

        {
            get;
            set;
        }

        public int TileHeight
        {
            get;
            set;
        }

        public MapLayer AddLayer(MapLayer layer)
        {
            Layers.Add(layer);
            return layer;
        }

        public Map(int numLayers)
        {

            Layers = new List<MapLayer>();

            for (int i = 0; i < numLayers; i++)
            {
               // Layers.Add(new MapLayer());
            }

        }

        MapLayer GetLayer(int index)
        {

            return Layers[index];

        }
    
        void SetLayer(MapLayer layer,int index)
        {

            Layers[index] = layer;
            sceneChanged = true;
            
        }

        public List<HighLightTile> HL = new List<HighLightTile>();

        public void HighlightTile(int x,int y)
        {

            HL.Add(new HighLightTile() { X = x, Y = y });
            sceneChanged = true;

        }

        public bool sceneChanged = false;
        SceneGraph2D oGraph = null;
        public Vivid.Scene.SceneGraph2D UpdateGraph(int tw,int th)
        {
          //  if (sceneChanged == false) return oGraph;
            Vivid.Scene.SceneGraph2D Graph = oGraph;
            if (oGraph == null)
            {

               Graph = new Vivid.Scene.SceneGraph2D();
            }
            oGraph = Graph;
            sceneChanged = false;
            float sp = 0;
            int li = 0;
            Graph.Root = new GraphNode();
            foreach (var layer in Layers)
            {
                li++;
                for (int y = 0; y < layer.Height; y++)
                {
                    for (int x = 0; x < layer.Width; x++)
                    {

                        var tile = layer.GetTile(x, y);

                        if (tile == null)
                        {
                            if (li == 1)
                            {

                                var notile = new GraphSprite(new Tex2D("content/edit/notile.png", false),null, TileWidth, TileHeight);
                                notile.SetPos(x * TileWidth, y * TileHeight);
                                notile.TileX = x;
                                notile.TileY = y;
                                Graph.Add(notile);
                            }
                            continue;
                        }

                        var tileSpr = new GraphSprite(tile.ColorImage,tile.NormalImage, TileWidth, TileHeight);

                        tileSpr.TileX = x;
                        tileSpr.TileY = y;

                        int mx = x * TileWidth;
                        int my = y * TileHeight;



                        tileSpr.SetPos(mx, my);
                        tileSpr.Obj[0] = tile;
                        if (Lights.Count > 0)
                        {
                            tileSpr.ShadowPlane = sp;
                        }
                        Graph.Add(tileSpr);
                        if (li > 1)
                        {
                            tileSpr.CastShadow = true;
                            tileSpr.RecvShadow = false;
                        }

                    }
                }
                sp = sp + 0.1f;
            }

            NewMethod(Graph);

            if(Graph.Lights.Count != Lights.Count)
            {
                Graph.Lights.Clear();
                Graph.Add(Lights.ToArray());
            }

            Graph.Markers.Clear();
            foreach(var m in this.Markers)
            {

                Graph.Markers.Add(m);

            }
           // Graph.Add(Lights.ToArray());
            foreach (var l in Lights)
            {

                var lg = new GraphSprite(new Tex2D("content/edit/light.png", true),null, 64, 64);
                lg.X = l.X;
                lg.Y = l.Y;
                lg.Z = l.Z;
             //   Graph.Add(lg);

            }

         
            return Graph;

        }

        private void NewMethod(SceneGraph2D Graph)
        {
            foreach (var hl in HL)
            {


                int mx = hl.X * TileWidth;
                int my = hl.Y * TileHeight;


                var hs = new GraphSprite(new Tex2D("content/edit/highlight1.png", true),null, 64, 64);

                hs.SetPos(mx, my);
                // tileSpr.SetPos(mx, my);


                Graph.Add(hs); ;

            }
        }
    }
}
