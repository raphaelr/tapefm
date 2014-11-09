using System.IO;

namespace TapeFM.Server.Code.StreamServer.Mux
{
    public static class OpusHeaderGenerator
    {
        public const ushort Preskip = 0xf000;
        public static void GenerateOpusHeader(OggMuxer muxer, Stream target)
        {
            var identification = GenerateIdentification();
            muxer.UpdatePage(HeaderType.BeginOfStream, 0, identification, identification.Length);
            target.Write(muxer.OutputBuffer, 0, muxer.OutputBufferLength);

            var comment = GenerateComment();
            muxer.UpdatePage(HeaderType.Default, 0, comment, comment.Length);
            target.Write(muxer.OutputBuffer, 0, muxer.OutputBufferLength);
        }

        private static byte[] GenerateIdentification()
        {
            return new byte[]
            {
                0x4f, 0x70, 0x75, 0x73, 0x48, 0x65, 0x61, 0x64, // OpusHead
                1, // version
                2, // channels
                Preskip & 0xff, (Preskip & 0xff00) >> 8, // pre skip
                0x80, 0xb8, 0, 0, // sample rate (48000 Hz)
                0, 0, // gain
                0 // channel map (RTP stereo)
            };
        }

        private static byte[] GenerateComment()
        {
            return new byte[]
            {
                0x4f, 0x70, 0x75, 0x73, 0x54, 0x61, 0x67, 0x73, // OpusTags
                6, 0, 0, 0, // Vendor Length
                0x74, 0x61, 0x70, 0x65, 0x46, 0x4d, // Vendor String,
                0, 0, 0, 0 // Num Tags
            };
        }
    }
}