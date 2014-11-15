using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Permissions;
using TapeFM.Server.Controllers;

namespace TapeFM.Server.Code.StreamServer
{
    public static class RadioStationManager
    {
        private static readonly TraceSource Trace = Logger.GetComponent("RadioStationManager");
        private static readonly ConcurrentDictionary<string, RadioStation> Stations;

        static RadioStationManager()
        {
            Stations = new ConcurrentDictionary<string, RadioStation>();
        }

        public static RadioStation GetStation(string key)
        {
            RadioStation station = null;
            if (!string.IsNullOrEmpty(key))
            {
                station = Stations.GetOrAdd(key, CreateNewStation);
            }
            return station;
        }

        public static RadioStation GetDefault()
        {
            return GetStation("default");
        }

        private static RadioStation CreateNewStation(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = Guid.NewGuid().ToString("N");
            }

            Trace.TraceEvent(TraceEventType.Information, 0, "Creating new station with key {0}", key);
            var station = new RadioStation(key);
            station.TooLongIdle += (_, __) => RemoveStation(station);
            station.CurrentSourceChanged += Trackservice.Publish;
            station.Start();
            return station;
        }

        private static void RemoveStation(RadioStation station)
        {
            if (station != null)
            {
                Trace.TraceEvent(TraceEventType.Information, 0, "Removing station with key {0}", station.Key);
                RadioStation tmp;
                if (Stations.TryRemove(station.Key, out tmp))
                {
                    station.Stop();
                }
            }
        }
    }
}