namespace Vivid.Pixels
{
    public class PixelMap
    {
        public int Width;
        public int Height;
        public bool Alpha = true;
        public byte[] Data = null;
        private int pixelSize = 3;

        public PixelMap(int w, int h, bool alpha)
        {
            Width = w;
            Height = h;
            Alpha = alpha;
            if (alpha) pixelSize = 4;
            Data = new byte[w * h * pixelSize];
            for (int l = 0; l < Data.Length; l++)
            {
                Data[l] = 0;
            }
        }

        public byte GetR(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize)];
        }

        public byte GetG(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize) + 1];
        }

        public byte GetB(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize) + 2];
        }

        public byte GetA(int x, int y)
        {
            return Data[y * Width * pixelSize + (x * pixelSize) + 3];
        }

        public void GetRGB(int x, int y, ref byte R, ref byte G, ref byte B, ref byte A)
        {
            R = GetR(x, y); G = GetG(x, y); B = GetB(x, y); A = GetA(x, y);
        }

        public void SetRGB(int x, int y, byte r, byte g, byte b, byte a)
        {
            Data[y * Width * pixelSize + (x * pixelSize)] = r;
            Data[y * Width * pixelSize + (x * pixelSize) + 1] = g;
            Data[y * Width * pixelSize + (x * pixelSize) + 2] = b;
            Data[y * Width * pixelSize + (x * pixelSize) + 3] = a;
        }
    }
}