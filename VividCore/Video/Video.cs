using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Vivid.Video
{
    public class VideoFrame
    {
        public Texture.Texture2D Img = null;
        public long PTS, PKTPTS, DTS;
        public int W, H;
        public byte[] Data;
        public long Clock;
        public double DPTS = 0.0;
        public double DDELAY = 0.0;
        public long Pict = 0;
        public int CacheID = -1;

        // public DataCache Data = null;
        public bool Paused = false;

        ~VideoFrame()
        {
            if (Img != null)
            {
                Img.Delete();
            }
        }
    }

    public class DataCache
    {
        public byte[] Data = null;
    }

    public class VideoPlayer
    {
        private IntPtr vid = IntPtr.Zero;
        public static bool AudioUp = false;
        public Queue<VideoFrame> Frames = new Queue<VideoFrame>();

        public VideoFrame CurrentFrame = null;
        public double CLOCK = 0.0;
        public double AUDIOCLOCK = 0.0;
        public long TimeStart = 0;
        public Texture.Texture2D CurrentTex = null;

        public List<DataCache> FreeCache = new List<DataCache>();
        public List<DataCache> UsedCache = new List<DataCache>();
        public Mutex FrameMutex = new Mutex();
        public bool Paused = false;
        public long audioStartTime = -1;

        public VideoPlayer(string path)
        {
            Profiler.Profile.BeginBlock("InitVideo");

            if (AudioUp == false)
            {
                int res = VideoIn.initAudio();
                AudioUp = true;
            }

            //Console.WriteLine("VideoInit:" + VideoIn
            var rt = Marshal.StringToHGlobalAnsi(path);
            vid = VideoIn.initVideoNative(rt);

            TimeStart = Environment.TickCount;

            var decode_thread = new Thread(new ThreadStart(Thr_Decode));
            decode_thread.Start();
            CurThr = decode_thread;
            CurThr.Priority = ThreadPriority.AboveNormal;
            Profiler.Profile.EndBlock("InitVideo");
        }

        ~VideoPlayer()
        {
            CurThr.Abort();
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Play()
        {
            Paused = false;
        }

        public bool gotAudioTime = false;
        public Thread CurThr = null;
        public double TimeDelta = 0.0;

        public void Thr_Decode()
        {
            while (true)
            {
                if (!Paused)
                {
                    if (gotAudioTime == false)
                    {
                        if (VideoIn.audioHasBegun() == 1)
                        {
                            gotAudioTime = true;
                            audioStartTime = Environment.TickCount;
                        }
                    }
                    else
                    {
                        long curTime = Environment.TickCount;

                        long timed = curTime - audioStartTime;
                        AUDIOCLOCK = ((double)(timed) / 1000.0);
                    }

                    FrameMutex.WaitOne();

                    while (true)
                    {
                        if (Frames.Count == 0) break;
                        var frm = Frames.Peek();
                        if (frm.DPTS < AUDIOCLOCK)
                        {
                            Frames.Dequeue();
                        }
                        else
                        {
                            break;
                        }
                    }
                    FrameMutex.ReleaseMutex();
                    if (Frames.Count > 0)
                    {
                        double ft = Frames.Peek().DPTS;

                        TimeDelta = ft - AUDIOCLOCK;
                    }

                    FrameMutex.WaitOne();
                    if (Frames.Count < 8)
                    {
                        DecodeNextFrame();
                    }
                    FrameMutex.ReleaseMutex();
                }
                //  GC.Collect();
                //  T//hread.Sleep(2);
            }
        }

        public int Width, Height, Line;
        public byte[] Data;

        public bool CacheBuilt = false;

        public void DecodeNextFrame()
        {
            Profiler.Profile.BeginBlock("DecodeFrame");
            VideoIn.decodeNextFrame(vid);

            int fw = VideoIn.getFrameWidth(vid);
            int fh = VideoIn.getFrameHeight(vid);
            if (fw < 0 || fw > 10000)
            {
                Profiler.Profile.EndBlock("DecodeFrame");
                return;
            }

            Width = fw;
            Height = fh;

            // if (Data == null)
            // {
            //   Data = new byte[Width * Height * 3];w
            //}

            VideoIn.genFrameData(vid);

            //  var tex = new Texture.Texture2D(Width, Height, Data, false);

            VideoFrame frame = new VideoFrame();
            // frame.Data = buf;

            frame.W = Width;

            frame.H = Height;
            frame.DPTS = VideoIn.getDPTS(vid);
            if (frame.DPTS < CLOCK - 0.1f) return;

            byte[] buf = new byte[Width * Height * 3];

            VideoIn.getFrameData(vid, buf);
            frame.Data = buf;
            frame.DDELAY = VideoIn.getDDelay(vid);
            frame.Pict = VideoIn.getPict(vid);

            FrameMutex.WaitOne();
            Frames.Enqueue(frame);
            FrameMutex.ReleaseMutex();

            Profiler.Profile.EndBlock("DecodeFrame");
        }

        public double GetClock()
        {
            return CLOCK;
        }

        public void SetClock(double v)
        {
            CLOCK = v;
        }

        public long LastTick = 0;
        public long CurTick = 0;
        public double timeDelay = 0.0;
        public int CurrentTexID = -1;

        public Texture.Texture2D GetCurrentImage()
        {
            int used = 0;
            Profiler.Profile.BeginBlock("GetCurrentImage");

            if (CurrentFrame == null)
            {
                Profiler.Profile.EndBlock("GetCurrentImage");
                return null;
            }
            if (CurrentTex == null) CurrentTex = new Texture.Texture2D(Width, Height, new byte[Width * Height * 3], false);

            if (CurrentFrame == UpFrame)
            {
                Profiler.Profile.EndBlock("GetCurrentImage");
                return CurrentTex;
            }

            CurrentTex.LoadSub(CurrentFrame.Data);
            UpFrame = CurrentFrame;
            Profiler.Profile.EndBlock("GetCurrentImage");
            return CurrentTex;
        }

        public VideoFrame UpFrame = null;

        public VideoFrame GetCurrentFrame()
        {
            Profiler.Profile.BeginBlock("GetFrame");

            //  GC.Collect();

            long ctick = Environment.TickCount;

            if (LastTick == 0)
            {
                LastTick = ctick;
            }

            if (Paused)
            {
                LastTick = ctick;
                Profiler.Profile.EndBlock("GetFrame");
                return CurrentFrame;
            }

            double ts = (double)(ctick - LastTick) / 1000.0;

            LastTick = ctick;

            CLOCK += ts;

            double dt = CLOCK - TimeDelta; // - timeDelay * 30;

            // Console.WriteLine("Clock:" + dt);

            if (Frames.Count == 0) return null;

            if (CurrentFrame == null)
            {
                if (Frames.Count == 0) return null;
                FrameMutex.WaitOne();
                CurrentFrame = Frames.Dequeue();
                FrameMutex.ReleaseMutex();
            }

            if (CurrentFrame.DPTS <= dt)
            {
            }
            else
            {
                Profiler.Profile.EndBlock("GetFrame");
                return CurrentFrame;
            }

            if (CurrentFrame != null)
            {
                if (Frames.Count > 0)
                {
                    //  CurrentFrame.Img.Delete();

                    CurrentFrame = Frames.Dequeue();

                    // CLOCK = CurrentFrame.DPTS;

                    double dd = (double)CurrentFrame.DDELAY;
                    dd += ((double)CurrentFrame.Pict * (CurrentFrame.DDELAY * 0.5));

                    timeDelay = dd;

                    //dd = dd * 1000;

                    dd = dd * 50;
                    //LastTick += (long)dd;

                    //CLOCK = CLOCK += dd;
                    //CLOCK = CLOCK -= dd;
                }
                Profiler.Profile.EndBlock("GetFrame");
                return CurrentFrame;
            }
            if (Frames.Count == 0)
            {
                Profiler.Profile.EndBlock("GetFrame");
                return null;
            }
            if (CurrentFrame == null)
            {
                FrameMutex.WaitOne();
                CurrentFrame = Frames.Dequeue();
                FrameMutex.ReleaseMutex();
            }
            Profiler.Profile.EndBlock("GetFrame");
            return CurrentFrame;
        }

        public class VideoIn
        {
            [DllImport("VideoNative.dll")]
            public static extern int audioHasBegun();

            [DllImport("VideoNative.dll")]
            public static extern void genFrameData(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern int initAudio();

            [DllImport("VideoNative.dll")]
            public static extern long getPict(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern IntPtr initVideoNative(IntPtr path);

            [DllImport("VideoNative.dll")]
            public static extern int decodeNextFrame(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern void getFrameData(IntPtr vid, byte[] buf);

            [DllImport("VideoNative.dll")]
            public static extern long getLastDTS(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern long getLastPTS(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern long getLastPktPTS(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern double getDPTS(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern double getDDelay(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern int getFrameLineSize(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern int getFrameWidth(IntPtr vid);

            [DllImport("VideoNative.dll")]
            public static extern int getFrameHeight(IntPtr vid);
        }
    }
}