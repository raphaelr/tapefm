using Microsoft.AspNet.SignalR;

namespace TapeFM.Server.Controllers
{
    public class StatusUpdates : Hub
    {
        public static void NotifyListeners()
        {
            GlobalHost.ConnectionManager.GetHubContext<StatusUpdates>()
                .Clients.All.update();
        }
    }
}