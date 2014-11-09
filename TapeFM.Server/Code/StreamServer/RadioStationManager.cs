using System;
using System.Collections.Concurrent;
using System.Linq;

namespace TapeFM.Server.Code.StreamServer
{
    public static class RadioStationManager
    {
        private static readonly TimeSpan IdleTimeToLive = TimeSpan.FromHours(1);
        private static readonly ConcurrentDictionary<string, RadioStation> Stations;

        static RadioStationManager()
        {
            Stations = new ConcurrentDictionary<string, RadioStation>();
        }

        public static string CreateNewStation(string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString("N");
            }

            var station = new RadioStation();
            station.Start();
            Stations[key] = station;
            return key;
        }

        public static RadioStation GetStation(string key)
        {
            RadioStation station = null;
            if (!string.IsNullOrEmpty(key))
            {
                Stations.TryGetValue(key, out station);
            }
            return station;
        }

        public static RadioStation GetDefault()
        {
            return GetStation("default");
        }

        public static void ReapDeadStations()
        {
            var dead = Stations.ToArray()
                .Where(s => s.Value.LastPublish < DateTime.Now.Subtract(IdleTimeToLive));
            foreach (var station in dead)
            {
                station.Value.Stop();
                RadioStation value;
                Stations.TryRemove(station.Key, out value);
            }
        }
    }
}