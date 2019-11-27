﻿using OpenTK;
using Vivid.App;
using Vivid.Help;

namespace Vivid.Scene
{
    public class Cam3D : Node3D
    {
        public Vector3 LR = new Vector3(0, 0, 0);
        public Matrix4 PP;

        public Matrix4 ProjMat
        {
            get
            {
                var nm = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), (float)AppInfo.RW / (float)AppInfo.RH, MinZ, MaxZ);

                return nm; ;
            }
        }

        public Vector3 ViewVec
        {
            get
            {
                Vector3 f = new Vector3(0, 0, 1);
                f = Vector3.TransformVector(f, World);
                return -f;
            }
        }

        public void Write()
        {
            IOHelp.WriteMatrix(LocalTurn);
            IOHelp.WriteVec(LocalPos);
            IOHelp.WriteFloat(MinZ);
            IOHelp.WriteFloat(MaxZ);
            IOHelp.WriteVec(LR);
        }

        public void Read()
        {
            LocalTurn = IOHelp.ReadMatrix();
            LocalPos = IOHelp.ReadVec3();
            MinZ = IOHelp.ReadFloat();
            MaxZ = IOHelp.ReadFloat();
            LR = IOHelp.ReadVec3();
        }

        public override void Rot(Vector3 r, Space s)
        {
            LR = r;
            CalcMat();
        }

        public override void Turn(Vector3 t, Space s)
        {
            LR = LR + t;
            CalcMat();

            //Matrix4 t = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
            //LocalTurn = t * LocalTurn;
        }

        public void CalcMat()
        {
            //LR.X = Wrap(LR.X);
            // LR.Y = Wrap(LR.Y);
            //  LR.Z = Wrap(LR.Z);
            Vector3 r = LR;
            Matrix4 t = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y));// * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
            LocalTurn = t;
        }

        public float Wrap(float v)
        {
            if (v < 0)
            {
                v = 360 + v;
            }
            if (v > 360)
            {
                v = v - 360;
            }
            if (v < 0 || v > 360)
            {
                return Wrap(v);
            }

            return v;
        }

        public float FOV = 35;
        public bool DepthTest = true;
        public bool AlphaTest = false;
        public bool CullFace = true;
        public float MinZ = 0.1f, MaxZ = 1508;

        public Cam3D()
        {
            Rot(new Vector3(0, 0, 0), Space.Local);
        }

        public Matrix4 CamWorld
        {
            get
            {
                Matrix4 m = Matrix4.Invert(World);
                return m;
            }
        }

        public Matrix4 PrevCamWorld
        {
            get
            {
                Matrix4 m = Matrix4.Invert(PrevWorld);
                return m;
            }
        }
    }
}