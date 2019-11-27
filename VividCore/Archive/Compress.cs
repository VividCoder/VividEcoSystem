using System.IO;
using System.IO.Compression;

namespace Vivid.Archive
{
    public static class Compression
    {
        public static byte[] Compress(byte[] data)
        {
            MemoryStream com_mem = new MemoryStream();

            MemoryStream mem = new MemoryStream(data);
            mem.Seek(0, SeekOrigin.Begin);

            using (DeflateStream comStream = new DeflateStream(com_mem, CompressionMode.Compress))
            {
                mem.CopyTo(comStream);
            }

            return com_mem.GetBuffer();
        }
    }

    public static class Decompression
    {
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream decom_mem = new MemoryStream();

            MemoryStream mem = new MemoryStream(data);
            mem.Seek(0, SeekOrigin.Begin);

            using (DeflateStream decomStream = new DeflateStream(mem, CompressionMode.Decompress))
            {
                decomStream.CopyTo(decom_mem);
            }

            return decom_mem.GetBuffer();
        }
    }
}