namespace TapeFM.Server.Code.DataAccess
{
    public interface IDatabaseAdapter
    {
        void Set(string key, string value);
        string Get(string key);
        void Remove(string key);
    }
}