using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vivid.Draw;
using Vivid.FXS;
using Vivid.Reflect;

namespace Vivid.Scene
{
    public class SceneGraph2D
    {
        public ClassIO ClassCopy
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            set;
        }
        public FXBlurShadow BlurShadow
        {
            get;
            set;

        }
        public FXDrawShadow DrawShadow
        {
            get;
            set;
        }
        public FXShadowImage ShadowImage
        {
            get;
            set;
        }
        public FXImage UnlitImage
        { get;
            set;
        }
        public FXLitImage LitImage
        {
            get;
            set;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Z
        {
            get;
            set;
        }

        public float Rot
        {
            get;
            set;
        }

        public GraphNode Root
        {
            get;
            set;
        }

        public List<GraphLight> Lights
        {
            get;
            set;
        }

        public static FrameBuffer.FrameBufferColor ShadowBuf = null;
        public static Texture.Texture2D White1 = null;
        static int sc = 0;
        public SceneGraph2D()
        {
           // Console.WriteLine("Created Scene:" + sc);
            sc++;
            White1 = new Texture.Texture2D("data/tex/white1.png", Texture.LoadMethod.Single, false);

            Running = false;
            X = 0;
            Y = 0;
            Z = 1;
            Rot = 0;
            Root = new GraphNode();
            Lights = new List<GraphLight>();
            LitImage = new FXLitImage();
            ShadowImage = new FXShadowImage();
            DrawShadow = new FXDrawShadow();
            BlurShadow = new FXBlurShadow();
            UnlitImage = new FXImage();
        }

        public void Copy()
        {
            ClassCopy = new Reflect.ClassIO(this);
            ClassCopy.Copy();
            CopyNode(Root);
        }

        public void CopyNode(GraphNode node)
        {
            node.CopyProps();
            foreach (GraphNode nn in node.Nodes)
            {
                CopyNode(nn);
            }
        }

        public void Restore()
        {
            ClassCopy.Reset();
            RestoreNode(Root);
        }

        public void RestoreNode(GraphNode node)
        {
            node.RestoreProps();
            foreach (GraphNode nn in node.Nodes)
            {
                RestoreNode(nn);
            }
        }

        public void Add(GraphNode node)
        {
            node.Graph = this;
            Root.Nodes.Add(node);
            node.Root = Root;
        }

        public void Add(params GraphNode[] nodes)
        {
            foreach (GraphNode node in nodes)
            {
                node.Graph = this;
                Root.Nodes.Add(node);
                node.Root = Root;
            }
        }

        public void Add(GraphLight node, bool toGraph = false)
        {
            if (toGraph)
            {
                Root.Nodes.Add(node);
            }
            node.Graph = this;
            Lights.Add(node);
        }

        public void Add(params GraphLight[] lights)
        {
            foreach (GraphLight light in lights)
            {
                light.Graph = this;
                Lights.Add(light);
            }
        }

        public void Translate(float x, float y)
        {
            X = X + x;
            Y = Y + y;
        }

        public void Move(float x, float y)
        {
            Vector2 r = Util.Maths.Rotate(-x, -y, (180.0f - Rot), 1.0f);

            X = X + r.X / Z;
            Y = Y + r.Y / Z;
        }

        public void Update()
        {
            if (Running)
            {
                UpdateNode(Root);
            }
            else
            {
            }
        }

        public void UpdateNode(GraphNode node)
        {
            node.Update();

            foreach (GraphNode sub in node.Nodes)
            {
                UpdateNode(sub);
            }
        }

        public void DrawNode(GraphNode node)
        {
            //if(node.ImgFrame == null)

            if (node.ImgFrame != null)
            {
                if (node.ImgFrame.Width < 2)
                {
                    Console.WriteLine("Illegal Image ID:" + node.ImgFrame.ID);
                    while (true)
                    {
                    }
                }

                bool first = true;



                if (first)
                {
                    //     Render.SetBlend(BlendMode.Alpha);
                    first = false;
                }
                else
                {
                    //       Render.SetBlend(BlendMode.Add);
                }

                //    LitImage.Bind();

                float[] xc;
                float[] yc;

                node.SyncCoords();

                xc = node.XC;
                yc = node.YC;


             
                Render.Image(node.DrawP, node.ImgFrame,node.NormalMap ,node.ShadowPlane);
            
                //Render.Image(xc, yc, node.ImgFrame);



            }
            foreach (GraphNode snode in node.Nodes)
            {
                DrawNode(snode);
            }
        }

        public void CreateShadowBuf(int w, int h)
        {

            // ShadowBuf = new FrameBuffer.FrameBufferColor(w, h);

        }
        public static FrameBuffer.FrameBufferColor ShadowBuffer2;
        public void BindShadowBuf(FrameBuffer.FrameBufferColor from)
        {
            if (ShadowBuffer2 == null || ShadowBuffer2.IW != ShadowBuf.IW || ShadowBuffer2.IH != ShadowBuf.IH)
            {
                //ShadowBuffer2 = new FrameBuffer.FrameBufferColor(ShadowBuf.IW, ShadowBuf.IH);

            }
            ShadowBuffer2.Bind();



        }

        public void ReleaseShadowBuf()
        {

            ShadowBuffer2.Release();

        }
        public static FrameBuffer.FrameBufferColor Shadow3 = null;
        public void DrawShadowBuf()
        {

            Lights[0].RenderShadowBuffer(this);
            //ShadowBuffer2.Bind();

        }

        public void BindShadowBuf2()
        {
            BindShadowBuf(null);





        }

        public void ReleaseShadowBuf2()
        {
            ShadowBuffer2.Release();

        }

        public void DrawNodeShadow(GraphNode node)
        {
            if (node.ImgFrame != null && node.CastShadow)
            {
                if (node.ImgFrame.Width < 2)
                {
                    Console.WriteLine("Illegal Image ID:" + node.ImgFrame.ID);
                    while (true)
                    {
                    }
                }

                bool first = true;



                if (first)
                {
                    //     Render.SetBlend(BlendMode.Alpha);
                    first = false;
                }
                else
                {
                    //       Render.SetBlend(BlendMode.Add);
                }

                //    LitImage.Bind();

                float[] xc;
                float[] yc;

                node.SyncCoords();

                xc = node.XC;
                yc = node.YC;

                Render.Image(node.DrawP, node.ImgFrame,null);

                //Render.Image(xc, yc, node.ImgFrame);



            }
            foreach (GraphNode snode in node.Nodes)
            {
                DrawNodeShadow(snode);
            }
        }
        public void DrawSingleNode(GraphNode node)
        {

            Render.Begin();
            node.SyncCoords();
            DrawNode(node);
            UnlitImage.Bind();
            Render.End2D();
            UnlitImage.Release();

        }
        public void Draw(bool shadows)
        {
            // OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Blend);

            bool first = true;
            if(Lights.Count == 0)
            {

                Render.Begin();
                DrawNode(Root);
                UnlitImage.Bind();
                Render.End2D();
                UnlitImage.Release();
            }

            foreach (var l in Lights)
            {

                Render.Begin();
                DrawNode(Root);
                LitImage.Light = l;
                LitImage.Graph = this;
                LitImage.LightZ = l.Z;
              //  Console.WriteLine("LZ:" + l.Z);
                if (LitImage.Light != null)
                {


                    LitImage.Bind();
                    Render.SetBlend(BlendMode.Alpha); ;
                    if (shadows)
                    {
                        l.SB1.BB.Bind(1);
                    }
                    else
                    {
                        White1.Bind(1);
                    }
                    if (first)
                    {
                        Render.SetBlend(BlendMode.Alpha);
                        first = false;
                    }
                    else
                    {
                        Render.SetBlend(BlendMode.Add);
                    }

                    Render.End2D();
                    if (shadows)
                    {
                        l.SB1.BB.Release(1);
                    }
                    else
                    {
                        White1.Release(1);
                    }
                    LitImage.Release();
                }

            }
        }

        private float sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        private bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        public GraphNode PickNode(GraphNode node, int x, int y)
        {
            foreach (GraphNode n in node.Nodes)
            {
                GraphNode p = PickNode(n, x, y);
                if (p != null)
                {
                    return p;
                }
            }
            //if (node.DrawP == null)
           // {

                node.SyncCoords();
            
            //Console.WriteLine("XP:" + node.DrawP[0].X + " YP:" + node.DrawP[0].Y);

            
            if (node.DrawP != null)
            {
                if (PointInTriangle(new Vector2(x, y), node.DrawP[0], node.DrawP[1], node.DrawP[2]))
                {
                    return node;
                }
                else if (PointInTriangle(new Vector2(x, y), node.DrawP[2], node.DrawP[3], node.DrawP[0]))
                {
                    return node;
                }
            }
            return null;
        }

        public class PickSite
        {
            public int X, Y, Z;
        }


        public GraphNode Pick(int x, int y)
        {
            return PickNode(Root, x, y);
        }

        public Vector2 GetPoint(float x, float y)
        {
            int w, h;
            w = App.AppInfo.RW;
            h = App.AppInfo.RH;
            Vector2 r = new Vector2(x, y);
            r = Util.Maths.Push(r, -w / 2, -h / 2);
            r = Util.Maths.Rotate(r.X, r.Y, Rot, 1);
            r.X = r.X + X;
            r.Y = r.Y + Y;
            return r;
        }

        public void Load(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            ReadGraph(r);
            Root.Read(r);
            fs.Close();
            r = null;
            fs = null;
        }

        public void ReadGraph(BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            int lc = r.ReadInt32();
            for (int i = 0; i < lc; i++)
            {
                GraphLight nl = new GraphLight();
                nl.Read(r);
                Add(nl);
            }
            Root = new GraphNode
            {
                Graph = this
            };
            Root.Read(r);
        }

        public void Save(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);
            WriteGraph(w);
            fs.Flush();
            fs.Close();
            w = null;
            fs = null;
        }

        public void WriteGraph(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Lights.Count());
            foreach (GraphLight l in Lights)
            {
                l.Write(w);
            }

            Root.Write(w);
        }
    }
}