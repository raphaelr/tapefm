using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using TapeFM.Server.Code.DataAccess;

namespace TapeFM.Server.Code
{
    public static class Database
    {
        private static readonly TraceSource Trace = Logger.GetComponent("Database");

        private static readonly IDatabaseAdapter Adapter;

        public const string CacheKeySongs = "cached_songs";

        public static ICacheEntry<T> CreateEntry<T>(string key, Func<T> factory)
            where T : class
        {
            return new CacheEntry<T>(key, factory);
        }

        public static void Save<T>(string key, T value)
        {
            Adapter.Set(key, Serialize(value));
        }

        public static T Get<T>(string key)
        {
            var value = Adapter.Get(key);
            return key == null ? default(T) : Deserialize<T>(value);
        }

        public static void Clear()
        {
            Trace.TraceEvent(TraceEventType.Warning, 0, "Clearing cache");
            Adapter.Remove(CacheKeySongs);
        }

        private static string Serialize<T>(T value)
        {
            return JToken.FromObject(value).ToString();
        }

        private static T Deserialize<T>(string value)
        {
            try
            {
                return JToken.Parse(value).ToObject<T>();
            }
            catch
            {
                return default(T);
            }
        }

        static Database()
        {
            if (!string.IsNullOrEmpty(TapeFmConfig.RedisServer))
            {
                Trace.TraceEvent(TraceEventType.Information, 0, "Using Redis Database");
                Adapter = new RedisDatabaseAdapter();
            }
            else
            {
                Trace.TraceEvent(TraceEventType.Information, 0, "Using In-Memory Database");
                Adapter = new InMemoryDatabaseAdapter();
            }
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
                    Adapter.Set(_key, Serialize(value));
                }
                return value;
            }

            public void Invalidate()
            {
                Adapter.Remove(_key);
            }
        }
    }
}