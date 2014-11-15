using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TapeFM.Server.Code;
using TapeFM.Server.Code.StreamServer;
using TapeFM.Server.Code.StreamServer.Mux;

namespace TapeFM.Server.Controllers.StreamServer
{
    public class ListenStreamController : ApiController
    {
        private static TraceSource Trace = Logger.GetComponent("ListenStreamController");

        [Route("listen/stream")]
        public HttpResponseMessage Get(HttpRequestMessage message)
        {
            var station = RadioStationManager.GetDefault();
            if (station == null)
            {
                return message.CreateErrorResponse(HttpStatusCode.NotFound, "Station not found");
            }

            var content = new PushStreamContent((stream, _, __) =>
            {
                var muxer = new OggMuxer();
                OpusHeaderGenerator.GenerateOpusHeader(muxer, stream);

                ulong absoluteGranulePosition = OpusHeaderGenerator.Preskip;
                RadioStation.DataAvailableCallback handler = (frame, length, sampleSize) =>
                {
                    absoluteGranulePosition += (ulong)sampleSize;
                    muxer.UpdatePage(HeaderType.Default, absoluteGranulePosition, frame, length);
                    try
                    {
                        stream.Write(muxer.OutputBuffer, 0, muxer.OutputBufferLength);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                };
                station.Subscribe(handler);
            });

            content.Headers.ContentType.MediaType = "audio/ogg";
            content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("codec", "opus"));

            var response = message.CreateResponse();
            response.Content = content;
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };

            Trace.TraceEvent(TraceEventType.Information, 0, "Listener with IP {0} connected",
                message.GetOwinContext().Request.RemoteIpAddress);
            return response;
        }
    }
}