using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using StackExchange.Redis;

namespace TapeFM.Server.Code
{
    public static class Database
    {
        private static readonly TraceSource Trace = Logger.GetComponent("Database");

        public const string CacheKeySongs = "cached_songs";

        private static readonly IDatabase Redis;

        public static ICacheEntry<T> CreateEntry<T>(string key, Func<T> factory)
            where T : class
        {
            return new CacheEntry<T>(key, factory);
        }

        public static void Save<T>(string key, T value)
        {
            Redis.StringSet(key, Serialize(value));
        }

        public static T Get<T>(string key)
        {
            var value = Redis.StringGet(key);
            return value.IsNull ? default(T) : Deserialize<T>(value);
        }

        public static void Clear()
        {
            Trace.TraceEvent(TraceEventType.Warning, 0, "Clearing Redis cache");
            Redis.KeyDelete(CacheKeySongs, CommandFlags.FireAndForget);
        }

        private static string Serialize<T>(T value)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static T Deserialize<T>(string value)
        {
            var serializer = new DataContractJsonSerializer(typeof (T));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                try
                {
                    return (T) serializer.ReadObject(stream);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        static Database()
        {
            Redis = ConnectionMultiplexer
                .Connect(TapeFmConfig.RedisServer)
                .GetDatabase(TapeFmConfig.RedisDatabase);
        }

        private class CacheEntry<T> : ICacheEntry<T>
            where T : class
        {
            private readonly string _key;
            private readonly Func<T> _factory;

            public CacheEntry(string key, Func<T> factory)
            {
                _key = key;
                _factory = factory;
            }

            public T Get()
            {
                var value = Get<T>(_key);
                if (value == default(T))
                {
                    value = _factory();
                    Redis.StringSet(_key, Serialize(value), flags: CommandFlags.FireAndForget);
                }
                return value;
            }

            public void Invalidate()
            {
                Redis.KeyDelete(_key, CommandFlags.FireAndForget);
            }
        }
    }
}