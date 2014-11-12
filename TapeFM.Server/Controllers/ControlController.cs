using System.Web.Http;
using TapeFM.Server.Code.StreamServer;
using TapeFM.Server.Models;

namespace TapeFM.Server.Controllers
{
    [RoutePrefix("api/control")]
    public class ControlMessageController : ApiController
    {
        [HttpPost, Route("current_track")]
        public void SetCurrentTrack(Song song)
        {
            var station = RadioStationManager.GetDefault();
            station.Playlist.OverrideNext(song.Path);
            station.Skip();
        }

        [HttpPost, Route("bitrate")]
        public IHttpActionResult SetBitrate(int kbps)
        {
            var station = RadioStationManager.GetDefault();
            if (kbps > 10 && kbps < 1000)
            {
                station.BitrateKbps = kbps;
                return Ok();
            }
            return BadRequest("Invalid bitrate");
        }
    }
}