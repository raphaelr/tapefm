using System.Collections.Generic;
using System.Web.Http;
using TapeFM.Server.Models;
using TapeFM.Server.Models.Dao;

namespace TapeFM.Server.Controllers
{
    public class SongsController : ApiController
    {
        public List<Song> Get()
        {
            return SongDao.GetAll();
        }
    }
}