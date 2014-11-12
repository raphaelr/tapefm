using System;
using System.Threading;
using TapeFM.Server.Models;
using TapeFM.Server.Models.Dao;

namespace TapeFM.Server.Code.StreamServer
{
    public class RadioStation
    {
        public delegate bool DataAvailableCallback(byte[] frame, int length, int sampleSize);

        private readonly RadioStationSource _source;
        private readonly RadioStationStreamer _streamer;
        private readonly Thread _thread;
        private bool _enableConfigurationSaving;

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

        public string CurrentSource
        {
            get { return _source.CurrentSource; }
        }

        public int BitrateKbps
        {
            get { return _streamer.BitrateKbps; }
            set
            {
                if (_streamer.BitrateKbps != value)
                {
                    _streamer.BitrateKbps = value;
                    SaveConfiguration();
                }
            }
        }

        public EmptyPlaylistMode EmptyPlaylistMode
        {
            get { return Playlist.EmptyPlaylistMode; }
            set
            {
                if (Playlist.EmptyPlaylistMode != value)
                {
                    Playlist.EmptyPlaylistMode = value;
                    SaveConfiguration();
                }
            }
        }

        public bool IsPaused
        {
            get { return _source.IsPaused; }
            set { _source.IsPaused = value; }
        }

        public RadioStation(string key)
        {
            Key = key;
            Playlist = new Playlist();
            _source = new RadioStationSource(Playlist);
            _streamer = new RadioStationStreamer(_source);
            _thread = new Thread(_streamer.Run);

            LoadConfiguration();
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

        private void LoadConfiguration()
        {
            var config = RadioStationConfigurationDao.Get(Key);
            if (config != null)
            {
                BitrateKbps = config.BitrateKbps;
                EmptyPlaylistMode = config.EmptyPlaylistMode;
            }
            _enableConfigurationSaving = true;
        }

        private void SaveConfiguration()
        {
            if (_enableConfigurationSaving)
            {
                var config = new RadioStationConfiguration
                {
                    BitrateKbps = BitrateKbps,
                    EmptyPlaylistMode = EmptyPlaylistMode
                };
                RadioStationConfigurationDao.Save(Key, config);
            }
        }

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