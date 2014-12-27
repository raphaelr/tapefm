using System.Web.Http;
using TapeFM.Server.Code.StreamServer;
using TapeFM.Server.Models;

namespace TapeFM.Server.Controllers
{
    public class StatusController : ApiController
    {
        public Status Get()
        {
            var station = RadioStationManager.GetDefault();
            return new Status
            {
                CurrentTrack = string.IsNullOrEmpty(station.CurrentSource) ? null : station.CurrentSource,
                IsPaused = station.IsPaused,
                BitrateKbps = station.BitrateKbps,
                EmptyPlaylistMode = station.EmptyPlaylistMode
            };
        }

        public void Post([FromBody] Status status)
        {
            var station = RadioStationManager.GetDefault();
            
            if (!string.IsNullOrEmpty(status.CurrentTrack) && AreTracksDifferent(station.CurrentSource, status.CurrentTrack))
            {
                station.Playlist.OverrideNext(status.CurrentTrack);
                station.Skip();
            }
            
            if (status.BitrateKbps.HasValue && status.BitrateKbps > 10 && status.BitrateKbps < 1000)
            {
                station.BitrateKbps = status.BitrateKbps.Value;
            }

            if (status.IsPaused.HasValue)
            {
                station.IsPaused = status.IsPaused.Value;
            }
            
            if (status.EmptyPlaylistMode.HasValue)
            {
                station.EmptyPlaylistMode = status.EmptyPlaylistMode.Value;
            }
        }

        private static bool AreTracksDifferent(string a, string b)
        {
            a = a.ToLowerInvariant().Trim('/');
            b = b.ToLowerInvariant().Trim('/');
            return a != b;
        }
    }
}