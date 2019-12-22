using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Scene
{
    public class Graph2DRain
    {

        public SceneGraph2D Graph = null;
        public List<RainDrop> Drops = new List<RainDrop>();
        public void Init(int drops = 80)
        {

            for(int i = 0; i < drops; i++)
            {

                var rd = new RainDrop(Graph);
                Drops.Add(rd);


            }

        }

        public void Update()
        {
            rs:
            foreach(var rd in Drops)
            {
                rd.Update();
                if(rd.DropSprite.Y>2000)
                {
                    Drops.Remove(rd);
                    Graph.Root.Nodes.Remove(rd.DropSprite);
                    var nd = new RainDrop(Graph);
                    nd.DropSprite.Y *= 0.01f;
                    Drops.Add(nd);
                    goto rs;
                }
            }

        }

    }

    public class RainDrop
    {
        public static Random Rnd = null;
        public GraphSprite DropSprite;
        public static Tex.Tex2D DropImg;
        public float FallZ = 1.0f;
        public SceneGraph2D Graph;
        public RainDrop(SceneGraph2D graph)
        {
            if(DropImg == null)
            {
                Rnd = new Random(Environment.TickCount);
                DropImg = new Tex.Tex2D("Corona/img/fx/raindrop1.png", true);
            }
            DropSprite = new GraphSprite(DropImg,null, 16, 16);
            Graph = graph;

            DropSprite.X = Rnd.Next(-2500, 2500);
            DropSprite.Y = Rnd.Next(-500, 1000);
            FallZ = 0.4f + (float)Rnd.NextDouble();
           
            Graph.Add(DropSprite);
        }
        public void Update()
        {
            DropSprite.Y += 5.0f * FallZ;
            DropSprite.W = 8;
            DropSprite.H = 22 * FallZ;
            //DropSprite.ShadowPlane = 0.1f;

        }
        
    }

}
