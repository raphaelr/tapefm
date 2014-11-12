using Microsoft.AspNet.SignalR;

namespace TapeFM.Server.Controllers
{
    public class Trackservice : Hub
    {
        public static void Publish(object sender, string track)
        {
            track = track ?? "Dead Air";
            GlobalHost.ConnectionManager.GetHubContext<Trackservice>()
                .Clients.All.setCurrentTrack(track);
        }
    }
}