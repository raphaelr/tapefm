using System;
using System.Diagnostics;
using System.Threading;
using TapeFM.Server.Models;
using TapeFM.Server.Models.Dao;

namespace TapeFM.Server.Code.StreamServer
{
    public class RadioStation
    {
        public delegate bool DataAvailableCallback(byte[] frame, int length, int sampleSize);

        private static readonly TraceSource Trace = Logger.GetComponent("RadioStation");

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
                    TraceEvent(TraceEventType.Information, "Setting target bitrate to {0} kbps", value);
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
                    TraceEvent(TraceEventType.Information, "Setting empty playlist mode to {0}", value);
                    SaveConfiguration();
                }
            }
        }

        public bool IsPaused
        {
            get { return _source.IsPaused; }
            set
            {
                if (_source.IsPaused != value)
                {
                    TraceEvent(TraceEventType.Information, "Setting pause status to {0}", value);
                    _source.IsPaused = value;
                }
            }
        }

        public RadioStation(string key)
        {
            Key = key;
            Playlist = new Playlist();
            _source = new RadioStationSource(Playlist);
            _streamer = new RadioStationStreamer(_source);
            _thread = new Thread(_streamer.Run)
            {
                IsBackground = true
            };

            LoadConfiguration();
        }

        public void Skip()
        {
            TraceEvent(TraceEventType.Information, "Skipping current track");
            _source.Skip();
        }

        public void Start()
        {
            TraceEvent(TraceEventType.Information, "Starting");
            _thread.Start();
        }

        public void Stop()
        {
            TraceEvent(TraceEventType.Information, "Stopping");
            _streamer.Stop();
        }

        private void LoadConfiguration()
        {
            TraceEvent(TraceEventType.Information, "Restoring configuration");
            var config = RadioStationConfigurationDao.Get(Key);
            if (config != null)
            {
                BitrateKbps = config.BitrateKbps;
                EmptyPlaylistMode = config.EmptyPlaylistMode;
                TraceEvent(TraceEventType.Verbose, "Restored");
            }
            else
            {
                TraceEvent(TraceEventType.Verbose, "Configuration not found");
            }
            _enableConfigurationSaving = true;
        }

        private void SaveConfiguration()
        {
            if (_enableConfigurationSaving)
            {
                TraceEvent(TraceEventType.Verbose, "Saving configuration");
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

        private void TraceEvent(TraceEventType type, string message, params object[] args)
        {
            if (Trace.Switch.ShouldTrace(type))
            {
                var originalMessage = string.Format(message, args);
                Trace.TraceEvent(type, 0, "[Station {0}] {1}", Key, originalMessage);
            }
        }
    }
}