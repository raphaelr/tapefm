using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using TapeFM.Server.Code.StreamServer;

namespace TapeFM.Server.Controllers
{
    public class Trackservice : Hub
    {
        public override Task OnConnected()
        {
            Clients.Caller.setCurrentTrack(RadioStationManager.GetDefault().CurrentSource);
            return Task.FromResult(true);
        }

        public static void Publish(object sender, string track)
        {
            GlobalHost.ConnectionManager.GetHubContext<Trackservice>()
                .Clients.All.setCurrentTrack(track);
        }
    }
}