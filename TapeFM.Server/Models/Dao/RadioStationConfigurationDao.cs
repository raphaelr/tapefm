using TapeFM.Server.Code;

namespace TapeFM.Server.Models.Dao
{
    public static class RadioStationConfigurationDao
    {
        public static void Save(string stationKey, RadioStationConfiguration configuration)
        {
            Database.Save(GetRedisKey(stationKey), configuration);
        }

        public static RadioStationConfiguration Get(string stationKey)
        {
            return Database.Get<RadioStationConfiguration>(GetRedisKey(stationKey));
        }

        private static string GetRedisKey(string stationKey)
        {
            var key = "station_config_" + stationKey;
            return key;
        }
    }
}