using System;
using System.Runtime.InteropServices;

namespace TapeFM.Server.Code.StreamServer.Native
{
    public static class NativeOpus
    {
        public const int CtlBitrate = 4002;

        [DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr opus_encoder_create(int samplingRate, int channels, int application, out int error);

        [DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opus_encode(IntPtr encoder, byte[] pcm, int frameSize,
            byte[] output, int outputSize);

        [DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
        public static extern void opus_encoder_destroy(IntPtr encoder);

        [DllImport("opus", CallingConvention = CallingConvention.Cdecl, EntryPoint = "opus_encoder_ctl")]
        public static extern void opus_encoder_ctl_bitrate(IntPtr encoder, int request, int bitsPerSecond);
    }
}