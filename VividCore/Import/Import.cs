﻿using System.Collections.Generic;
using System.IO;
using Vivid.Scene;

namespace Vivid.Import
{
    public static class Import
    {
        public static Dictionary<string, Importer> Imports = new Dictionary<string, Importer>();

        public static void RegDefaults()
        {
            RegImp(".3ds", new AssImpImport());
            RegImp(".fbx", new AssImpImport());
            RegImp(".blend", new AssImpImport());
            RegImp(".dae", new AssImpImport());
            RegImp(".b3d", new AssImpImport());
            RegImp(".gltf", new AssImpImport());
            RegImp(".x", new AssImpImport());
            RegImp(".obj", new AssImpImport());
            RegImp(".glb", new AssImpImport());
        }

        public static void RegImp(string key, Importer imp)
        {
            Imports.Add(key, imp);
        }

        public static Importer GetImp(string key)
        {
            if (Imports.ContainsKey(key))
            {
                return Imports[key];
            }
            return null;
        }

        public static Node3D ImportAnimNode(string path)
        {
            string key = new FileInfo(path).Extension.ToLower();
            Importer imp = Imports[key];
            Node3D r = imp.LoadAnimNode(path);
            return r;
        }

        public static Node3D ImportNode(string path)
        {
            string key = new FileInfo(path).Extension.ToLower();
            if (Imports.ContainsKey(key))
            {
                Importer imp = Imports[key];
                Node3D r = imp.LoadNode(path);
                return r;
            }
            else
            {
                return null;
            }
        }
    }
}