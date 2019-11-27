﻿using OpenTK;
using Vivid.Data;
using Vivid.Scene;

namespace Vivid.Effect
{
    public static class FXG
    {
        public static Effect3D FXOverride = null;
        public static Matrix4 Local = Matrix4.Identity;
        public static Matrix4 PrevLocal = Matrix4.Identity;
        public static Matrix4 Proj = Matrix4.Identity;
        public static Cam3D Cam = null;
        public static Entity3D Ent = null;
        public static Mesh3D Mesh = null;
    }
}