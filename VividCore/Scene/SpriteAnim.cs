using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid.Scene
{
    public class SpriteAnim
    {

        public Tex.Tex2D GetFrame(int frame)
        {
            return Frames[frame];
        }
        public List<Tex.Tex2D> Frames = new List<Tex.Tex2D>();

        public float Speed = 0.1f;

        public SpriteAnim(string path,float spd)
        {
            Speed = spd;

            var fi = new System.IO.DirectoryInfo(path);
            foreach(var f in fi.GetFiles())
            {
                Console.WriteLine("F:" + f.Name);
                if (f.Name.Contains(".cache")) continue;
                var tex = new Tex.Tex2D(path+f.Name, true);
                Frames.Add(tex);
            }


        }

    }
}
