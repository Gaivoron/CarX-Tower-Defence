using System;

namespace Shared.ObjectPool
{
    public interface IObject
    {
        event Action Released;

        void Init();
        void Release();
    }

    public interface IObject<T> : IObject
    {
        T Key { get; }
    }
}