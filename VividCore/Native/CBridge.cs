using System;
using System.Runtime.InteropServices;

namespace Vivid.Native
{
    public class CBridge
    {
        [DllImport("vecbridge.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int drawSimpleRect(int x, int y, int w, int h);

        [DllImport("vecbridge.dll")]
        public static extern void setDrawCol(float r, float g, float b, float a);

        [DllImport("vecbridge.dll")]
        public static extern void setDrawBlend(int mode);

        [DllImport("vecbridge.dll")]
        public static extern IntPtr genQuad(int x, int y, int w, int h);
    }
}