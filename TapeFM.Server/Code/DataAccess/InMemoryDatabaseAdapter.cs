using System.Collections.Concurrent;

namespace TapeFM.Server.Code.DataAccess
{
    public class InMemoryDatabaseAdapter : IDatabaseAdapter
    {
        private readonly ConcurrentDictionary<string, string> _dictionary;

        public InMemoryDatabaseAdapter()
        {
            _dictionary = new ConcurrentDictionary<string, string>();
        }

        public void Set(string key, string value)
        {
            _dictionary.AddOrUpdate(key, value, (k, old) => value);
        }

        public string Get(string key)
        {
            string value;
            _dictionary.TryGetValue(key, out value);
            return value;
        }

        public void Remove(string key)
        {
            string value;
            _dictionary.TryRemove(key, out value);
        }
    }
}