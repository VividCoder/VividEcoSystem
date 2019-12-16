using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Scene
{
    public class GraphMarker : GraphNode
    {
        public string Type
        {
            get;
            set;
        }
        public string SubType
        {
            get;
            set;
        }
        public int Index
        {
            get;
            set;
        }
        public override void Write(System.IO.BinaryWriter w)
        {
            if(Type == null)
            {
                Type = "";
            }
            if(SubType == null)
            {
                SubType = "";
            }
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Type);
            w.Write(SubType);
            w.Write(Index);


        }
        public override void Read(System.IO.BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            Type = r.ReadString();
            SubType = r.ReadString();
            Index = r.ReadInt32();
        }
    }
}
