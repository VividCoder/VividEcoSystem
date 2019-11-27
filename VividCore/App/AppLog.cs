using System;

namespace Vivid.App
{
    public static class AppLog
    {
        public static void Log(string msg, string area = "")
        {
            Console.WriteLine(msg + "@" + area);
        }
    }
}