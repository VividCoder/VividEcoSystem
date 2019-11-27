using System.IO;

namespace Vivid.Help
{
    public class IOHelp
    {
        public static BinaryWriter w;
        public static BinaryReader r;

        public static void WriteMatrix(OpenTK.Matrix4 m)
        {
            WriteVec(m.Row0);
            WriteVec(m.Row1);
            WriteVec(m.Row2);
            WriteVec(m.Row3);
        }

        public static void WriteVec(OpenTK.Vector4 v)
        {
            w.Write(v.X);
            w.Write(v.Y);
            w.Write(v.Z);
            w.Write(v.W);
        }

        public static void WriteVec(OpenTK.Vector3 v)
        {
            w.Write(v.X);
            w.Write(v.Y);
            w.Write(v.Z);
        }

        public static void WriteFloat(float v)
        {
            w.Write(v);
        }

        public static void WriteDouble(double v)
        {
            w.Write(v);
        }

        public static void WriteBool(bool v)
        {
            w.Write(v);
        }

        public static void WriteString(string s)
        {
            if (s == null) s = " ";
            w.Write(s);
        }

        public static void WriteInt(int v)
        {
            w.Write(v);
        }

        public static int ReadInt()
        {
            return r.ReadInt32();
        }

        public static void WriteBytes(byte[] b)
        {
            w.Write(b.Length);
            w.Write(b);
        }

        public static byte[] ReadBytes()
        {
            int bc = r.ReadInt32();
            return r.ReadBytes(bc);
        }

        public static string ReadString()
        {
            return r.ReadString();

        }

        public static void WriteTextureList(System.Collections.Generic.List<Texture.Texture2D> list)
        {
            WriteInt(list.Count);
            foreach (var tex in list)
            {
                WriteTexture2D(tex);
            }

        }

        public static System.Collections.Generic.List<Texture.Texture2D> ReadTextureList()
        {
            var tl = new System.Collections.Generic.List<Texture.Texture2D>();
            int lc = ReadInt();
            for (int i = 0; i < lc; i++)
            {
                tl.Add(ReadTexture2D());
            }
            return tl;
        }

        public static OpenTK.Matrix4 ReadMatrix()
        {
            OpenTK.Matrix4 m = new OpenTK.Matrix4(ReadVec4(), ReadVec4(), ReadVec4(), ReadVec4());
            return m;
        }

        public static OpenTK.Vector4 ReadVec4()
        {
            return new OpenTK.Vector4(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }

        public static OpenTK.Vector3 ReadVec3()
        {
            return new OpenTK.Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }

        public static float ReadFloat()
        {
            return r.ReadSingle();
        }

        public static double ReadDouble()
        {
            return r.ReadDouble();
        }

        public static void WriteTexture2D(Texture.Texture2D tex)
        {
            WriteString(tex.Path);
        }

        public static Texture.Texture2D ReadTexture2D()
        {
            return new Texture.Texture2D(ReadString(), Texture.LoadMethod.Single, true);
        }

        public static bool ReadBool()
        {
            return r.ReadBoolean();
        }
    }
}