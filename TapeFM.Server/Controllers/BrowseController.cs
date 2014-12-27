using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using TapeFM.Server.Models;
using TapeFM.Server.Models.Dao;

namespace TapeFM.Server.Controllers
{
    public class BrowseController : ApiController
    {
        public List<DirectoryEntry> Get(string dirname)
        {
            if (string.IsNullOrEmpty(dirname) || dirname == "/")
            {
                dirname = "";
            }
            else
            {
                dirname = dirname.Trim('/') + "/";
            }
            
            var numSlashes = dirname.Count(x => x == '/');
            var subtree = SongDao.GetAll().Where(x => x.Path.StartsWith(dirname)).ToList();
            var files = subtree.Where(x => x.Path.Count(p => p == '/') == numSlashes);
            var subdirectoryNames =
                subtree.Select(x => x.Path.Split('/').Skip(numSlashes).ToList())
                    .Where(x => x.Count > 1)
                    .Select(x => x.FirstOrDefault())
                    .Distinct();

            return files.Select(x => new DirectoryEntry {FullPath = x.Path, Name = Path.GetFileName(x.Path)})
                .Concat(
                    subdirectoryNames.Select(
                        x => new DirectoryEntry {FullPath = dirname + x, IsDirectory = true, Name = x}))
                .OrderBy(x => x.Name)
                .ToList();
        }
    }
}