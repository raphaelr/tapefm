using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json.Serialization;
using Owin;
using TapeFM.Server.Code.StreamServer;
using TapeFM.Server.Controllers;

[assembly: OwinStartup(typeof(TapeFM.Server.Code.Startup))]

namespace TapeFM.Server.Code
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseFileServer(new FileServerOptions { FileSystem = new PhysicalFileSystem("../TapeFM.Webapp/public") });
            app.MapSignalR();
            app.UseWebApi(CreateWebApiConfig());

            TapeFmConfig.Load();
            RadioStationManager.CreateNewStation("default");
            RadioStationManager.GetDefault().CurrentSourceChanged += Trackservice.Publish;
        }

        private static HttpConfiguration CreateWebApiConfig()
        {
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
            return config;
        }
    }
}