using System;
using System.Diagnostics;
using System.IO;
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

            LibraryDirectory = CleanPath(Configuration.Get("tapefm:LibraryDirectory"));
            RedisServer = Configuration.Get("tapefm:RedisServer");
            RedisDatabase = Configuration.GetInt("tapefm:RedisDatabase");

            if (!Directory.Exists(LibraryDirectory))
            {
                Logger.GetComponent("TapeFmConfig")
                    .TraceEvent(TraceEventType.Critical, 0, "Specified library directory does not exist: {0}",
                        LibraryDirectory);
                throw new ApplicationException("Fatal error, exiting");
            }
        }

        private static string CleanPath(string path)
        {
            return Path.GetFullPath(path)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                   + Path.DirectorySeparatorChar;
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