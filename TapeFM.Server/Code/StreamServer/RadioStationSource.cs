using System;
using TapeFM.Server.Models;

namespace TapeFM.Server.Code.StreamServer
{
    public class RadioStationSource
    {
        private readonly object _syncRoot;
        private readonly Decoder _decoder;
        private readonly Playlist _playlist;
        private bool _isEof;

        public event EventHandler<string> CurrentSourceChanged;

        public string CurrentSource { get; private set; }

        public RadioStationSource(Playlist playlist)
        {
            _syncRoot = new object();
            _decoder = new Decoder();
            _playlist = playlist;
            _isEof = true;
        }

        public void FillBuffer(byte[] sourceBuffer)
        {
            lock (_syncRoot)
            {
                EnsureStream();

                var result = _decoder.Stream.ReadFull(sourceBuffer);
                if(!result)
                {
                    _isEof = true;
                }
            }
        }

        public void Skip()
        {
            lock (_syncRoot)
            {
                _decoder.Stop();
                _isEof = true;
            }
        }

        private void EnsureStream()
        {
            if (_isEof)
            {
                var next = _playlist.Next();
                _decoder.Decode(next);
                _isEof = false;
                OnCurrentSourceChanged(next);
            }
        }

        protected virtual void OnCurrentSourceChanged(string e)
        {
            CurrentSource = e;
            var handler = CurrentSourceChanged;
            if (handler != null) handler(this, e);
        }
    }
}