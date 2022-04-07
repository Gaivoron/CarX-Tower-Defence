namespace Shared.ObjectPool
{
    public interface IPool<TKey, TObject> where TObject : IObject<TKey>
    {
        TObject Get(TKey key);
    }
    public interface IPool<TObject> where TObject : IObject
    {
        TObject Get();
    }
}