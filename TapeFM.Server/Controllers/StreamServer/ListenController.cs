using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace TapeFM.Server.Controllers.StreamServer
{
    public class ListenController : ApiController
    {
        [Route("listen")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var playlist = string.Format(PlaylistTemplate, GetBaseUri(request));

            var response = request.CreateResponse();
            response.Content = new StringContent(playlist, Encoding.UTF8, "audio/x-scpls");
            return response;
        }

        private const string PlaylistTemplate = @"[playlist]
File1={0}/listen/stream
Title1=102.9 MHz tapeFM
Length1=-1

NumberOfEntries=1
Version=2";

        private static string GetBaseUri(HttpRequestMessage request)
        {
            return request.RequestUri.Scheme + "://" + request.RequestUri.Authority +
                   request.GetRequestContext().VirtualPathRoot.TrimEnd('/');
        }
    }
}