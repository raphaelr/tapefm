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
            var station = GetStation();
            station.Playlist.OverrideNext(song.Path);
            station.Skip();
        }

        [HttpPost, Route("bitrate")]
        public IHttpActionResult SetBitrate(int kbps)
        {
            if (kbps > 10 && kbps < 1000)
            {
                GetStation().BitrateKbps = kbps;
                return Ok();
            }
            return BadRequest("Invalid bitrate");
        }

        [HttpPost, Route("pause")]
        public void SetPaused(bool paused)
        {
            GetStation().IsPaused = paused;
        }

        [HttpPost, Route("empty_playlist_mode")]
        public void SetEmptyPlaylistMode(EmptyPlaylistMode mode)
        {
            GetStation().EmptyPlaylistMode = mode;
        }

        [HttpPost, Route("skip")]
        public void Skip()
        {
            GetStation().Skip();
        }

        private RadioStation GetStation()
        {
            return RadioStationManager.GetDefault();
        }
    }
}