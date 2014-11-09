using System.IO;
using System.Xml.Linq;

namespace TapeFM.Server.Code
{
    public static class TapeFmConfig
    {
        public static string LibraryDirectory { get; private set; }
        public static string RedisServer { get; private set; }
        public static int RedisDatabase { get; private set; }

        public static void Load()
        {
            var xml = XElement.Parse(File.ReadAllText("tapefm.config"));
            LibraryDirectory = GetString(xml, "libraryDirectory");
            RedisServer = GetString(xml, "redisServer");
            RedisDatabase = GetInt(xml, "redisDatabase") ?? 0;
        }

        private static string GetString(XElement parent, string key)
        {
            var element = parent.Element(key);
            if (element != null)
            {
                return element.Value;
            }

            var attribute = parent.Attribute(key);
            if (attribute != null)
            {
                return attribute.Value;
            }

            return null;
        }

        private static int? GetInt(XElement parent, string key)
        {
            return Utility.IntParseNullable(GetString(parent, key));
        }
    }
}