namespace TapeFM.Server.Code
{
    public interface ICacheEntry<out T>
    {
        T Get();
        void Invalidate();
    }
}