using System;
using TapeFM.Server.Code.StreamServer.Native;

namespace TapeFM.Server.Code.StreamServer
{
    public class Encoder : IDisposable
    {
        public const int RecommendedOutputBufferSize = 4000;

        private readonly int _numChannels;
        private IntPtr _encoder;
        private int _bitrateKbps;

        public int BitrateKbps
        {
            get { return _bitrateKbps; }
            set
            {
                if (_bitrateKbps != value)
                {
                    _bitrateKbps = value;
                    NativeOpus.opus_encoder_ctl_bitrate(_encoder, NativeOpus.CtlBitrate, value * 1024);
                }
            }
        }

        public Encoder(int samplingRate, int channels, OpusApplication application)
        {
            _numChannels = channels;
            int error;
            
            _encoder = NativeOpus.opus_encoder_create(samplingRate, channels, (int)application, out error);
            if (error != 0)
            {
                throw new Exception("Opus error: " + error);
            }
            BitrateKbps = 300;
        }

        public int Encode(byte[] pcm, byte[] output)
        {
            var result = NativeOpus.opus_encode(_encoder, pcm, pcm.Length/_numChannels/2, output, output.Length);
            if (result < 0)
            {
                throw new Exception("Opus error: " + result);
            }
            return result;
        }

        public void Dispose()
        {
            if (_encoder != IntPtr.Zero)
            {
                NativeOpus.opus_encoder_destroy(_encoder);
                _encoder = IntPtr.Zero;
            }
        }

        public enum OpusApplication
        {
            VoIp = 2048,
            Audio = 2049,
            RestrictedLowDelay = 2051
        }
    }
}