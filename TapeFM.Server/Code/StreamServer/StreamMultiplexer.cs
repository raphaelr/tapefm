using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TapeFM.Server.Code.StreamServer
{
    public class StreamMultiplexer
    {
        private readonly object _syncRoot = new object();
        private readonly HashSet<Stream> _targets;

        public StreamMultiplexer()
        {
            _targets = new HashSet<Stream>();
        }

        public void Subscribe(Stream stream)
        {
            lock (_syncRoot)
            {
                _targets.Add(stream);
            }
        }

        public void Unsubscribe(Stream stream)
        {
            lock (_syncRoot)
            {
                _targets.Remove(stream);
            }
        }

        public int Publish(byte[] data, int length)
        {
            if (length == 0)
            {
                return 0;
            }

            var affectedStreams = 0;
            lock (_syncRoot)
            {
                foreach (var target in _targets.ToArray())
                {
                    try
                    {
                        target.Write(data, 0, length);
                        target.Flush();
                        affectedStreams++;
                    }
                    catch
                    {
                        Unsubscribe(target);
                    }
                }
            }
            return affectedStreams;
        }
    }
}