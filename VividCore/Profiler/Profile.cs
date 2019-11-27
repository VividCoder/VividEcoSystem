//#define Profile
using System;
using System.Collections.Generic;
using System.IO;

namespace Vivid.Profiler
{
    public class ProfileBlock
    {
        public string Name = "";
        public long RunTimes = 0;
        public long AvgMS = 0;
        public long BeginMS = 0;
        public long TotalMS = 0;
    }

    public static class Profile
    {
        //#define Profile

        public static FileStream fs = null;
        public static TextWriter tw = null;

        public static DateTime Start, End;

        public static Dictionary<string, ProfileBlock> Profiles = new Dictionary<string, ProfileBlock>();

        public static void BeginProfiling(string path, bool newFile = false)
        {
#if (Profile)
            Profiles.Clear();
            if (newFile)
            {
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Seek(fs.Length, SeekOrigin.Begin);
            }

            tw = new StreamWriter(fs);
            Start = DateTime.Now;

            tw.WriteLine("Profile:" + path + " Start Time:" + Start.ToLongTimeString());
#endif
        }

        public static void BeginBlock(string name)
        {
#if (Profile)
            ProfileBlock block = null;
            if (Profiles.ContainsKey(name))
            {
                block = Profiles[name];
            }
            else
            {
                block = new ProfileBlock()
                {
                    Name = name
                };
                Profiles.Add(name, block);
            }

            block.BeginMS = Environment.TickCount;
            block.RunTimes++;

#endif
        }

        public static void EndBlock(string name)
        {
#if (Profile)
            var block = Profiles[name];

            long time = Environment.TickCount;

            long per = time - block.BeginMS;

            long new_a = (block.AvgMS + per)/2;
            block.AvgMS = new_a;
            block.TotalMS += (time - per);
#endif
        }

        public static void Stop()
        {
#if (Profile)
            tw.WriteLine("");
            foreach (var block_key in Profiles.Keys)
            {
                var block = Profiles[block_key];

                tw.WriteLine("-----------------> Block");

                tw.WriteLine("Block:" + block.Name + "  Calls:" + block.RunTimes + "  AvgRunTime:" + block.AvgMS + "  RunTime(Secs):" + (float)(float)(block.AvgMS) / 1000.0f);

                tw.WriteLine("-----------------> End Block");
                tw.WriteLine("\n");
            }
            tw.WriteLine("");
            End = DateTime.Now;
            tw.WriteLine("Profile Ended: Time:" + End.ToLongTimeString());
            tw.Flush();
            fs.Flush();
            fs.Close();
#endif
        }
    }
}