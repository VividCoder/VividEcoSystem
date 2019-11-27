using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using Vivid.Data;
using Vivid.Help;
using Vivid.Reflect;
using Vivid.Scripting;
namespace Vivid.Scene
{
    public delegate void NodeSelected(Node3D node);



    public enum Space
    {
        Local, World
    }

    public class Node3D : FusionBase
    {
        public ClassIO ClassCopy
        {
            get;
            set;
        }


        public bool NoSave
        {
            get;
            set;
        }

        public void Changed()
        {
        }

        public List<string> RenderTags = new List<string>();

        public bool BreakTop = false;

        public bool CastDepth = true;

        public bool CastShadows = true;

        public bool FaceCamera = false;

        public VInfoMap<string, object> Links = new VInfoMap<string, object>();

        public bool Lit = true;

        public EventHandler NameChanged = null;

        public bool On = true;

        public Matrix4 PrevWorld;

        public NodeSelected Selected = null;

        public List<Node3D> Sub = new List<Node3D>();

        public Node3D Top = null;

        private static int nn = 0;

        public string GetName()
        {
            return Name;
        }

        private readonly EventHandler PosChanged = null;

        private Vector3 _LocalPos = Vector3.Zero;

        private string _Name = "";


        public List<NodeScript> NodeScripts
        {

            get
            {
                return _NodeScripts;
            }
            set
            {
                _NodeScripts = value;
            }

        }

        public virtual void Read()
        {
            LocalTurn = Help.IOHelp.ReadMatrix();
            LocalPos = Help.IOHelp.ReadVec3();
            LocalScale = Help.IOHelp.ReadVec3();
            Name = Help.IOHelp.ReadString();
            ReadScripts();


        }
        public virtual void Write()
        {
            Help.IOHelp.WriteMatrix(LocalTurn);
            Help.IOHelp.WriteVec(LocalPos);
            Help.IOHelp.WriteVec(LocalScale);
            Help.IOHelp.WriteString(Name);
            WriteScripts();

        }

        public void ReadClass(object cls)
        {
            var cn = IOHelp.ReadString();
            var np = IOHelp.ReadInt();
            foreach (var prop in cls.GetType().GetProperties())
            {
                var pn = IOHelp.ReadString();
                int pt = IOHelp.ReadInt();
                switch (pt)
                {
                    case -1:
                        prop.SetValue(cls, null);
                        break;
                    case 0:
                        prop.SetValue(cls, IOHelp.ReadString());
                        break;
                    case 1:
                        prop.SetValue(cls, IOHelp.ReadInt());
                        break;
                    case 2:
                        prop.SetValue(cls, IOHelp.ReadFloat());
                        break;
                    case 3:
                        prop.SetValue(cls, IOHelp.ReadDouble());
                        break;
                    case 4:
                        prop.SetValue(cls, IOHelp.ReadTexture2D());
                        break;
                    case 5:
                        prop.SetValue(cls, IOHelp.ReadBool());
                        break;
                    case 6:
                        prop.SetValue(cls, IOHelp.ReadVec3());
                        break;
                    case 7:
                        prop.SetValue(cls, IOHelp.ReadVec4());
                        break;
                    case 8:
                        prop.SetValue(cls, IOHelp.ReadMatrix());
                        break;
                    case 9:

                        dynamic nl = prop.GetValue(cls);

                        nl.Clear();

                        int ec = IOHelp.ReadInt();

                        for (int i = 0; i < ec; i++)
                        {

                        }


                        break;

                }
            }
        }

        public void WriteClass(object cls)
        {

            IOHelp.WriteString(cls.GetType().Name);
            IOHelp.WriteInt(cls.GetType().GetProperties().Length);
            foreach (var prop in cls.GetType().GetProperties())
            {

                IOHelp.WriteString(prop.GetType().Name);
                var val = prop.GetValue(cls);
                WriteVal(val);

            }

        }

        private void WriteVal(object val)
        {
            if (val == null)
            {
                IOHelp.WriteInt(-1);
            }
            else
            if (val is string)
            {
                IOHelp.WriteInt(0);
                IOHelp.WriteString(val as string);
            }
            else
            if (val is int)
            {
                IOHelp.WriteInt(1);
                IOHelp.WriteInt((int)val);
            }
            else
            if (val is float)
            {
                IOHelp.WriteInt(2);
                IOHelp.WriteFloat((float)val);
            }
            else
            if (val is double)
            {
                IOHelp.WriteInt(3);
                IOHelp.WriteDouble((double)val);
            }
            else
            if (val is Texture.Texture2D)
            {
                IOHelp.WriteInt(4);
                IOHelp.WriteTexture2D(val as Texture.Texture2D);
            }
            else if (val is bool)
            {
                IOHelp.WriteInt(5);
                IOHelp.WriteBool((bool)val);
            }
            else if (val is Vector3)
            {
                IOHelp.WriteInt(6);
                IOHelp.WriteVec((Vector3)val);
            }
            else if (val is Vector4)
            {
                IOHelp.WriteInt(7);
                IOHelp.WriteVec((Vector4)val);
            }
            else if (val is Matrix4)
            {
                IOHelp.WriteInt(8);
                IOHelp.WriteMatrix((Matrix4)val);
            }
            else
            {

                if (val.GetType().Name.Contains("List"))
                {

                    IOHelp.WriteInt(9);

                    dynamic vo = val;

                    IOHelp.WriteInt(vo.Count);

                    //dynamic vo = val;

                    foreach (dynamic vi in vo)
                    {

                        WriteVal(vi);

                    }

                }
                else
                {
                    //    IOHelp.WriteInt(10);
                    //     WriteClass(val);
                }


            }
        }

        public void WriteScripts()
        {

            //
            Help.IOHelp.WriteInt(NodeScripts.Count);
            foreach (var scr in NodeScripts)
            {
                Help.IOHelp.WriteString(scr.FullPath);
                scr.SaveNode();
            }
        }

        public void ReadScripts()
        {


            NodeScripts.Clear();
            int ns = Help.IOHelp.ReadInt();
            for (int i = 0; i < ns; i++)
            {
                string path = Help.IOHelp.ReadString();
                var cs = NodeScriptCompiler.Compile(path);
                cs.LoadNode();
                NodeScripts.Add(cs);
                cs.Node = this;

            }
        }

        private List<NodeScript> _NodeScripts = new List<NodeScript>();
        public void AddScript(NodeScript ns)
        {
            NodeScripts.Add(ns);
        }

        public void InitUIScripts(Vivid.Resonance.UI ui)
        {
            foreach (var scr in NodeScripts)
            {
                scr.InitUI(ui);
            }
        }

        public void InitScripts()
        {
            foreach (var scr in NodeScripts)
            {
                scr.InitNode();
            }
        }

        public void SetPos(Vector3 pos)
        {
            LocalPos = pos;
        }

        public void UpdateScripts()
        {
            foreach (var scr in NodeScripts)
            {
                if (scr.Updated)
                {
                    Console.WriteLine("Updated:" + scr.FullPath);
                }
                scr.UpdateNode();
            }
        }
        public static Node3D LoadNode(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            Help.IOHelp.r = r;

            int et = IOHelp.ReadInt();
            switch (et)
            {
                case 1:
                    var new_ent = new Entity3D();
                    new_ent.Read();
                    fs.Close();
                    return new_ent;
                    break;
                case 2:
                    var new_node = new Node3D();
                    new_node.Read();
                    fs.Close();
                    return new_node;
                    break;
            }

            fs.Close();
            return null;
        }
        public void SaveNode(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            Help.IOHelp.w = bw;

            if (this is Entity3D)
            {
                IOHelp.WriteInt(1);
                this.Write();
            }
            else if (this is Node3D)
            {
                IOHelp.WriteInt(2);
                this.Write();
            }

            bw.Flush();
            fs.Flush();
            fs.Close();


        }

        public void DrawScripts()
        {

            foreach (var scr in NodeScripts)
            {
                scr.RenderNode();
            }

        }


        public Node3D()
        {
            Init();
            Rot(new Vector3(0, 0, 0), Space.Local);
            Name = "Node" + nn;
            nn++;
            LocalPos = new Vector3(0, 0, 0);
            LocalEular = new Vector3(0, 0, 0);
            LocalScale = new Vector3(1, 1, 1);
            LocalTurn = Matrix4.Identity;
            Running = false;
            RenderTags.Add("All");
            NoSave = false;

        }

        public bool AlwaysAlpha
        {
            get
                  ;
            set;
        }

        public Vector3 LocalEular
        {
            get;
            set;
        }

        public Vector3 LocalPos
        {
            get => _LocalPos;
            set
            {
                _LocalPos = value;
                PosChanged?.Invoke(this, null);
            }
        }

        public Vector3 LocalScale
        {
            get
            {
                return _LocalScale;
            }
            set
            {
                _LocalScale = value;
                // Console.WriteLine("Set Scale:" + value + " Name:" + Name);
            }
        }

        private Vector3 _LocalScale = Vector3.Zero;

        public Matrix4 LocalTurn
        {
            get;
            set;
        }

        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                NameChanged?.Invoke(this, null);
            }
        }

        public bool Running
        {
            get;
            set;
        }



        //public List<NodeScript> Scripts = new List<NodeScript>();

        public List<Node3D> TopList
        {
            get
            {
                List<Node3D> tl = new List<Node3D>();
                if (Top != null)
                {
                    Top.AddTop(tl);
                    return tl;
                }
                else
                {
                    return null;
                }
                return null;
            }
        }

        public Node3D TopTop
        {
            get
            {
                if (Top != null)
                {
                    return Top.TopTop;
                }
                else
                {
                    return this;
                }
            }
        }

        public Matrix4 WorldNoScale
        {
            get
            {
                Matrix4 r = Matrix4.Identity;
                if (Top != null)
                {
                    r = Top.World;
                }

                r = (LocalTurn * Matrix4.CreateTranslation(LocalPos)) * r;

                return r;
            }
        }

        public Matrix4 World
        {
            get
            {
                Matrix4 r = Matrix4.Identity;
                if (Top != null)
                {
                    r = Top.World;
                }

                r = (Matrix4.CreateScale(LocalScale) * LocalTurn * Matrix4.CreateTranslation(LocalPos)) * r;

                return r;
            }
        }

        public Vector3 WorldPos
        {
            get
            {
                Matrix4 v = World;

                return v.ExtractTranslation();
            }
        }

        public void Add(Node3D node)
        {
            Sub.Add(node);
            node.Top = this;
        }

        public virtual void SetLightmapTex(Vivid.Texture.Texture2D tex)
        {
        }

        public virtual void AddLink(string name, object obj)
        {
            Links.Add(name, obj);
        }

        public void AddProxy(Node3D node)
        {
            Sub.Add(node);
        }

        public void AddTop(List<Node3D> l)
        {
            l.Add(this);
            if (BreakTop)
            {
                return;
            }

            if (Top != null)
            {
                Top.AddTop(l);
            }
        }

        public void CopyProps()
        {
            ClassCopy = new ClassIO(this);
            ClassCopy.Copy();
            foreach (var cs in NodeScripts)
            {
                cs.ClassCopy = new ClassIO(cs);
                cs.ClassCopy.Copy();
            }
        }

        public void RestoreProps()
        {
            ClassCopy.Reset();
            foreach (var cs in NodeScripts)
            {
                cs.ClassCopy.Reset();
            }
        }

        public void Begin()
        {
            Running = true;

            foreach (Node3D ent in Sub)
            {
                ent.Begin();
            }
        }

        public void Pause()
        {
            Running = false;
            foreach (Node3D ent in Sub)
            {
                ent.Pause();
            }
        }

        public void Resume()
        {
            Running = true;
            foreach (Node3D ent in Sub)
            {
                ent.Resume();
            }
        }

        public void End()
        {
            Running = false;

            foreach (Node3D ent in Sub)
            {
                ent.End();
            }
        }

        public virtual void Init()
        {
        }

        public void LookAt(Node3D n)
        {
            LookAt(n.WorldPos, new Vector3(0, 1, 0));
        }

        public void LookAt(Vector3 p, Vector3 up)
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p - LocalPos, up);
            //Console.WriteLine("Local:" + LocalPos.ToString() + " TO:" + p.ToString());
            //m=m.ClearTranslation();

            //   m = m.Inverted();
            //m = m.ClearScale();
            //m = m.ClearProjection();
            m = m.Inverted();

            LocalTurn = m;
        }

        public void LookAtZero(Vector3 p, Vector3 up)
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p, up);
            LocalTurn = m;
        }

        public void Move(Vector3 v, Space s)
        {
            // v.X = -v.X;
            if (s == Space.Local)
            {
                //Console.WriteLine("NV:" + v);
                Vector3 ov = WorldPos;
                Vector3 nv = Vector3.TransformPosition(v, WorldNoScale);
                Vector3 mm = nv - ov;

                LocalPos = LocalPos + mm;//Matrix4.Invert(nv);
                //LocalPos = LocalPos + new Vector3(nv.X, nv.Y, nv.Z);
            }
        }

        public void Pos(Vector3 p, Space s)
        {
            if (s == Space.Local)
            {
                LocalPos = p;
            }
        }

        public virtual void Present(Cam3D c)
        {
        }

        // public void LookAt(Vector3 t)
        //{
        //   LocalTurn = Matrix4.LookAt(WorldPos, t, Vector3.UnitY);
        // }
        public virtual void PresentDepth(Cam3D c)
        {
        }

        public SceneGraph3D Graph = null;

        public virtual void Rot(Vector3 r, Space s)
        {
            if (s == Space.Local)
            {
                LocalTurn = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
                if (Graph != null)
                {
                    this.Graph.SceneChanged = true;
                }
            }
        }

        public void Select()
        {
            Selected?.Invoke(this);
        }

        public void SetLightmap()
        {
            if (this is Entity3D || this is Terrain.Terrain3D)
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.VRLightMap();
            }
            foreach (Node3D n in Sub)
            {
                n.SetLightmap();
            }
        }

        public void SetMultiPass()
        {
            if (this is Entity3D || this is Terrain.Terrain3D)
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.RMultiPass();
            }
            foreach (Node3D n in Sub)
            {
                n.SetMultiPass();
            }
        }

        public void StartFrame()
        {
            PrevWorld = World;
        }

        public Vector3 Transform(Vector3 p)
        {
            return Vector3.TransformPosition(p, World);
        }

        private Vector3 prot = Vector3.Zero;

        public virtual void Turn(Vector3 r, Space s = Space.World)
        {
            if (s == Space.World)
            {
                Matrix4 t = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
                LocalTurn = LocalTurn * t;//.Inverted();
                //verified and correct.
            }
            else
            {
                //in progress.

                //prot = prot + r;
                //r = prot;


                Matrix4 t = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
                LocalTurn = t * LocalTurn;
            }
        }




        public virtual void Update()
        {
            UpdateNode(1.0f);
        }

        public virtual void UpdateNode(float t)
        {

            foreach (Node3D n in Sub)
            {
                n.UpdateNode(t);
            }
        }

        public void Edit(EditNode edit)
        {
            edit(this);
            foreach (Node3D sub in Sub)
            {
                sub.Edit(edit);
            }
        }
    }

    public delegate void EditNode(Node3D node);
}