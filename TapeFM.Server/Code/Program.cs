using System;
using System.Collections.Generic;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Host.HttpListener;

namespace TapeFM.Server.Code
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var builder = new AppBuilder();
            new Startup().Configuration(builder);

            var uri = TapeFmConfig.Configuration.Get("host:url")
                      ?? "http://localhost:5000/";

            var properties = new Dictionary<string, object>();
            properties["host.Addresses"] = new List<IDictionary<string, object>>
            {
                UriToAddressObject(uri)
            };

            OwinServerFactory.Create(builder.Build(), properties);
            Console.WriteLine("Now listening on {0}", uri);
            Console.WriteLine("Press enter to stop");
            Console.ReadLine();

            return 0;
        }

        private static IDictionary<string, object> UriToAddressObject(string uriString)
        {
            var uri = new Uri(uriString);
            return new Dictionary<string, object>
            {
                {"scheme", uri.Scheme},
                {"host", uri.Host},
                {"port", uri.Port.ToString()},
                {"path", uri.PathAndQuery}
            };
        }
    }
}