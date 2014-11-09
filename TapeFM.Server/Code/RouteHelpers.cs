using System.Web;
using System.Web.Routing;

namespace TapeFM.Server.Code
{
    public static class RouteHelpers
    {
        public static void MapHttpHandler<THandler>(this RouteCollection routes,
            string name, string url, object defaults = null, object constraints = null)
            where THandler : IHttpHandler, new()
        {
            var route = new Route(url, new HttpHandlerRouteHandler<THandler>())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };
            routes.Add(name, route);
        }

        private class HttpHandlerRouteHandler<THttpHandler> : IRouteHandler
            where THttpHandler : IHttpHandler, new()
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new THttpHandler();
            }
        }
    }
}