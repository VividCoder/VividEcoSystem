using PhysX;

using System.IO;

namespace Vivid.Physics
{
    public class PyObject
    {
        public PhysX.Material Mat = null;

        public PyObject()
        {
            SetMat();
        }

        public void SetMat()
        {
            Mat = PhysicsManager.py.CreateMaterial(0.7f, 0.7f, 0.1f);
        }

        public virtual void Create(Scene.Entity3D ent)
        {
        }

        public virtual void Grab()
        {
        }
    }

    public class PyStatic : PyObject
    {
        public RigidStatic RID = null;
        public PhysX.Material Mat = null;
        public PhysX.Shape Shape;

        public PyStatic(Scene.Entity3D ent)
        {
            Mat = PhysicsManager.py.CreateMaterial(0.7f, 0.7f, 0.1f);
            CreateMesh(ent);
            PhysicsManager.AddObj(this);
        }

        public void CreateMesh(Scene.Entity3D ent)
        {
            System.Collections.Generic.List<OpenTK.Vector3> verts = ent.GetAllVerts();
            System.Collections.Generic.List<int> tris = ent.GetAllTris();


            System.Numerics.Vector3[] rvert = new System.Numerics.Vector3[verts.Count];

            int vi = 0;
            foreach (OpenTK.Vector3 v in verts)
            {
                rvert[vi] = new System.Numerics.Vector3(v.X, v.Y, v.Z);

                vi++;
            }

            int[] at = new int[tris.Count];

            for (int i = 0; i < tris.Count; i++)
            {
                at[i] = tris[i];
            }

            TriangleMeshDesc tm = new TriangleMeshDesc()
            {
                Flags = 0,
                Triangles = at,
                Points = rvert
            };

            Cooking cook = PhysicsManager.py.CreateCooking();

            MemoryStream str = new MemoryStream();
            TriangleMeshCookingResult cookr = cook.CookTriangleMesh(tm, str);

            str.Position = 0;

            TriangleMesh trim = PhysicsManager.py.CreateTriangleMesh(str);

            TriangleMeshGeometry trig = new TriangleMeshGeometry(trim);

            RID = PhysicsManager.py.CreateRigidStatic();

            Shape ns = RigidActorExt.CreateExclusiveShape(RID, trig, Mat);

            //RID.CreateShape ( trig, Mat );

            var wm = ent.WorldNoScale;

            float m11 = wm.M11;
            float m12 = wm.M12;
            float m13 = wm.M13;
            float m14 = wm.M14;

            float m21 = wm.M21;
            float m22 = wm.M22;
            float m23 = wm.M23;
            float m24 = wm.M24;

            float m31 = wm.M31;
            float m32 = wm.M32;
            float m33 = wm.M33;
            float m34 = wm.M34;

            float m41 = wm.M41;
            float m42 = wm.M42;
            float m43 = wm.M43;
            float m44 = wm.M44;

            System.Numerics.Matrix4x4 tp = new System.Numerics.Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

            // RID.GlobalPose = System.Numerics.Matrix4x4.CreateRotationX(-(float)System.Math.PI / 2);
            //RID.GlobalPosePosition = ent.LocalPos;

            PhysicsManager.Scene.AddActor(RID);
        }
    }

    public class PyDynamic : PyObject
    {
        public Scene.Entity3D Sent;
        public RigidDynamic ID = null;
        public RigidStatic RID = null;
        public PhysX.Material Mat = null;
        public PhysX.Shape Shape;

        // public System.Numerics.Matrix4x4 Pose;
        public override void Grab()
        {
            OpenTK.Matrix4 mat = PoseTurn;
            OpenTK.Vector3 pos = PosePos;
            // Console.WriteLine("Pos:" + pos.X + " " + pos.Y + " " + pos.Z); Sent.LocalPos = mat.ExtractTranslation();
            mat = mat.ClearTranslation();
            mat = mat.ClearScale();
            //var y = pos.Y;
            //pos.Y =-pos.Z;
            //pos.Z = -y;

            Sent.LocalPos = pos;
            Sent.LocalTurn = PoseTurn;
            // Sent.LocalTurn = mat;
        }

        public PyDynamic(PyType type, Scene.Entity3D ent)
        {
            Sent = ent;
            PhysicsManager.AddObj(this);
            Mat = PhysicsManager.py.CreateMaterial(0.7f, 0.7f, 0.1f);
            switch (type)
            {
                case PyType.Box:
                    CreateBox(ent);
                    break;

                    break;
            }
        }

        public OpenTK.Vector3 PosePos
        {
            get
            {
                System.Numerics.Vector3 pp = ID.GlobalPosePosition;
                return new OpenTK.Vector3(pp.X, pp.Y, pp.Z);
            }
        }

        public OpenTK.Matrix4 PoseTurn
        {
            get
            {
                System.Numerics.Matrix4x4 m;

                m = ID.GlobalPose;

                m.Translation = new System.Numerics.Vector3(0, 0, 0);

                OpenTK.Matrix4 res;
                res = new OpenTK.Matrix4(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
                return res;
            }
            set
            {
                OpenTK.Matrix4 World = value;

                float m11 = World.M11;
                float m12 = World.M12;
                float m13 = World.M13;
                float m14 = World.M14;

                float m21 = World.M21;
                float m22 = World.M22;
                float m23 = World.M23;
                float m24 = World.M24;

                float m31 = World.M31;
                float m32 = World.M32;
                float m33 = World.M33;
                float m34 = World.M34;

                float m41 = World.M41;
                float m42 = World.M42;
                float m43 = World.M43;
                float m44 = World.M44;

                System.Numerics.Matrix4x4 tm = new System.Numerics.Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

                ID.GlobalPose = tm;
            }
        }

        public void CreateBox(Scene.Entity3D ent)
        {
            Scene.Bounds bb = ent.Bounds;
            ID = PhysicsManager.py.CreateRigidDynamic();

            BoxGeometry ge = new BoxGeometry(bb.W / 2, bb.H / 2, bb.D / 2);
            //            Shape = ID.CreateShape ( ge, Mat );
            ID.LinearVelocity = new System.Numerics.Vector3(0, 0, 0);
            Shape = RigidActorExt.CreateExclusiveShape(ID, ge, Mat);



            //Pose = ID.GlobalPose;
            var wm = ent.WorldNoScale;
            float m11 = wm.M11;
            float m12 = wm.M12;
            float m13 = wm.M13;
            float m14 = wm.M14;

            float m21 = wm.M21;
            float m22 = wm.M22;
            float m23 = wm.M23;
            float m24 = wm.M24;

            float m31 = wm.M31;
            float m32 = wm.M32;
            float m33 = wm.M33;
            float m34 = wm.M34;

            float m41 = wm.M41;
            float m42 = wm.M42;
            float m43 = wm.M43;
            float m44 = wm.M44;

            System.Numerics.Matrix4x4 tm = new System.Numerics.Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

            ID.GlobalPose = tm;

            ID.SetMassAndUpdateInertia(3);

            Physics.PhysicsManager.Scene.AddActor(ID);
        }

        public void ApplyForce(OpenTK.Vector3 imp)
        {


            ID.AddForce(new System.Numerics.Vector3(imp.X, imp.Y, imp.Z), ForceMode.Force, true);


        }

        public void ApplyForceLocal(OpenTK.Vector3 imp, OpenTK.Vector3 local)
        {

            ID.AddForceAtLocalPosition(new System.Numerics.Vector3(imp.X, imp.Y, imp.Z), new System.Numerics.Vector3(local.X, local.Y, local.Z), ForceMode.Force, true);
        }

        public void AddTorque(OpenTK.Vector3 tr)
        {

            ID.AddTorque(new System.Numerics.Vector3(tr.X, tr.Y, tr.Z), ForceMode.Force, true);

        }



        public void GetPose()
        {
        }

    }
    public enum ForceType
    {
        Local, Center
    }
}