using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid;
using Vivid.App;
using Vivid.State;
using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.Tex;
using Vivid.Texture;
using SpaceEngine;
using SpaceEngine.Map.Layer;
using SpaceEngine.Map;
using SpaceEngine.Forms;
using SpaceEngine.Map.Tile;
using SpaceEngine.Map.TileSet;
namespace MapEditor.Forms
{

    public class TileSetTabPage : TabPageForm
    {
        public Map TileMap;
        public MapLayer TileLayer;
        public MapViewForm TView;
        public int tx, ty;
        int lmx, lmy;
        public Vivid.Scene.GraphNode ONode;
        public TileSetTabPage(string name) : base(name)
        {
            tx = 0;
            ty = 0;
            TileMap = new Map();
            TileLayer = new MapLayer(16, 16,TileMap);
            TileMap.AddLayer(TileLayer);

           // var l1 = new Vivid.Scene.GraphLight();



            TView = new MapViewForm(TileMap,false);
            Add(TView);
            AfterSet = () =>
            {

                TView.Set(0, 0, W, H);

            };

           

            TView.MouseDown = (b) =>
            {
                if (TView.Graph != null)
                {
                    var node = ONode;//TView.Graph.Pick(lmx, lmy);


                    if (node != null)
                    {

                        TileBrowser.ActiveTile = node.Obj[0];

                    }
                }

            };

            TView.MouseMove = (x, y, dx, dy) =>
            {
                lmx = x;
                lmy = y;
                if (TView.Graph != null)
                {
                    AppInfo.RW = TView.MapFrame.IW;
                    AppInfo.RH = TView.MapFrame.IH;
                    var node = TView.Graph.Pick(x, y);
                    AppInfo.RW = AppInfo.W;
                    AppInfo.RH = AppInfo.H;
                    ONode = node;
                    if (node != null)
                    {

                        var tView = TileMap;
                        TView.Map.HL.Clear();
                        TView.Map.HighlightTile(node.TileX, node.TileY);
                        TView.UpdateGraph();
                        //TView.Graph.X = -32 + TView.W / 2;
                       // TView.Graph.Y = -32 + TView.H / 2;
                        // Console.WriteLine("MX:" + x + " MY:" + y);
                    }
                    else
                    {

                        ClearHL(TView);

                    }
                }
            };



            //TileMap.AddLight(l1);
        }
        private void ClearHL(MapViewForm tileSet_View)
        {
            //var tView = Map;

            if (TView.Graph != null)
            {

                if (TView.Map.HL.Count > 0)
                {

                    TView.Map.HL.Clear();
                    tileSet_View.UpdateGraph();
                  //  tileSet_View.Graph.X = -32 + tileSet_View.W / 2;
                   // tileSet_View.Graph.Y = -32 + tileSet_View.H / 2;


                }

            }
        }
    }


    public class TileBrowser : WindowForm
    {
        public TabForm Tab = null;

        public MapViewForm Map = null;
        public static Tile ActiveTile = null;
        public TabPageForm GetActivePage()
        {

            return Tab.Shown;
            
        }

        public Map TileMap;
        public MapLayer TileLayer;
        int tx, ty;
        public void AddTileSet(TileSet ts)
        {

            var actp = GetActivePage() as TileSetTabPage;
            if (actp != null)
            {
              
                foreach(var t in ts.Tiles)
                {

                    actp.TileLayer.SetTile(actp.tx, actp.ty, t);
                    actp.tx++;
                    if (actp.tx > 63)
                    {
                        actp.tx = 0;
                        actp.ty++;
                    }
                }

            }

          
            actp.TView.UpdateGraph();
           // actp.TView.Graph.X = -32 + actp.TView.W / 2;
            //actp.TView.Graph.Y = -32 + actp.TView.H / 2;
        }

        public void newSet()
        {

            var setPage = new TileSetTabPage("New set");
            setPage.Set(0, 0, body.W, body.H);
            Tab.AddPage(setPage);
            


           
        }

        public TileBrowser()
        {

            Tab = new TabForm();
           

       

            body.Add(Tab);
            //body.Add(Map);
            
            AfterSet = () =>
            {
                Tab.X = 0;
                Tab.Y = -15;
                Tab.W = W;
                Tab.H = body.H;
               // Map.Set(0, 0, body.W, body.H);
            };

        }

     

    }
}
