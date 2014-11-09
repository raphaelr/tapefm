using Microsoft.AspNet.SignalR;
using TapeFM.Server.Code;

namespace TapeFM.Server.Controllers
{
    public class Trackservice : Hub
    {
        public static void Publish(object sender, string track)
        {
            track = (track ?? "").Replace(TapeFmConfig.LibraryDirectory, "");
            GlobalHost.ConnectionManager.GetHubContext<Trackservice>()
                .Clients.All.setCurrentTrack(track);
        }
    }
}