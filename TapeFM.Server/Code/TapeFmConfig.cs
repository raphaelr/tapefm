using Microsoft.Framework.ConfigurationModel;

namespace TapeFM.Server.Code
{
    public static class TapeFmConfig
    {
        public static IConfiguration Configuration { get; private set; }

        public static string LibraryDirectory { get; private set; }
        public static string RedisServer { get; private set; }
        public static int RedisDatabase { get; private set; }

        public static void Load()
        {
            Configuration = new Configuration().AddIniFile("tapefm.ini").AddEnvironmentVariables();
            LibraryDirectory = Configuration.Get("tapefm:LibraryDirectory");
            RedisServer = Configuration.Get("tapefm:RedisServer");
            RedisDatabase = Configuration.Get<int>("tapefm:RedisDatabase");
        }
    }
}