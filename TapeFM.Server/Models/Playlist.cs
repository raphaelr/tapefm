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
                var songs = SongDao.GetAll();
                var next = songs[_rng.Next(songs.Count)];
                result = next.Path;
            }
            return Path.Combine(TapeFmConfig.LibraryDirectory, result);
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