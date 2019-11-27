using System;
using System.Collections.Generic;
using System.Threading;

namespace Vivid.Jobs
{
    public class JobTimer
    {
        public Thread JobThread = null;
        public int Interval = 0;
        public JobFunc Job = null;
        public static List<JobTimer> Jobs = new List<JobTimer>();

        public JobTimer(int interval, JobFunc job)
        {
            Interval = interval;
            Job = job;
            Jobs.Add(this);
            JobThread = new Thread(new ThreadStart(Job_Thread));
            JobThread.Start();
        }

        public void Job_Thread()
        {
            int lj = Environment.TickCount - Interval;
            while (true)
            {
                int ct = Environment.TickCount;

                if (ct > (lj + Interval))
                {
                    if (Job.Invoke())
                    {
                        JobThread.Abort();
                    }

                    lj = ct;
                }

                Thread.Sleep(1);
            }
        }
    }

    public delegate bool JobFunc();
}