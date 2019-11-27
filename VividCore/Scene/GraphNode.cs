using OpenTK;

using System;
using System.Collections.Generic;
using System.IO;

using Vivid.Reflect;
using Vivid.Util;

namespace Vivid.Scene
{
    public class GraphNode
    {
        public ClassIO ClassCopy
        {
            get;
            set;
        }
        public bool CastShadow
        {
            get;
            set;
        }

        public bool RecvShadow
        {
            get;
            set;
        }
        public List<GraphNode> Nodes
        {
            get;
            set;
        }

        public Tex.Tex2D ImgFrame
        {
            get;
            set;
        }

        public Tex.Tex2D NormalMap
        {
            get;
            set;
        }

        public float ShadowPlane
        {
            get;
            set;

        }

        public dynamic[] Obj = new dynamic[255];

        public void CopyProps()
        {
            ClassCopy = new ClassIO(this);
            ClassCopy.Copy();
        }

        public void RestoreProps()
        {
            ClassCopy.Reset();
        }

        public int TileX
        {
            get;
            set;
        }

        public int TileY
        {
            get;
            set;
        }

        public int TileZ
        {
            get;
            set;
        }

        public System.Drawing.Bitmap ImgBitmap
        {
            get => _ImgBit;
            set
            {
                if (value == null)
                {
                    return;
                }

                _ImgBit = value;
                ImgFrame = new Tex.Tex2D(value, true)
                {
                    Name = ImgBitmap.ToString()
                };
                W = ImgFrame.Width;
                H = ImgFrame.Height;
            }
        }

        private System.Drawing.Bitmap _ImgBit;

        public string Name
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

        public float W
        {
            get;
            set;
        }

        //public float _W = 0;

        public float H
        {
            get;
            set;
        }

        public GraphNode Root
        {
            get;
            set;
        }

        public float[] XC = new float[4];
        public float[] YC = new float[4];
        public Vector2[] DrawP = null;

        public void EditMove(float x, float y)
        {
            Vector2 r = Util.Maths.Rotate(x, y, Graph.Rot, 1.0f);
            X = X + r.X;
            Y = Y + r.Y;
        }




        public void SyncCoords()
        {
            if (Graph == null) return;
            int sw = Vivid.App.AppInfo.RW;
            int sh = Vivid.App.AppInfo.RH;

            float[] ox = new float[4];
            float[] oy = new float[4];

            ox[0] = (-W / 2);// * Graph.Z * Z;
            ox[1] = (W / 2);// * Graph.Z * Z;
            ox[2] = (W / 2);// * Graph.Z* Z ;
            ox[3] = (-W / 2);// *Graph.Z*Z;

            oy[0] = (-H / 2);// * Graph.Z*Z;
            oy[1] = (-H / 2);// *Graph.Z*Z;
            oy[2] = (H / 2);// * Graph.Z * Z;
            oy[3] = (H / 2);// * Graph.Z * Z;

            Vector2[] p = Maths.RotateOC(ox, oy, Rot, Z, 0, 0);

            p = Maths.Push(p, X - Graph.X, Y - Graph.Y);

            p = Maths.RotateOC(p, Graph.Rot, Graph.Z, 0, 0);

            p = Maths.Push(p, sw / 2, sh / 2);

            DrawP = p;

            //p = Maths.Push(p, X, Y);

            //p = Maths.Push(p,sw / 2, sh / 2);

            //p = Maths.Push(p, X+sw/2, Y+sh/2, Graph.Z);

            // Draw.Render.Image(p, ImgFrame);
        }

        public SceneGraph2D Graph
        {
            get;
            set;
        }

        public GraphNode()
        {
            W = 64;
            H = 64;
            RecvShadow = true;
            Z = 1.0f;
            Nodes = new List<GraphNode>();
            ShadowPlane = 0;
        }

        public void Translate(float x, float y, float z = 0.0f)
        {
            X = X + x;
            Y = Y + y;
            Z = Z + z;
        }

        public void Move(float x, float y, float z = 0.0f)
        {
            Vector2 r = Util.Maths.Rotate(x, y, 360 - Rot, 1.0f);
            X = X + r.X;
            Y = Y - r.Y;
            Z = Z + z;
        }

        public void Point(float x, float y)
        {
            double r = Math.Atan2(y, x);
            Rot = (float)r * (180.0f / (float)Math.PI);
        }

        public void SetPos(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetZ(float z)
        {
            Z = z;
        }

        public void Rotate(float r)
        {
            Rot = Rot + r;
            if (Rot < 0)
            {
                Rot = 360.0f + Rot;
            }

            if (Rot > 360)
            {
                Rot = Rot - 360.0f;
            }
        }

        public void Scale(float z)
        {
            Z = Z + z;
        }

        public void SetRotate(float r)
        {
            Rot = r;
        }

        public GraphNode Node
        {
            get;
            set;
        }

        public virtual void Init()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Draw()
        {
        }

        public string ImgLinkName = "";

        public void Read(BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            Name = r.ReadString();

            if (r.ReadBoolean())
            {
                bool alpha = r.ReadBoolean();
                string tp = r.ReadString();
                string tn = r.ReadString();
                ImgLinkName = tp;
                // Console.WriteLine("TN:" + tn + " TP:" + tp); ImgFrame = new Tex.Tex2D(tp, alpha);
            }

            int nc = r.ReadInt32();

            for (int i = 0; i < nc; i++)
            {
                GraphNode nn = new GraphNode
                {
                    //nn.Roo = this;

                    Graph = Graph
                };
                Nodes.Add(nn);
                nn.Root = this;
                nn.Read(r);
            }
        }

        public void Write(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Name);
            if (ImgFrame != null)
            {
                w.Write(true);
                w.Write(ImgFrame.Alpha);
                w.Write(ImgFrame.Path);
                w.Write(ImgFrame.Name);
            }
            else
            {
                w.Write(false);
            }

            w.Write(Nodes.Count);

            foreach (GraphNode n in Nodes)
            {
                n.Write(w);
            }
        }
    }
}