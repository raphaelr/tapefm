using System;
using System.Threading;
using TapeFM.Server.Models;

namespace TapeFM.Server.Code.StreamServer
{
    public class RadioStation
    {
        private readonly RadioStationSource _source;
        private readonly RadioStationStreamer _streamer;
        private readonly Thread _thread;

        public string Key { get; private set; }
        public Playlist Playlist { get; private set; }

        public event EventHandler<string> CurrentSourceChanged
        {
            add { _source.CurrentSourceChanged += value; }
            remove { _source.CurrentSourceChanged -= value; }
        }

        public event EventHandler TooLongIdle
        {
            add { _streamer.TooLongIdle += value; }
            remove { _streamer.TooLongIdle -= value; }
        }

        public RadioStation(string key)
        {
            Key = key;
            Playlist = new Playlist();
            _source = new RadioStationSource(Playlist);
            _streamer = new RadioStationStreamer(_source);
            _thread = new Thread(_streamer.Run);
        }

        public string CurrentSource
        {
            get { return _source.CurrentSource; }
        }

        public int BitrateKbps
        {
            get { return _streamer.BitrateKbps; }
            set { _streamer.BitrateKbps = value; }
        }

        public void Skip()
        {
            _source.Skip();
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _streamer.Stop();
        }

        public delegate bool DataAvailableCallback(byte[] frame, int length, int sampleSize);
        public void Subscribe(DataAvailableCallback callback)
        {
            var signal = new ManualResetEventSlim();
            byte[] frame = null;
            int length = 0, sampleSize = 0;
            RadioStationStreamer.FrameEncodedCallback handler = (nframe, nlength, nsampleSize) =>
            {
                frame = nframe;
                length = nlength;
                sampleSize = nsampleSize;
                signal.Set();
            };
            _streamer.FrameEncoded += handler;

            try
            {
                var keepGoing = true;
                while (keepGoing)
                {
                    signal.Wait();
                    keepGoing = callback(frame, length, sampleSize);
                    signal.Reset();
                }
            }
            finally
            {
                _streamer.FrameEncoded -= handler;
            }
        }
    }
}