using System.Web.Http;
using TapeFM.Server.Code;

namespace TapeFM.Server.Controllers
{
    public class CacheController : ApiController
    {
        public void Delete()
        {
            Database.Clear();
        }
    }
}