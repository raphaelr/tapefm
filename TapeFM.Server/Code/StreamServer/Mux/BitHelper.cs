namespace TapeFM.Server.Code.StreamServer.Mux
{
    public static class BitHelper
    {
        public static void SetBytes32(this byte[] array, int offset, uint value)
        {
            array[offset + 0] = (byte)((value & 0x000000ff) >> 0);
            array[offset + 1] = (byte)((value & 0x0000ff00) >> 8);
            array[offset + 2] = (byte)((value & 0x00ff0000) >> 16);
            array[offset + 3] = (byte)((value & 0xff000000) >> 24);
        }

        public static void SetBytes64(this byte[] array, int offset, ulong value)
        {
            array.SetBytes32(offset + 0, (uint)((value & 0x00000000ffffffff) >> 0));
            array.SetBytes32(offset + 4, (uint)((value & 0xffffffff00000000) >> 32));
        }
    }
}