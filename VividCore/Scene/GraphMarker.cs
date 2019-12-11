using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Scene
{
    public class GraphMarker : GraphNode
    {
        public string Type = "";
        public string Name = "";
        public int Num = 0;
        public override void Write(System.IO.BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Type);
            w.Write(Name);
            w.Write(Num);


        }
        public override void Read(System.IO.BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            Type = r.ReadString();
            Name = r.ReadString();
            Num = r.ReadInt32();
        }
    }
}
