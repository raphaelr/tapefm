using System;
using Microsoft.Owin.Hosting;

namespace TapeFM.Server.Code
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            TapeFmConfig.Load(args);
            var url = TapeFmConfig.Configuration.Get("host:url") ?? "http://localhost:5000/";

            var options = new StartOptions
            {
                ServerFactory = TapeFmConfig.Configuration.Get("host:server") ?? "Microsoft.Owin.Host.HttpListener",
            };
            options.Urls.Add(url);

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Running server on {0}", url);
                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
}