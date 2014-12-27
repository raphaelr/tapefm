using System.Web.Http;
using TapeFM.Server.Code.StreamServer;

namespace TapeFM.Server.Controllers.StreamServer
{
    public class SkipController : ApiController
    {
        public void Post()
        {
            RadioStationManager.GetDefault().Skip();
        }
    }
}
