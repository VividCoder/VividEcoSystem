using OpenTK;
using System.Collections.Generic;
using Vivid.Data;
using Vivid.Material;
using Vivid.Visuals;

namespace Vivid.Scene
{
    public class LightMapOptions
    {
        public bool ReceiveLight = true;
        public bool CastShadows = true;

        public LightMapOptions()
        {
        }
    }

    public class Entity3D : Node3D
    {
        public bool CastShadows
        {
            get;
            set;
        }

        public string GetName()
        {
            return Name;
        }

        public Matrix4 GlobalInverse;

        public List<Mesh3D> Meshes
        {
            get
            {
                return _Meshes;
            }
            set
            {
                _Meshes = value;
            }
        }

        private List<Mesh3D> _Meshes = new List<Mesh3D>();





        public Physics.PyObject PO = null;
        public Physics.PyType PyType;
        public Renderer Renderer = null;
        private float bw, bh, bd;

        private float sw, sh, sd;

        public LightMapOptions LightMapInfo = new LightMapOptions();

        public Entity3D()
        {
            CastShadows = true;

        }

        public Bounds Bounds
        {
            get
            {
                sw = sh = sd = 20000;
                bw = bh = bd = -20000;
                GetBounds(this);
                Bounds res = new Bounds
                {
                    W = bw - sw,
                    H = bh - sh,
                    D = bd - sd,
                    MinX = sw,
                    MaxX = bw,
                    MinY = sh,
                    MaxY = bh,
                    MinZ = sd,
                    MaxZ = bd
                };
                return res;
            }
        }



        public void AddMesh(Mesh3D mesh)
        {
            Meshes.Add(mesh);

        }

        public virtual void Bind()
        {
        }

        public void Clean()
        {
            Meshes = new List<Mesh3D>();
            Renderer = null;
        }

        public void AddTorque(OpenTK.Vector3 tr)
        {

            var pd = PO as Physics.PyDynamic;
            pd.AddTorque(tr);

        }

        public void AddForce(OpenTK.Vector3 force)
        {
            var pd = PO as Physics.PyDynamic;
            pd.ApplyForce(force);

        }

        public void EnablePy(Physics.PyType type)
        {
            PyType = type;
            switch (PyType)
            {
                case Physics.PyType.Box:
                    PO = new Physics.PyDynamic(type, this);
                    break;

                case Physics.PyType.Mesh:
                    PO = new Physics.PyStatic(this);
                    break;
            }
        }

        public void GetBounds(Entity3D node)
        {

            foreach (Mesh3D m in node.Meshes)
            {
                if (m.VertexData == null) return;
                for (int i = 0; i < m.VertexData.Length; i++)
                {
                    var vr = m.VertexData[i].Pos;

                    int vid = i * 3;
                    if (vr.X < sw)
                    {
                        sw = vr.X;
                    }
                    if (vr.X > bw)
                    {
                        bw = vr.X;
                    }
                    if (vr.Y < sh)
                    {
                        sh = vr.Y;
                    }
                    if (vr.Y > bh)
                    {
                        bh = vr.Y;
                    }
                    if (vr.Z < sd)
                    {
                        sd = vr.Z;
                    }
                    if (vr.Z > bd)
                    {
                        bd = vr.Z;
                    }
                }
            }
            foreach (Node3D snode in node.Sub)
            {
                if (snode is Entity3D || snode is Terrain.Terrain3D)
                {
                    GetBounds(snode as Entity3D);
                }
            }
        }

        public List<Vector3> GetAllVerts()
        {
            Entity3D node = this;
            List<Vector3> nl = new List<Vector3>();
            GetVerts(nl, this);
            return nl;
        }
        public List<int> GetAllTris()
        {
            List<int> l = new List<int>();
            GetTris(l, this);
            return l;
        }

        public void GetTris(List<int> tris, Entity3D node)
        {
            foreach (Mesh3D m in Meshes)
            {
                for (int i = 0; i < m.TriData.Length; i++)
                {
                    tris.Add(m.TriData[i].V0);
                    tris.Add(m.TriData[i].V1);
                    tris.Add(m.TriData[i].v2);
                }
            }
            foreach (var s_node in Sub)
            {
                GetTris(tris, (Entity3D)s_node);
            }
        }
        public void GetVerts(List<Vector3> verts, Entity3D node)
        {

            foreach (Mesh3D m in node.Meshes)
            {
                for (int i = 0; i < m.VertexData.Length; i++)
                {
                    //int vid = i * 3;

                    Vector3 nv = m.VertexData[i].Pos;// new Vector3(m.Vertices[vid], m.Vertices[vid + 1], m.Vertices[vid + 2]);
                    nv = Vector3.TransformPosition(nv, node.World);
                    verts.Add(nv);
                    // verts.Add(m.Vertices[vid]); verts.Add(m.Vertices[vid + 1]);
                    // verts.Add(m.Vertices[vid + 2]);
                }
            }
            foreach (Node3D snode in node.Sub)
            {
                GetVerts(verts, snode as Entity3D);
            }
        }

        public override void Init()
        {
            // Renderer = new VRMultiPass();
        }

        public virtual void PostRender()
        {
        }

        /// <summary> To be called AFTER data asscoiation.
        public virtual void PreRender()
        {
        }

        public override void Present(Cam3D c)
        {
            // GL.MatrixMode(MatrixMode.Projection); GL.LoadMatrix(ref c.ProjMat);
            SetMats(c);
            Bind();
            PreRender();
            Render();
            PostRender();
            Release();
            // foreach (var s in Sub) { s.Present(c); }
        }

        public void PresentSSRExtrasMap(Cam3D c)
        {
            SetMats(c);
            Bind();
            PreRender();
            RenderSSRExtrasMap();
            PostRender();
            Release();
        }

        public void PresentPositionMap(Cam3D c)
        {
            SetMats(c);
            Bind();
            PreRender();
            RenderPositionMap();
            PostRender();
            Release();
        }

        public void PresentNormalMap(Cam3D c)
        {
            SetMats(c);
            Bind();
            PreRender();
            RenderNormalMap();
            PostRender();
            Release();
        }

        public override void PresentDepth(Cam3D c)
        {
            SetMats(c);
            Bind();
            PreRender();
            RenderDepth();
            PostRender();
            Release();
            foreach (Node3D s in Sub)
            {
                //     s.PresentDepth ( c );
            }
        }

        public override void Read()
        {
            LocalTurn = Help.IOHelp.ReadMatrix();
            LocalPos = Help.IOHelp.ReadVec3();
            LocalScale = Help.IOHelp.ReadVec3();
            Name = Help.IOHelp.ReadString();

            AlwaysAlpha = Help.IOHelp.ReadBool();
            On = Help.IOHelp.ReadBool();

            int mc = Help.IOHelp.ReadInt();
            for (int m = 0; m < mc; m++)
            {
                Mesh3D msh = new Mesh3D();
                msh.Read();
                //Meshes.Add ( msh );
                AddMesh(msh);
            }
            ReadScripts();
            SetMultiPass();
        }

        public Entity3D Clone()
        {

            var new_ent = new Entity3D();
            new_ent.Meshes = Meshes;
            new_ent.Renderer = Renderer;
            return new_ent;

        }

        public void MakeQuad(float size)
        {

            Mesh3D m = new Mesh3D(12, 4);
            m.SetVertex(0, new Vector3(-size, 0, -size), Vector3.Zero, Vector3.Zero, Vector3.Zero, new Vector2(0, 0));
            m.SetVertex(1, new Vector3(size, 0, -size), Vector3.Zero, Vector3.Zero, Vector3.Zero, new Vector2(1, 0));
            m.SetVertex(2, new Vector3(size, 0, size), Vector3.Zero, Vector3.Zero, Vector3.Zero, new Vector2(1, 1));
            m.SetVertex(3, new Vector3(-size, 0, size), Vector3.Zero, Vector3.Zero, Vector3.Zero, new Vector2(0, 1));

            m.SetTri(0, 0, 1, 2);
            m.SetTri(1, 2, 3, 0);
            m.SetTri(2, 0, 2, 1);
            m.SetTri(3, 2, 0, 3);

            m.Material = new Material3D();

            m.Final();

            AddMesh(m);

            Renderer = new VRNoFx();

        }

        public virtual void Release()
        {
        }

        public virtual void Render()
        {
            Effect.FXG.Ent = this;
            foreach (Mesh3D m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.Render(m);
            }
        }

        public Vivid.Pick.PickResult CamPick(int x, int y)
        {
            Pick.PickResult res = new Pick.PickResult();

            Pick.Ray mr = Pick.Picker.CamRay(SceneGraph3D.CurScene.Cams[0], x, y);

            float cd = 0;
            bool firstHit = true;
            Mesh3D hitMesh = null;
            float cu = 0, cv = 0;

            foreach (var msh in Meshes)
            {
                for (int i = 0; i < msh.TriData.Length; i++)
                {
                    var td = msh.TriData[i];

                    var r0 = msh.VertexData[td.V0].Pos;
                    var r1 = msh.VertexData[td.V1].Pos;
                    var r2 = msh.VertexData[td.v2].Pos;

                    r0 = Vector3.TransformPosition(r0, World);
                    r1 = Vector3.TransformPosition(r1, World);
                    r2 = Vector3.TransformPosition(r2, World);

                    Vector3? pr = Pick.Picker.GetTimeAndUvCoord(mr.pos, mr.dir, r0, r1, r2);
                    if (pr == null)
                    {

                    }
                    else
                    {
                        Vector3 cr = (Vector3)pr;
                        if (cr.X < cd || firstHit)
                        {
                            firstHit = false;
                            cd = cr.X;
                            hitMesh = msh;

                            cu = cr.Y;
                            cv = cr.Z;
                        }
                    }


                }
            }

            if (firstHit)
            {
                return null;
            }

            res.Dist = cd;
            res.Node = this;
            res.Pos = Pick.Picker.GetTrilinearCoordinateOfTheHit(cd, mr.pos, mr.dir);
            res.Ray = mr;
            res.UV = new Vector3(cu, cv, 0);
            res.Mesh = hitMesh;

            return res;

        }


        public virtual void RenderSSRExtrasMap()
        {
            Effect.FXG.Ent = this;
            foreach (Mesh3D m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.RenderSSRExtrasMap(m);
            }
        }

        public virtual void RenderPositionMap()
        {
            Effect.FXG.Ent = this;
            foreach (Mesh3D m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.RenderPositionMap(m);
            }
        }

        public virtual void RenderNormalMap()
        {
            Effect.FXG.Ent = this;
            foreach (Mesh3D m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.RenderNormalMap(m);
            }
        }

        public virtual void RenderDepth()
        {
            Effect.FXG.Ent = this;
            foreach (Mesh3D m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.RenderDepth(m);
            }
        }

        public void ScaleMeshes(float x, float y, float z)
        {
            DScale(x, y, z, this);
        }

        public void SetMat(Material3D mat)
        {
            foreach (Mesh3D m in Meshes)
            {
                m.Material = mat;
            }
            foreach (Node3D n in Sub)
            {
                if (n is Entity3D || n is Terrain.Terrain3D)
                {
                    ;
                }

                {
                    Entity3D ge = n as Entity3D;
                    ge.SetMat(mat);
                }
            }
        }

        public void SetMats(Cam3D c)
        {
            int w = App.AppInfo.RW;
            int h = App.AppInfo.RH;
            int aw = App.AppInfo.W;
            int ah = App.AppInfo.H;
            Effect.FXG.Proj = c.ProjMat;
            Effect.FXG.Cam = c;
            // GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 mm = Matrix4.Identity;
            // mm = c.CamWorld;
            //mm = mm * Matrix4.Invert(Matrix4.CreateTranslation(c.WorldPos));

            mm = World;
            //var wp = LocalPos;
            //mm = mm*Matrix4.CreateTranslation(wp);
            //GL.LoadMatrix(ref mm);
            Effect.FXG.Local = mm;
            Effect.FXG.PrevLocal = PrevWorld;
        }

        public override void Write()
        {
            Help.IOHelp.WriteMatrix(LocalTurn);
            Help.IOHelp.WriteVec(LocalPos);
            Help.IOHelp.WriteVec(LocalScale);
            Help.IOHelp.WriteString(Name);


            Help.IOHelp.WriteBool(AlwaysAlpha);
            Help.IOHelp.WriteBool(On);


            int mc = Meshes.Count;
            Help.IOHelp.WriteInt(mc);
            foreach (Mesh3D msh in Meshes)
            {
                msh.Write();
            }
            WriteScripts();
        }

        public int SubSaveCount()
        {
            int num = 0;
            foreach (Node3D node in Sub)
            {
                if (!node.NoSave)
                {
                    num++;
                }
            }
            return num;
        }

        private void DScale(float x, float y, float z, Entity3D node)
        {
            foreach (Mesh3D m in node.Meshes)
            {
                m.Scale(x, y, z);
            }
            foreach (Node3D snode in node.Sub)
            {
                DScale(x, y, z, snode as Entity3D);
            }
        }
    }
}