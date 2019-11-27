﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Vivid.FrameBuffer;
using Vivid.Settings;

namespace Vivid.Scene.Node
{
    public enum LightType
    {
        Ambient, Directional, Point
    }

    public class Light3D : Node3D
    {
        public static Light3D Active = null;

        public static int LightNum = 0;

        public bool CastShadows = true;

        public FrameBufferCube ShadowFB = null;

        public Flare.Flare3D LensFlare = null;

        // public Vector3 AmbCE = 0.3f;
        public Texture.TextureCube ShadowMap = null;

        public LightType Type = LightType.Point;

        public Light3D()
        {
            Diff = new Vector3(2.9f, 2.9f, 2.9f);
            Spec = new Vector3(0.2f, 0.2f, 0.2f);
            Amb = new Vector3(0.2f, 0.2f, 0.2f);
            Atten = 0.1f;
            CreateShadowFBO();
            LightNum++;
            Range = 1500;
        }

        public Vector3 Amb
        {
            get;
            set;
        }

        public float Atten
        {
            get;
            set;
        }

        public Vector3 Diff
        {
            get;
            set;
        }

        public float Range
        {
            get;
            set;
        }

        public Vector3 Spec
        {
            get;
            set;
        }

        public void Changed()
        { }

        public void CreateShadowFBO()
        {
            ShadowFB = new FrameBufferCube(Quality.ShadowMapWidth, Quality.ShadowMapHeight);
        }

        public void DrawShadowMap(SceneGraph3D graph)
        {
            Active = this;

            Cam3D cam = new Cam3D();
            Effect.FXG.Cam = cam;
            cam.FOV = 90;
            cam.MaxZ = Quality.ShadowDepth;

            graph.CamOverride = cam;
            cam.LocalPos = LocalPos;
            cam.MaxZ = Quality.ShadowDepth;
            // cam.LocalTurn = LocalTurn;

            int fn = 0;

            GL.Disable(EnableCap.ScissorTest);
            App.SetVP.Set(0, 0, App.AppInfo.W, App.AppInfo.H);

            TextureTarget f = ShadowFB.SetFace(fn);

            SetCam(f, cam);

            graph.RenderingShadows = true;

            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(1), cam);
            graph.RenderDepth();

            // ShadowFB.Release(); graph.CamOverride = null;

            SetCam(ShadowFB.SetFace(2), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(3), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(4), cam);
            graph.RenderDepth();

            SetCam(ShadowFB.SetFace(5), cam);
            graph.RenderDepth();

            ShadowFB.Release();

            graph.RenderingShadows = false;
            graph.CamOverride = null;
        }

        public void Read()
        {
            LocalTurn = Help.IOHelp.ReadMatrix();
            LocalPos = Help.IOHelp.ReadVec3();
            Diff = Help.IOHelp.ReadVec3();
            Spec = Help.IOHelp.ReadVec3();
            Amb = Help.IOHelp.ReadVec3();
            Range = Help.IOHelp.ReadFloat();
            CastShadows = Help.IOHelp.ReadBool();
        }

        public void Write()
        {
            Help.IOHelp.WriteMatrix(LocalTurn);
            Help.IOHelp.WriteVec(LocalPos);
            Help.IOHelp.WriteVec(Diff);
            Help.IOHelp.WriteVec(Spec);
            Help.IOHelp.WriteVec(Amb);
            Help.IOHelp.WriteFloat(Range);
            Help.IOHelp.WriteBool(CastShadows);
        }

        private static void SetCam(TextureTarget f, Cam3D Cam)
        {
            switch (f)
            {
                case TextureTarget.TextureCubeMapPositiveX:
                    Cam.LookAtZero(new Vector3(1, 0, 0), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapNegativeX:
                    Cam.LookAtZero(new Vector3(-1, 0, 0), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapPositiveY:

                    Cam.LookAtZero(new Vector3(0, -1, 0), new Vector3(0, 0, -1));
                    break;

                case TextureTarget.TextureCubeMapNegativeY:
                    Cam.LookAtZero(new Vector3(0, 1, 0), new Vector3(0, 0, 1));
                    break;

                case TextureTarget.TextureCubeMapPositiveZ:
                    Cam.LookAtZero(new Vector3(0, 0, 1), new Vector3(0, -1, 0));
                    break;

                case TextureTarget.TextureCubeMapNegativeZ:
                    Cam.LookAtZero(new Vector3(0, 0, -1), new Vector3(0, -1, 0));
                    break;
            }
        }
    }
}