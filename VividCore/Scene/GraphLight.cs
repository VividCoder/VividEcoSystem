using OpenTK;

using System.IO;

namespace Vivid.Scene
{
    public class GraphLight : GraphNode
    {
        public Vector3 Diffuse
        {
            get;
            set;
        }

        public Vector3 Specular
        {
            get;
            set;
        }

        public float Shiny
        {
            get;
            set;
        }

        public float Range
        {
            get;
            set;
        }

        public LightType Type
        {
            get;
            set;
        }

        public bool On
        {
            get;
            set;
        }

        public bool CastShadows
        {
            get;
            set;
        }

        public GraphLight()
        {
            Diffuse = new Vector3(0.5f, 0.5f, 0.5f);
            Specular = new Vector3(0, 0, 0);
            Shiny = 0.2f;
            Range = 500;
            Type = LightType.Point;
            On = true;
            CastShadows = true;
        }

        public FrameBuffer.FrameBufferColor SB1;
        public FrameBuffer.FrameBufferColor SB2;
        public void CheckShadowSize(int w, int h)
        {
            if (SB1 == null)
            {
                CreateShadowBuffers(w, h);
            }
            if (SB1.IW != w || SB1.IH != h)
            {
                CreateShadowBuffers(w, h);
            }

        }
        public void RenderShadowBuffer(SceneGraph2D graph)
        {

            if (graph.Root == null) return;
            if (SB1 == null)
            {
                CreateShadowBuffers(App.AppInfo.RW, App.AppInfo.RH);
            }

            SB1.Bind();
            Vivid.Draw.Render.Begin();
            graph.DrawNodeShadow(graph.Root);

            //LitImage.Light = Lights[0];
            //LitImage.Graph = this;
            // if (LitImage.Light != null)
            //{

            graph.ShadowImage.Bind();


            Vivid.Draw.Render.SetBlend(Vivid.Draw.BlendMode.Alpha); ;

            Vivid.Draw.Render.End2D();


            graph.ShadowImage.Release();
            // }


            SB1.Release();


            SB2.Bind();

            //Graph.ShadowBuf.BB.Bind(0);

            graph.DrawShadow.Graph = graph;
            graph.DrawShadow.Light = this;


            Vivid.Draw.IntelliDraw.BeginDraw();
            Vivid.Draw.IntelliDraw.DrawImg(0, 0, Vivid.App.AppInfo.RW, Vivid.App.AppInfo.RH, SB1.BB, new Vector4(1, 1, 1f, 1), true);
            Vivid.Draw.IntelliDraw.EndDraw(graph.DrawShadow);

            SB2.Release();

            //return;


            SB1.Bind();

            Vivid.Draw.IntelliDraw.BeginDraw();
            Vivid.Draw.IntelliDraw.DrawImg(0, 0, Vivid.App.AppInfo.RW, Vivid.App.AppInfo.RH, SB2.BB, new Vector4(1, 1, 1f, 1), false);
            Vivid.Draw.IntelliDraw.EndDraw(graph.BlurShadow);

            SB1.Release();


        }

        private void CreateShadowBuffers(int w, int h)
        {
            SB1 = new FrameBuffer.FrameBufferColor(w, h);
            SB2 = new FrameBuffer.FrameBufferColor(w, h);
        }

        public void Write(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Diffuse.X);
            w.Write(Diffuse.Y);
            w.Write(Diffuse.Z);
            w.Write(Specular.X);
            w.Write(Specular.Y);
            w.Write(Specular.Z);
            w.Write(Shiny);
            w.Write(Range);
            w.Write(On);
            w.Write(CastShadows);
            w.Write((int)Type);
        }

        public void Read(BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            Vector3 d = new Vector3
            {
                X = r.ReadSingle(),
                Y = r.ReadSingle(),
                Z = r.ReadSingle()
            };
            Diffuse = d;
            Vector3 s = new Vector3
            {
                X = r.ReadSingle(),
                Y = r.ReadSingle(),
                Z = r.ReadSingle()
            };
            Specular = s;
            Shiny = r.ReadSingle();
            Range = r.ReadSingle();
            On = r.ReadBoolean();
            CastShadows = r.ReadBoolean();
            Type = (LightType)r.ReadInt32();
        }
    }

    public enum LightType
    {
        Point, Directional, Ambient, Spot
    }
}