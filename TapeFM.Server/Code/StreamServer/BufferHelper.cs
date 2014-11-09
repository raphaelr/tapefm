using System.IO;

namespace TapeFM.Server.Code.StreamServer
{
    public static class BufferHelper
    {
        public static bool ReadFull(this Stream stream, byte[] buffer)
        {
            var bytesRead = 0;
            while (bytesRead < buffer.Length)
            {
                var currentRead = stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
                if (currentRead == 0)
                {
                    ZeroBuffer(buffer, bytesRead);
                    return false;
                }
                bytesRead += currentRead;
            }
            return true;
        }

        private static void ZeroBuffer(byte[] buffer, int offset)
        {
            for (var i = offset; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
        }
    }
}