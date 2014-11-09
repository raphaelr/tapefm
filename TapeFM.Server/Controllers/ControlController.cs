using System.Web.Http;
using TapeFM.Server.Code.StreamServer;
using TapeFM.Server.Models;

namespace TapeFM.Server.Controllers
{
    public class ControlController : ApiController
    {
        [HttpPost, ActionName("current_track")]
        public void SetCurrentTrack(Song song)
        {
            var station = RadioStationManager.GetDefault();
            station.Playlist.OverrideNext(song.Path);
            station.Skip();
        }
    }
}