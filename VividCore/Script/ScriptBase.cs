﻿using Vivid.Scene;

namespace Vivid.Script
{
    public class ScriptLink : Node3D
    {
        private dynamic script;

        public string FilePath
        {
            get;
            set;
        }

        public bool Compiled
        {
            get;
            set;
        }

        public ScriptLink()
        {
            Compiled = false;
        }

        public void Compile(Node3D node)
        {
            if (Compiled)
            {
                return;
            }

            Compiled = true;

            script.Node = node;
            System.Console.WriteLine("Script:" + FilePath + " Compiled.");
        }

        public void Update()
        {
            script.Update();
        }

        public void Begin()
        {
            script.Begin();
        }

        public void End()
        {
            script.End();
        }
    }
}