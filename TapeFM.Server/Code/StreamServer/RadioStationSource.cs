using System;
using TapeFM.Server.Models;

namespace TapeFM.Server.Code.StreamServer
{
    public class RadioStationSource
    {
        private readonly object _syncRoot;
        private readonly Decoder _decoder;
        private readonly Playlist _playlist;
        private bool _isEof, _isSilence;

        public event EventHandler<string> CurrentSourceChanged;

        public string CurrentSource { get; private set; }
        public bool IsPaused { get; set; }

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
                if (IsPaused || _isSilence)
                {
                    BufferHelper.ZeroBuffer(sourceBuffer, 0);
                }
                else
                {
                    FillBufferFromStream(sourceBuffer);
                }
            }
        }

        private void FillBufferFromStream(byte[] sourceBuffer)
        {
            var result = _decoder.Stream.ReadFull(sourceBuffer);
            if (!result)
            {
                _isEof = true;
            }
        }

        public void Skip()
        {
            lock (_syncRoot)
            {
                _decoder.Stop();
                _isSilence = false;
                _isEof = true;
            }
        }

        private void EnsureStream()
        {
            if (_isEof)
            {
                var next = _playlist.Next();
                _isEof = false;
                OnCurrentSourceChanged(next);

                _isSilence = next == null;
                if (next != null)
                {
                    _decoder.Decode(next);
                }
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