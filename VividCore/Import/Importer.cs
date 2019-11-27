﻿using Vivid.Scene;

namespace Vivid.Import
{
    public class Importer
    {
        public string Ext = "";

        public virtual Node3D LoadNode(string path)
        {
            return null;
        }

        public virtual Node3D LoadAnimNode(string path)
        {
            return null;
        }

        public virtual SceneGraph3D LoadScene(string path)
        {
            return null;
        }
    }
}