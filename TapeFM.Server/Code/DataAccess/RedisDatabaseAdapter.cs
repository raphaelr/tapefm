using StackExchange.Redis;

namespace TapeFM.Server.Code.DataAccess
{
    public class RedisDatabaseAdapter : IDatabaseAdapter
    {
        private readonly IDatabase _redis;

        public RedisDatabaseAdapter()
        {
            _redis = ConnectionMultiplexer
                .Connect(TapeFmConfig.RedisServer)
                .GetDatabase(TapeFmConfig.RedisDatabase);
        }

        public void Set(string key, string value)
        {
            _redis.StringSet(key, value, flags: CommandFlags.FireAndForget);
        }

        public string Get(string key)
        {
            return _redis.StringGet(key);
        }

        public void Remove(string key)
        {
            _redis.KeyDelete(key, CommandFlags.FireAndForget);
        }
    }
}