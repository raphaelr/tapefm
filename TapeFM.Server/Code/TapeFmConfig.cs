using Microsoft.Framework.ConfigurationModel;

namespace TapeFM.Server.Code
{
    public static class TapeFmConfig
    {
        public static IConfiguration Configuration { get; private set; }

        public static string LibraryDirectory { get; private set; }
        public static string RedisServer { get; private set; }
        public static int RedisDatabase { get; private set; }

        public static void Load(string[] commandLineArgs = null)
        {
            if (Configuration != null) return;
            Configuration = CreateConfiguration(commandLineArgs);

            LibraryDirectory = Configuration.Get("tapefm:LibraryDirectory");
            RedisServer = Configuration.Get("tapefm:RedisServer");
            RedisDatabase = Configuration.GetInt("tapefm:RedisDatabase");
        }

        private static IConfiguration CreateConfiguration(string[] commandLineArgs)
        {
            var config = new Configuration();

            config.AddIniFile("tapefm.ini");
            config.AddEnvironmentVariables();
            if (commandLineArgs != null)
            {
                config.AddCommandLine(commandLineArgs);
            }

            return config;
        }

        private static int GetInt(this IConfiguration config, string key)
        {
            var str = config.Get(key);
            int result;
            int.TryParse(str, out result);
            return result;
        }
    }
}