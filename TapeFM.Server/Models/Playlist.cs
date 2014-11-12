using System;
using System.Collections.Concurrent;
using System.IO;
using TapeFM.Server.Code;
using TapeFM.Server.Models.Dao;

namespace TapeFM.Server.Models
{
    public class Playlist
    {
        private readonly object _syncRoot = new object();
        private readonly ConcurrentQueue<string> _queue;
        private readonly BetterRandomNumberGenerator _rng;
        private string _nextOverride;
        private string _previous;
        private EmptyPlaylistMode _emptyPlaylistMode;

        public EmptyPlaylistMode EmptyPlaylistMode
        {
            get
            {
                lock (_syncRoot)
                {
                    return _emptyPlaylistMode;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _emptyPlaylistMode = value;
                }
            }
        }

        public Playlist()
        {
            _queue = new ConcurrentQueue<string>();
            _rng = new BetterRandomNumberGenerator();
        }

        public string Next()
        {
            string result = null;
            lock (_syncRoot)
            {
                if (_nextOverride != null)
                {
                    result = _nextOverride;
                    _nextOverride = null;
                }
            }

            if (result == null && !_queue.TryDequeue(out result))
            {
                return DefaultPlaylistEntry();
            }
            
            _previous = result;
            return result;
        }

        private string DefaultPlaylistEntry()
        {
            switch (EmptyPlaylistMode)
            {
                case EmptyPlaylistMode.Silence:
                    return null;
                case EmptyPlaylistMode.RepeatLast:
                    if (string.IsNullOrEmpty(_previous))
                    {
                        _previous = GetRandomSong();
                    }
                    return _previous;
                default:
                    return GetRandomSong();
            }
        }

        private string GetRandomSong()
        {
            var songs = SongDao.GetAll();
            var next = songs[_rng.Next(songs.Count)];
            return next.Path;
        }

        public void OverrideNext(string song)
        {
            lock (_syncRoot)
            {
                _nextOverride = song;
            }
        }
    }
}