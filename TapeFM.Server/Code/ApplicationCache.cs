using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using StackExchange.Redis;

namespace TapeFM.Server.Code
{
    public static class ApplicationCache
    {
        private const string KeyAllKeys = "_keys";

        private static readonly IDatabase Redis;

        public static ICacheEntry<T> CreateEntry<T>(string key, Func<T> factory)
        {
            return new DataContractCacheEntry<T>(key, factory);
        }

        public static void Clear()
        {
            var keys = Redis
                .SetMembers(KeyAllKeys)
                .Select(x => (RedisKey)(string)x)
                .ToArray();
            Redis.KeyDelete(keys, CommandFlags.FireAndForget);
        }

        static ApplicationCache()
        {
            Redis = ConnectionMultiplexer
                .Connect(TapeFmConfig.RedisServer)
                .GetDatabase(TapeFmConfig.RedisDatabase);
        }

        private class DataContractCacheEntry<T> : ICacheEntry<T>
        {
            private readonly string _key;
            private readonly Func<T> _factory;
            private readonly DataContractJsonSerializer _serializer;

            public DataContractCacheEntry(string key, Func<T> factory)
            {
                _key = key;
                _factory = factory;
                _serializer = new DataContractJsonSerializer(typeof(T));
                Redis.SetAdd("_keys", key, CommandFlags.FireAndForget);
            }

            public T Get()
            {
                var stringValue = Redis.StringGet(_key);
                T value;
                if (stringValue.IsNull)
                {
                    value = _factory();
                    Redis.StringSet(_key, Serialize(value), flags: CommandFlags.FireAndForget);
                }
                else
                {
                    value = Deserialize(stringValue);
                }
                return value;
            }

            public void Invalidate()
            {
                Redis.KeyDelete(_key, CommandFlags.FireAndForget);
            }

            private string Serialize(T value)
            {
                using (var stream = new MemoryStream())
                {
                    _serializer.WriteObject(stream, value);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }

            private T Deserialize(string value)
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
                {
                    try
                    {
                        return (T) _serializer.ReadObject(stream);
                    }
                    catch
                    {
                        Invalidate();
                        return _factory();
                    }
                }
            }
        }
    }
}