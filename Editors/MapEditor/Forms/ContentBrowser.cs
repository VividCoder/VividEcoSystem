using Vivid.Resonance;
using Vivid.Resonance.Forms;
using Vivid.Texture;

using System.Collections.Generic;
using System.IO;

namespace MapEditor.Forms
{
    public delegate void ContentLoaded(string path);

    public class ContentBrowserForm : WindowForm
    {
        public ContentArea Area = null;
        public ContentLoaded LoadedAsset;
        public Vivid.Scene.SceneGraph3D Graph;
        public List<string> PP = new List<string>();
        public List<string> FilterOut = new List<string>();

        public static string CurrentPath = "";
        public OpenTK.Vector3 ImScale = new OpenTK.Vector3(1f, 1f, 1f);

        public ContentBrowserForm()
        {
            FilterOut.Add(".texdat");
            FilterOut.Add(".texdatsmall");

            Resized = () =>
            {
                body.ViewX = GX;
                body.ViewY = GY;
                body.ViewW = W;
                body.ViewH = H;
                title.ViewX = GX;
                title.ViewY = GY;
                title.ViewW = W;
                title.ViewH = H;
            };

            Area = new ContentArea();
            Area.Root = this;
            AfterSet = () =>
            {
                Area.Set(0, 0, body.W, body.H);

            };

            body.Add(Area);
            Set(20, 400, 500, 250, "Content");
        }

        public void LoadAsset(string file)
        {
            var node = Vivid.Import.Import.ImportNode(file);
            if (node == null) return;
            node.SetMultiPass();
            Graph.Add(node);
            node.LocalScale = ImScale;
            LoadedAsset?.Invoke(file);
            InspectorForm.Main.SetObj(node);
        }
        public string prev = "";

        int pl = 0;

        public void SetPath(string file)
        {
            CurrentPath = file;
            Area.Contents.Clear();
            Area.Folders.Clear();
            int x = 5;
            int y = 5;


            if (new DirectoryInfo(file).Parent != null)
            {
                prev = new DirectoryInfo(file).Parent.FullName;

                var newe = new ContentEntry();
                newe.IsFolder = true;
                newe.Name = "Parent";
                newe.FullPath = prev;
                Area.Folders.Add(newe);
                Area.Contents.Add(newe);
                newe.X = x;
                newe.Y = y;
                // prev = file;
                x = x + 120;
            }




            PP.Add(file);
            foreach (var dir in new DirectoryInfo(file).GetDirectories())
            {
                bool skip = false;
                foreach (var fo in FilterOut)
                {
                    if (dir.FullName.ToLower().Contains(fo.ToLower()))
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;

                string id = dir.FullName;

                ContentEntry ce = null;

                if (Area.Cache.ContainsKey(id))
                {
                    ce = Area.Cache[id];
                }
                else
                {
                    ce = new ContentEntry();
                    ce.Name = dir.Name;
                    ce.FullPath = dir.FullName;
                    Area.Cache.Add(dir.FullName, ce);

                    ce.IsFolder = true;
                    ce.X = x;
                    ce.Y = y;
                    x = x + 120;
                    if (x > Area.W - 64)
                    {
                        x = 5;
                        y = y + 00;
                    }
                }
                Area.Folders.Add(ce);
                Area.Contents.Add(ce);
            }
            Area.Files.Clear();
            foreach (var fil in new DirectoryInfo(file).GetFiles())
            {
                bool skip = false;
                foreach (var fo in FilterOut)
                {
                    if (fil.FullName.ToLower().Contains(fo.ToLower()))
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;
                var ce = new ContentEntry();

                if (Area.Cache.ContainsKey(fil.FullName))
                {
                    ce = Area.Cache[fil.FullName];
                }
                else
                {
                    Area.Cache.Add(fil.FullName, ce);

                    ce.Name = fil.Name;
                    ce.FullPath = fil.FullName;
                    ce.IsFolder = false;
                    ce.X = x;
                    ce.Y = y;
                    x = x + 120;
                    if (x > Area.W - 80)
                    {
                        x = 5;
                        y = y + 90;
                    }
                }
                Area.Files.Add(ce);
                Area.Contents.Add(ce);

            }
            var lo = Area.Contents[Area.Contents.Count - 1];
            Area.Scroller.SetMax(lo.Y + 55);
        }
    }

    public class ContentEntry
    {
        public int X = 0, Y = 0;
        public string Name;
        public string FullPath;
        public bool IsFolder = false;
        public dynamic Load()
        {
            var ext = new FileInfo(FullPath).Extension.ToLower();
            System.Console.WriteLine("Loading Content:" + ext + "!");
            switch (ext)
            {
                case ".cs":
                    return Vivid.Scripting.NodeScriptCompiler.Compile(FullPath);
                    break;
                case ".png":
                case ".jpg":
                case ".bmp":
                case ".gif":
                    return new Texture2D(FullPath, LoadMethod.Single, true);
                    break;
            }
            return null;
        }
    }

    public class ContentArea : UIForm
    {
        public Dictionary<string, ContentEntry> Cache = new Dictionary<string, ContentEntry>();
        public ContentBrowserForm Root;
        public static Texture2D FileTex;
        public static Texture2D FolderTex;
        public static Texture2D ScriptTex;
        public static Texture2D MusicTex;
        public static Texture2D ImgTex;
        public List<ContentEntry> Folders = new List<ContentEntry>();
        public List<ContentEntry> Files = new List<ContentEntry>();
        public List<ContentEntry> Contents = new List<ContentEntry>();
        public ScrollBarV Scroller = null;
        public ContentEntry Over = null;
        public ContentEntry DragEntry = null;
        public int YDif = 0;
        public ContentArea()
        {
            AfterSet = () =>
            {
                ViewX = GX;

                ViewY = GY;
                ViewW = W;
                ViewH = H;
                Scroller.Set(W - 10, 0, 10, H);
                if (Contents.Count > 0)
                {
                    var lo = Contents[Contents.Count - 1];
                    Scroller.SetMax(lo.Y + 55);
                }
            };

            Resized = () =>
            {

            };

            Scroller = new ScrollBarV();
            Add(Scroller);
            Scroller.ValueChange = (v) =>
            {
                YDif = -(int)v;
            };

            if (FileTex == null)
            {
                FileTex = new Texture2D("data/ui/file1.png", LoadMethod.Single, true);
                FolderTex = new Texture2D("data/nxUI/content/folder.png", LoadMethod.Single, true);
                ScriptTex = new Texture2D("data/nxUI/content/script.png", LoadMethod.Single, true);
                MusicTex = new Texture2D("data/nxUI/content/music.png", LoadMethod.Single, true);
                ImgTex = new Texture2D("data/nxUI/content/texture.png", LoadMethod.Single, true);
            }
            Draw = () =>
            {
                DrawFormSolid(new OpenTK.Vector4(0.2f, 0.2f, 0.2f, 0.9f));

                foreach (var ce in Contents)
                {
                    if (ce == Over)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.7f, 0.7f, 0.7f, 0.7f), ce.X, ce.Y + YDif, 64, 64);
                    }
                    if (ce.IsFolder)
                    {
                        DrawForm(FolderTex, ce.X + 5, ce.Y + 5 + YDif, 54, 54);
                    }
                    else
                    {
                        DrawForm(FileTex, ce.X, ce.Y + YDif, 64, 64);
                        var ext = new FileInfo(ce.FullPath).Extension;
                        switch (ext)
                        {
                            case ".cs":
                                DrawForm(ScriptTex, ce.X, ce.Y + 24 + YDif, 64, 44);
                                break;
                            case ".jpg":
                            case ".png":
                            case ".bmp":
                                DrawForm(ImgTex, ce.X, ce.Y + 24 + YDif, 64, 44);
                                break;
                            case ".mp3":
                            case ".wav":
                            case ".ogg":
                                DrawForm(MusicTex, ce.X, ce.Y + 24 + YDif, 64, 44);
                                break;
                        }
                    }
                    if (ce == Over)
                    {
                        DrawFormSolid(new OpenTK.Vector4(0.9f, 0.9f, 0.9f, 0.8f), ce.X - 5, ce.Y + 58 + YDif, 120, 25);

                        DrawText(ce.Name, ce.X, ce.Y + 58 + YDif, new OpenTK.Vector4(0.1f, 0.1f, 0.1f, 0.8f));
                    }
                    else
                    {
                        DrawText(ce.Name, ce.X, ce.Y + 58 + YDif, new OpenTK.Vector4(1, 1, 1, 0.8f));

                    }
                }
            };
            MouseLeave = () =>
            {
                Over = null;
            };
            MouseMove = (x, y, mx, my) =>
            {
                Over = null;
                foreach (var ce in Contents)
                {
                    if (x > ce.X && y > (ce.Y + YDif) && x < ce.X + 64 && y < ce.Y + 64)
                    {
                        Over = ce;
                    }
                }
                if (DragEntry != null)
                {
                    DragEntry.X += mx;
                    DragEntry.Y += my;

                    if (DragEntry.X < 12) DragEntry.X = 12;
                    if (DragEntry.Y < 12) DragEntry.Y = 12;
                }
            };
            MouseUp = (b) =>
            {
                if (b == 0)
                {
                    DragEntry = null;
                    if (UI.CurUI.DragObj == null) return;

                    UI.CurUI.CompleteDrag();

                    UI.CurUI.DragObj = null;
                }
            };
            MouseDown = (b) =>
            {
                if (b == 0)
                {
                    var ce = GetContent();
                    //  DragEntry = ce;
                    if (ce != null)
                    {
                        if (ce.IsFolder) return;
                        var dragObj = new DragObject();
                        dragObj.DragImg = FileTex;
                        UI.CurUI.DragObj = dragObj;
                        var item = new LabelForm().Set(-10, 50, 120, 25, ce.Name);
                        dragObj.Add(item);
                        dragObj.DragObj = ce;
                        dragObj.DragSubObj = this;
                        DragEntry = null;
                    }
                }
                if (b == 2)
                {
                    GoBack();
                }
            };
            DoubleClick = (b) =>
            {
                var ce = GetContent();
                if (ce == null) return;
                if (ce.IsFolder)
                {
                    Root.SetPath(ce.FullPath);
                    return;
                }
                else
                {
                    Root.LoadAsset(ce.FullPath);
                }
            };
        }

        public void GoBack()
        {
            if (Root.PP.Count > 1)
            {
                var pp = Root.PP[Root.PP.Count - 2];
                for (int i = 0; i < 2; i++)
                {
                    Root.PP.Remove(Root.PP[Root.PP.Count - 1]);
                }
                Root.SetPath(pp);
            }
        }

        private ContentEntry GetContent()
        {
            foreach (var ce in Contents)
            {
                if (UI.MX > GX + ce.X)
                {
                    if (UI.MX < (GX + ce.X + 64))
                    {
                        if (UI.MY > GY + (ce.Y + YDif))
                        {
                            if (UI.MY < (GY + ce.Y + 64))
                            {
                                return ce;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}

