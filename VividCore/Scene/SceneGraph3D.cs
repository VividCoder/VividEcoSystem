using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Vivid.Reflect;
using Vivid.Scene.Node;

namespace Vivid.Scene
{
    public class SceneGraph3D
    {
        public Cam3D CamOverride = null;

        // public List<GraphNode3D> Nodes = new List<GraphNode3D>();
        public List<Cam3D> Cams = new List<Cam3D>();

        public List<Light3D> Lights = new List<Light3D>();

        public Node3D Root = new Entity3D();

        public SceneGraph3D SubGraph = null;

        public List<Vector3> Verts;
        public List<int> Tris;
        public bool SceneCached = false;

        public Vivid.Resonance.UI UI = null;

        public static SceneGraph3D Graph = null;


        public bool RenderingShadows
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            set;
        }

        public bool Paused
        {
            get;
            set;
        }

        public ClassIO ClassCopy
        {
            get;
            set;
        }

        public void CheckAssets()
        {
            CheckAssets(Root);
        }

        public void CheckAssets(Node3D node)
        {
        Recheck:
            foreach (var scr in node.NodeScripts)
            {
                if (scr.Updated)
                {
                    System.Console.WriteLine("Updated:" + scr.FullPath);
                    node.NodeScripts.Remove(scr);

                    Vivid.Scripting.NodeScript ns = Vivid.Scripting.NodeScriptCompiler.Compile(scr.FullPath);
                    ns.Node = node;

                    //  scr.Transfer(ns);

                    node.NodeScripts.Add(ns);

                    goto Recheck;
                }
            }

            foreach (var sub_n in node.Sub)
            {
                CheckAssets(sub_n);
            }

        }

        public void InitUIScripts(Vivid.Resonance.UI ui)
        {
            InitUINodeScripts(Root, ui);
        }

        public void InitUINodeScripts(Node3D node, Vivid.Resonance.UI ui)
        {
            node.InitUIScripts(ui);
            foreach (var s_node in node.Sub)
            {
                InitUINodeScripts(s_node, ui);
            }
        }

        public void InitScripts()
        {

            InitNodeScripts(Root);

        }

        public static SceneGraph3D CurScene = null;

        public static void AddToScene(Node3D node)
        {
            CurScene.Root.Add(node);
            node.Top = CurScene.Root;
        }

        public static void LoadScene(string path)
        {
            Vivid.Audio.Songs.StopSong();

            CurScene.Root = new Node3D();
            CurScene.LoadGraph(path);
            CurScene.UI.Root = new Vivid.Resonance.UIForm().Set(0, 0, App.AppInfo.W, App.AppInfo.H);
            CurScene.InitScripts();
            CurScene.InitUIScripts(CurScene.UI);

        }

        public void InitNodeScripts(Node3D node)
        {

            node.InitScripts();
            //foreach(var s_node in node.Sub)
            //{
            for (int nn = 0; nn < node.Sub.Count; nn++)
            {
                var s_node = node.Sub[nn];
                InitNodeScripts(s_node);
            }

        }

        public void UpdateScripts()
        {

            UpdateNodeScripts(Root);

        }

        public void UpdateNodeScripts(Node3D node)
        {

            node.UpdateScripts();
            foreach (var s_node in node.Sub)
            {
                UpdateNodeScripts(s_node);
            }

        }

        public void DrawScripts()
        {

            DrawNodeScripts(Root);

        }

        public void DrawNodeScripts(Node3D node)
        {

            node.DrawScripts();
            foreach (var s_node in node.Sub)
            {
                DrawNodeScripts(s_node);
            }

        }

        public virtual void Add(Cam3D c)
        {
            Cams.Add(c);
        }

        public virtual void Add(Light3D l)
        {
            Lights.Add(l);
        }

        public virtual void Add(Node3D n)
        {
            Root.Add(n);
            n.Top = Root;
            SceneCached = false;
            SceneChanged = true;
            n.Graph = this;
        }

        public virtual void Rebuild()
        {
            SceneCached = false;
            SceneChanged = true;
        }

        public void Begin()
        {
            if (Running) return;
            if (Paused)
            {
                Pause();
                return;
            }
            Root.Begin();
            Running = true;
        }

        public void BeginFrame()

        {
            BeginFrameNode(Root);
            foreach (Cam3D c in Cams)
            {
                c.StartFrame();
            }
        }

        public SceneGraph3D()
        {
            InitThreads();
            Running = false;
            Paused = false;
            RenderingShadows = false;
            Root = new Node3D()
            {
                Name = "Root"
            };
            CurScene = this;
        }

        public void Remove(Node3D node)
        {

            node.Top.Sub.Remove(node);
            node.Top = null;


            //RemoveNode(Root, node);


        }

        public Node3D FindNode(string name)
        {
            if (Root == null) return null;
            return FindNode(Root, name);

        }

        public Node3D FindNode(Node3D s, string name)
        {

            if (s.Name == name)
            {
                return s;
            }
            foreach (var sub_n in s.Sub)
            {
                var fn = FindNode(sub_n, name);
                if (fn != null)
                {
                    return fn;
                }
            }
            return null;

        }

        public void RemoveNode(Node3D s, Node3D r)
        {

            if (s == r)
            {



            }

        }

        public void BeginFrameNode(Node3D node)
        {
            node.StartFrame();
            foreach (Node3D snode in node.Sub)
            {
                BeginFrameNode(snode);
            }
        }

        public void BeginRun()
        {
        }

        public virtual void Bind()
        {
        }

        public List<Vector3> Vert = new List<Vector3>();

        public int MaxThreads = 64;
        public Thread[] SceneTransforms = null;
        public Thread SceneCheck = null;

        public bool SceneChanged
        {
            get
            {
                return _Changed;
            }
            set
            {
                _Changed = value;
                if (_Changed == true)
                {
                    Edited?.Invoke();
                }
            }
        }

        public delegate void SceneEdited();

        public SceneEdited Edited = null;
        private bool _Changed = false;
        public Vector3[] SceneCache = null;
        public Entity3D[] SceneNodes = null;
        public SceneIntristics[] SceneInfos = null;
        public Dictionary<int, Entity3D> SceneLut = new Dictionary<int, Entity3D>();

        public class SceneIntristics
        {
            public List<Entity3D> Ents = new List<Entity3D>();
            public bool Done = false;
            public int Begin = 0, End = 0;
            public Thread Thr = null;
            public int ThreadID = 0;
        }

        public bool SceneUpdating = false;

        public void InitThreads()

        {
            void CheckThread()
            {
                if (SceneTransforms == null)
                {
                    SceneTransforms = new Thread[MaxThreads];
                }
                while (true)
                {
                    if (SceneUpdating)
                    {
                        int dc = 0;

                        foreach (var th in SceneInfos)
                        {
                            if (th.Done) dc++;
                        }
                        if (dc == SceneInfos.Length)
                        {
                            SceneUpdating = false;
                        }
                    }
                    if (SceneChanged && SceneUpdating == false)
                    {
                        SceneUpdating = true;
                        SceneChanged = false;
                        foreach (var thr in SceneTransforms)
                        {
                            if (thr == null) continue;
                            if (thr.ThreadState == ThreadState.Running)
                            {
                                thr.Abort();
                            }
                        }

                        List<Node3D> node_list = this.GetList(true);
                        int vc = 0;
                        int bc = 0;
                        foreach (var n in node_list)
                        {
                            var ent = n as Entity3D;

                            foreach (var m in ent.Meshes)
                            {
                                vc = vc + m.TriData.Length * 3;
                                bc = bc + m.TriData.Length * 3;
                            }
                        }
                        SceneCache = new Vector3[vc];
                        SceneNodes = new Entity3D[vc];

                        SceneInfos = new SceneIntristics[MaxThreads];

                        int pnc = node_list.Count / MaxThreads;

                        int cnc = 0;
                        int tid = 0;
                        for (int i = 0; i < MaxThreads; i++)
                        {
                            SceneInfos[i] = new SceneIntristics();
                            SceneInfos[i].Done = true;
                        }
                        bool first = true;
                        int vv = 0;
                        foreach (var nl in node_list)
                        {
                            if (first)
                            {
                                first = false;
                                SceneInfos[tid].Begin = vv;
                                SceneInfos[tid].Done = false;
                            }
                            SceneInfos[tid].Ents.Add(nl as Entity3D);
                            Entity3D me = nl as Entity3D;
                            foreach (var msh in me.Meshes)
                            {
                                vv = vv + msh.TriData.Length * 3;
                            }

                            cnc++;
                            if (cnc >= pnc)
                            {
                                SceneInfos[tid].End = vv;
                                cnc = 0;
                                tid++;
                                first = true;
                            }
                        }
                        SceneLut.Clear();
                        tid = 0;
                        foreach (var info in SceneInfos)
                        {
                            if (info.Ents.Count > 0)
                            {
                                void procScene(object obj)
                                {
                                    SceneIntristics si = obj as SceneIntristics;
                                    int id = si.ThreadID;
                                    int sv = si.Begin;
                                    System.Console.WriteLine("Begin:" + si.Begin + " End:" + si.End);
                                    foreach (var node in si.Ents)
                                    {
                                        System.Console.WriteLine("Node:" + node.Name);
                                        Matrix4 n_m = node.World;
                                        foreach (var msh in node.Meshes)
                                        {
                                            System.Console.WriteLine("Mesh:" + msh.TriData.Length);

                                            for (int i = 0; i < msh.TriData.Length; i++)
                                            {
                                                int v0 = msh.TriData[i].V0;
                                                int v1 = msh.TriData[i].V1;
                                                int v2 = msh.TriData[i].v2;
                                                Vector3 r0, r1, r2;
                                                r0 = Rot(msh.VertexData[v0].Pos, node);
                                                r1 = Rot(msh.VertexData[v1].Pos, node);
                                                r2 = Rot(msh.VertexData[v2].Pos, node);
                                                SceneNodes[sv] = node;
                                                SceneNodes[sv + 1] = node;
                                                SceneNodes[sv + 2] = node;
                                                SceneCache[sv++] = r0;
                                                SceneCache[sv++] = r1;
                                                SceneCache[sv++] = r2;
                                            }
                                        }
                                    }
                                    si.Done = true;
                                    System.Console.WriteLine("SceneThread:" + id + " complete.");
                                }

                                info.ThreadID = tid;
                                info.Thr = new Thread(new ParameterizedThreadStart(procScene));
                                info.Thr.Start(info);
                            }
                            tid++;
                        }

                        System.Console.WriteLine("Checked:" + node_list.Count + " nodes");
                        //SceneChanged = false;
                    }
                    Thread.Sleep(5);
                }
            }

            SceneCheck = new Thread(new ThreadStart(CheckThread));
            SceneCheck.Start();
        }

        public Vivid.Pick.PickResult CamPick(int x, int y)
        {
            if (SceneUpdating == true) return null;

            System.Console.WriteLine("Checking Scene: CacheSize:" + SceneCache.Length);

            Pick.PickResult res = new Pick.PickResult();

            Pick.Ray mr = Pick.Picker.CamRay(Cams[0], x, y);

            float cd = 0;
            bool firstHit = true;
            float cu = 0, cv = 0;
            Node3D cn = null;

            int ms = System.Environment.TickCount;

            for (int i = 0; i < SceneCache.Length; i += 3)
            {
                var r0 = SceneCache[i];
                var r1 = SceneCache[i + 1];
                var r2 = SceneCache[i + 2];

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
                        cn = SceneNodes[i];
                        cu = cr.Y;
                        cv = cr.Z;
                    }
                }
            }

            ms = System.Environment.TickCount - ms;
            System.Console.WriteLine("PickTime:" + ms + " Secs:" + (ms / 1000));

            if (firstHit)
            {
                return null;
            }

            res.Dist = cd;
            res.Node = cn;
            res.Pos = Pick.Picker.GetTrilinearCoordinateOfTheHit(cd, mr.pos, mr.dir);
            res.Ray = mr;
            res.UV = new Vector3(cu, cv, 0);

            return res;
        }

        public virtual void Clean()
        {
        }
        public virtual void Clear()
        {

            Root = new Node3D();
            Root.Name = "Root";
            Lights.Clear();
            Cams.Clear();

        }
        public void Record()
        {
            Copy();
        }

        public void Copy()
        {
            ClassCopy = new Reflect.ClassIO(this);
            ClassCopy.Copy();
            CopyNode(Root);
        }

        public void CopyNode(Node3D node)
        {
            node.CopyProps();
            foreach (Node3D nn in node.Sub)
            {
                CopyNode(nn);
            }
        }

        public void End()
        {
            if (!Running || !Paused) return;
            Root.End();
            Running = false;
            Paused = false;
        }

        public void Pause()
        {
            if (Paused)
            {
                Root.Resume();
                Paused = false;
                Running = true;
                return;
            }
            if (!Running)
            {
                return;
            }
            Root.Pause();
            Running = false;
            Paused = true;
        }

        public void EndRun()
        {
        }

        public List<Node3D> GetList(bool meshesOnly)
        {
            List<Node3D> list = new List<Node3D>();
            NodeToList(Root, meshesOnly, list);
            return list;
        }

        public void LoadGraph(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            Help.IOHelp.r = r;
            int cc = r.ReadInt32();
            for (int i = 0; i < cc; i++)
            {
                Cam3D nc = new Cam3D();
                nc.Read();
                Cams.Add(nc);
            }
            int lc = r.ReadInt32();
            for (int i = 0; i < lc; i++)
            {
                Light3D nl = new Light3D();
                nl.Read();
                Lights.Add(nl);
            }
            //Entity3D re = new Entity3D();
            Node3D new_e = null;

            Root = ReadNode();

            //re.Read();
            fs.Close();
            Root.SetMultiPass();
        }

        public Node3D ReadNode()
        {

            int node_t = Help.IOHelp.ReadInt();
            Node3D nn = null;
            if (node_t == 1)
            {
                Entity3D new_e = new Entity3D();
                new_e.Read();
                nn = new_e;
            }
            else if (node_t == 0)
            {
                Node3D new_n = new Node3D();
                new_n.Read();
                nn = new_n;
            }
            else
            {

                System.Environment.Exit(-3);

            }
            int sc = Help.IOHelp.ReadInt();
            for (int i = 0; i < sc; i++)
            {
                var rn = ReadNode();
                rn.Top = nn;
                nn.Sub.Add(rn);

            }
            return nn;

        }

        public void PauseRun()
        {
        }

        public virtual void Release()
        {
        }

        public virtual void RenderByTags(List<string> tags)
        {
            foreach (Node3D n in Root.Sub)
            {
                RenderNodeByTags(tags, n);
            }
        }

        public void DrawFormSolid(Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            Vivid.Draw.Pen2D.Rect(ViewX + x, ViewY + y, w, h, col);
        }

        public void DrawForm(Texture.Texture2D tex, Vector4 col, int x = 0, int y = 0, int w = -1, int h = -1)
        {
            Draw.Pen2D.BlendMod = Draw.PenBlend.Alpha;

            Draw.Pen2D.Rect(x, y, w, h, tex, col);
        }

        public int ViewX, ViewY, ViewW, ViewH;

        public static Cam3D LastCam = null;

        public virtual void RenderEntity(Entity3D ent)
        {
            LastCam = Cams[0];

            SubGraph?.Render();
            List<string> defTags = new List<string>
            {
                "All"
            };
            RenderNodeByTags(defTags, ent);


        }
        public virtual void Render()
        {
            LastCam = Cams[0];

            SubGraph?.Render();
            List<string> defTags = new List<string>
            {
                "All"
            };
            RenderNodeByTags(defTags, Root);


        }

        public void SetLightmapTex(Texture.Texture2D tex)
        {
            Root.SetLightmapTex(tex);
        }

        public void EditGraph(EditNode editor)
        {
            Root.Edit(editor);
        }

        public virtual void RenderNodeByTags(List<string> tags, Node3D node)
        {
            bool rt = false;
            foreach (string tag in tags)
            {
                if (node.RenderTags.Contains(tag))
                {
                    rt = true;
                    RenderThis(node);
                    break;
                }
            }
            if (rt)
            {
                foreach (Node3D n in node.Sub)
                {
                    RenderNodeByTags(tags, n);
                }
            }
        }

        public virtual void RenderSSRExtras()
        {
            GL.ClearColor(new OpenTK.Graphics.Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (CamOverride != null)
            {
                //foreach (var n in Nodes)
                //{
                RenderNodeSSRExtrasMap(Root, CamOverride);

                //}
            }
            else

            {
                RenderNodeSSRExtrasMap(Root, Cams[0]);
            }
        }

        public virtual void RenderPositionMap()
        {
            GL.ClearColor(new OpenTK.Graphics.Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (CamOverride != null)
            {
                //foreach (var n in Nodes)
                //{
                RenderNodePositionMap(Root, CamOverride);

                //}
            }
            else

            {
                RenderNodePositionMap(Root, Cams[0]);
            }
        }

        public virtual void RenderNormalMap()
        {
            GL.ClearColor(new OpenTK.Graphics.Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (CamOverride != null)
            {
                //foreach (var n in Nodes)
                //{
                RenderNodeNormalMap(Root, CamOverride);

                //}
            }
            else

            {
                RenderNodeNormalMap(Root, Cams[0]);
            }
        }

        public virtual void RenderDepth()
        {
            GL.ClearColor(new OpenTK.Graphics.Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (CamOverride != null)
            {
                //foreach (var n in Nodes)
                //{
                RenderNodeDepth(Root, CamOverride);

                //}
            }
            else

            {
                RenderNodeDepth(Root, Cams[0]);
            }
            //foreach (var c in Cams)
        }

        public virtual void RenderNode(Node3D node)
        {
            // Console.WriteLine("RenderNode:" + node.Name);
            RenderThis(node);
            foreach (Node3D snode in node.Sub)
            {
                // Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNode(snode);
            }
        }

        private void RenderThis(Node3D node)
        {
            if (node.Name == "Terrain")
            {
            }
            if (CamOverride != null)
            {
                foreach (Light3D l in Lights)
                {
                    Light3D.Active = l;

                    node.Present(CamOverride);
                }
            }
            else
            {
                foreach (Cam3D c in Cams)
                {
                    if (node.AlwaysAlpha)
                    {
                        Entity3D ge = node as Entity3D;
                        if (ge.Renderer is Visuals.RMultiPass)
                        {
                            ge.Renderer = new Visuals.VRNoFx();
                        }
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                        node.Present(c);
                        continue;
                    }
                    if (node.Lit)
                    {
                        bool second = true;
                        bool first = true;

                        foreach (Light3D l in Lights)
                        {
                            Light3D.Active = l;

                            if (first)
                            {
                                first = false;
                                GL.Disable(EnableCap.Blend);
                            }
                            else if (second)
                            {
                                second = false;
                                GL.Enable(EnableCap.Blend);
                                GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                            }

                            node.Present(c);

                            // foreach (var n in Nodes) { n.Present(c); }
                        }
                        GL.Disable(EnableCap.Blend);
                    }
                    else
                    {
                        if (node.FaceCamera)
                        {
                            node.LookAt(c.LocalPos, new Vector3(0, 1, 0));
                        }
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactor.Src1Alpha, BlendingFactor.OneMinusSrcAlpha);
                        GL.DepthMask(false);
                        node.Present(c);
                        GL.DepthMask(true);
                    }
                }
            }
        }

        public void RenderNodeSSRExtrasMap(Node3D node, Cam3D c)
        {
            var e3 = node as Entity3D;
            e3.PresentSSRExtrasMap(c);
            foreach (var node2 in node.Sub)
            {
                RenderNodeSSRExtrasMap(node2, c);
            }
        }

        public void RenderNodePositionMap(Node3D node, Cam3D c)
        {
            var e3 = node as Entity3D;

            e3.PresentPositionMap(c);

            foreach (var snode in node.Sub)
            {
                RenderNodePositionMap(snode, c);
            }
        }

        public void RenderNodeNormalMap(Node3D node, Cam3D c)
        {
            var e3 = node as Entity3D;

            e3.PresentNormalMap(c);

            foreach (var snode in node.Sub)
            {
                RenderNodeNormalMap(snode, c);
            }
        }

        public virtual void RenderNodeDepth(Node3D node, Cam3D c)
        {
            var e3 = node as Entity3D;

            if (node.CastDepth)
            {
                node.PresentDepth(c);
            }
            foreach (Node3D snode in node.Sub)
            {
                RenderNodeDepth(snode, c);
            }
        }

        public virtual void RenderNodeNoLights(Node3D node)
        {
            if (CamOverride != null)
            {
                foreach (Light3D l in Lights)
                {
                    Light3D.Active = l;

                    node.Present(CamOverride);
                }
            }
            else
            {
                foreach (Cam3D c in Cams)
                {
                    GL.Disable(EnableCap.Blend);

                    // Console.WriteLine("Presenting:" + node.Name);
                    node.Present(c);

                    // foreach (var n in Nodes) { n.Present(c); }
                }
            }
            foreach (Node3D snode in node.Sub)
            {
                // Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNodeNoLights(snode);
            }
        }

        public virtual void RenderNoLights()
        {
            Light3D.Active = null;
            RenderNodeNoLights(Root);
        }

        public virtual void RenderShadows()
        {
            int ls = 0;
            GL.Disable(EnableCap.Blend);
            foreach (Light3D l in Lights)
            {
                ls++;
                l.DrawShadowMap(this);
                // Console.WriteLine("LightShadows:" + ls);
            }
        }

        public void Restore()
        {
            ClassCopy.Reset();
            RestoreNode(Root);
        }

        public void RestoreNode(Node3D node)
        {
            node.RestoreProps();
            foreach (Node3D nn in node.Sub)
            {
                RestoreNode(nn);
            }
        }

        public Vector3 Rot(Vector3 p, Node3D n)
        {
            return Vector3.TransformPosition(p, n.World);
        }

        public void SaveGraph(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            Help.IOHelp.w = bw;

            bw.Write(Cams.Count);
            foreach (Cam3D c in Cams)
            {
                c.Write();
            }
            bw.Write(Lights.Count);
            foreach (Light3D c in Lights)
            {
                c.Write();
            }
            Node3D r = Root as Node3D;
            SaveNode(r);
            bw.Flush();
            fs.Flush();
            fs.Close();
        }

        public void SaveNode(Node3D node)
        {

            if (node is Entity3D)
            {
                Help.IOHelp.WriteInt(1);
                node.Write();

            }
            else if (node is Node3D)
            {
                Help.IOHelp.WriteInt(0);
                node.Write();
            }
            else
            {

                System.Environment.Exit(-2);

            }
            Help.IOHelp.WriteInt(node.Sub.Count);
            foreach (var sub in node.Sub)
            {
                SaveNode(sub);
            }
        }

        public virtual void Update()
        {
            //var tp = new XInput.XPad(0);
            if (Root == null) return;
            UpdateNode(Root);
        }

        public virtual void UpdateNode(Node3D node)
        {
            node.Update();
        }

        private void NodeToList(Node3D node, bool meshes, List<Node3D> list)
        {
            if (meshes)
            {
                if (node is Entity3D)
                {
                    list.Add(node);
                }
            }
            else
            {
                list.Add(node);
            }
            foreach (Node3D n2 in node.Sub)
            {
                NodeToList(n2, meshes, list);
            }
        }
    }
}