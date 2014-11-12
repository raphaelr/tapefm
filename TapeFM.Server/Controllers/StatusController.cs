using System.Web.Http;
using TapeFM.Server.Code;
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
                IsPaused = false,
                BitrateKbps = station.BitrateKbps,
                EmptyPlaylistMode = station.EmptyPlaylistMode
            };
        }
    }
}