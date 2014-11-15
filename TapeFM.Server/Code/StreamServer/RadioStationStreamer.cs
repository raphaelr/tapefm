using System;
using System.Diagnostics;
using System.Threading;

namespace TapeFM.Server.Code.StreamServer
{
    public class RadioStationStreamer
    {
        private static readonly TraceSource Trace = Logger.GetComponent("RadioStationStreamer");
        private static readonly TimeSpan IdleTimeToLive = TimeSpan.FromMinutes(5);

        private const int NumChannels = 2;
        private const int SampleRate = 48000;
        private const int FrameDurationMs = 60;
        private const int FrameSize = FrameDurationMs * NumChannels * SampleRate / 1000;

        private readonly object _syncRoot = new object();
        private readonly byte[] _pcmBuffer;
        private readonly byte[] _encodedBuffer;
        private readonly Encoder _encoder;
        private readonly RadioStationSource _source;
        private readonly Stopwatch _syncClock;
        private DateTime _lastPublish;
        private bool _stopFlag;
        private int _encodedBytes;

        public delegate void FrameEncodedCallback(byte[] frame, int length, int sampleLength);

        public event EventHandler TooLongIdle;
        public event FrameEncodedCallback FrameEncoded;

        public int BitrateKbps
        {
            get { return _encoder.BitrateKbps; }
            set { _encoder.BitrateKbps = value; }
        }

        public RadioStationStreamer(RadioStationSource source)
        {
            _pcmBuffer = new byte[FrameSize*2];
            _encodedBuffer = new byte[Encoder.RecommendedOutputBufferSize];
            _encoder = new Encoder(SampleRate, NumChannels, Encoder.OpusApplication.Audio);
            _syncClock = new Stopwatch();
            _source = source;
            _lastPublish = DateTime.Now;
        }

        public void Run()
        {
            Trace.TraceEvent(TraceEventType.Information, 0, "Streamer starting");
            while (true)
            {
                _syncClock.Restart();
                var ok = DoOneFrame();
                var msAhead = FrameDurationMs - (int)_syncClock.ElapsedMilliseconds - 1;
                if (msAhead > 0)
                {
                    Thread.Sleep(msAhead);
                }

                if(!ok)
                {
                    return;
                }
            }
        }

        private bool DoOneFrame()
        {
            var ok = FetchSource();
            if (ok)
            {
                EncodeFrame();
                DistributeFrame();
            }
            return ok;
        }

        public void Stop()
        {
            Trace.TraceEvent(TraceEventType.Information, 0, "Stopping");
            lock (_syncRoot)
            {
                _stopFlag = true;
            }
        }

        private bool FetchSource()
        {
            lock (_syncRoot)
            {
                if (_stopFlag)
                {
                    return false;
                }
            }
            
            _source.FillBuffer(_pcmBuffer);
            return true;
        }

        private void EncodeFrame()
        {
            _encodedBytes = _encoder.Encode(_pcmBuffer, _encodedBuffer);
        }

        private void DistributeFrame()
        {
            var handler = FrameEncoded;
            if (handler != null)
            {
                handler(_encodedBuffer, _encodedBytes, FrameSize/NumChannels);
                _lastPublish = DateTime.Now;
            }
            else
            {
                CheckWhetherToDie();
            }
        }

        private void CheckWhetherToDie()
        {
            if (_lastPublish + IdleTimeToLive < DateTime.Now)
            {
                Trace.TraceEvent(TraceEventType.Information, 0, "Commiting suicide because nobody is listening");
                var handler = TooLongIdle;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }
    }
}