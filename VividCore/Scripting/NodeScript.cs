using System;
using System.Threading;
using Vivid.Resonance;
using XInputDotNetPure;


namespace Vivid.Scripting
{

    public class NodeScriptCompiler
    {



        public static NodeScript Compile(string path)
        {
            var ns = CSScriptLibrary.CSScript.Evaluator.LoadFile(path) as NodeScript;
            ns.FullPath = path;
            ns.DiskTime = System.IO.File.GetLastWriteTime(path);
            return ns;
        }

    }

    public class NodeScript
    {

        public DateTime DiskTime = DateTime.Now;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Name = "NodeScript";

        public Vivid.Reflect.ClassIO ClassCopy = null;

        public bool Updated
        {
            get
            {
                var time = System.IO.File.GetLastWriteTime(FullPath);
                int ct = time.CompareTo(DiskTime);
                if (time.CompareTo(DiskTime) != 0)
                {
                    Thread.Sleep(1000);
                    return true;
                }


                return false;

            }
        }

        public string GetName()
        {
            return new System.IO.FileInfo(FullPath).Name;
        }


        public string FullPath
        {
            get;
            set;
        }

        public Scene.Node3D Node
        {
            get;
            set;
        }

        public virtual void Transfer(NodeScript to)
        {

        }

        public virtual void ApplyInEditor()
        {

        }

        public virtual void SaveNode()
        {

        }

        public virtual void LoadNode()
        {

        }

        public virtual void InitUI(UI ui)
        {

        }

        public virtual void InitNode()
        {

        }

        public virtual void UpdateNode()
        {

        }

        public virtual void RenderNode()
        {

        }

        public void LoadScene(string path)
        {



        }

        public GamePadState GetPad(int player)
        {
            return GamePad.GetState(PlayerIndex.One + player);
        }

    }
}
