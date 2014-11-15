using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;

[assembly: OwinStartup(typeof(TapeFM.Server.Code.Startup))]

namespace TapeFM.Server.Code
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            TapeFmConfig.Load();

            app.UseFileServer(new FileServerOptions { FileSystem = new PhysicalFileSystem("../TapeFM.Webapp/public") });
            app.MapSignalR();
            app.UseWebApi(CreateWebApiConfig());
        }

        private static HttpConfiguration CreateWebApiConfig()
        {
            var config = new HttpConfiguration();

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.EnsureInitialized();
            return config;
        }
    }
}