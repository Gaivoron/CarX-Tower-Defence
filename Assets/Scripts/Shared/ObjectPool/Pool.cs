using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shared.ObjectPool
{
    /// <summary>
    /// Данный класс реализует объектный пулл.
    /// </summary>
    public class Pool<TKey, TObject> : IPool<TKey, TObject>
        where TObject : IObject<TKey>
    {
        private readonly Func<TKey, IEnumerable<TObject>> _factory;
        private readonly IDictionary<TKey, IPool<TObject>> _pools = new Dictionary<TKey, IPool<TObject>>();

        public Pool(Func<TKey, IEnumerable<TObject>> factory)
        {
            _factory = factory;
        }

        TObject IPool<TKey, TObject>.Get(TKey key)
        {
            if (!_pools.TryGetValue(key, out var pool))
            {
                pool = new Pool<TObject>(() => _factory(key));
                _pools.Add(key, pool);
            }

            return pool.Get();
        }
    }

    public class Pool<TObject> : IPool<TObject>
        where TObject : IObject
    {
        private readonly Func<IEnumerable<TObject>> _factory;
        private readonly Queue<TObject> _available = new Queue<TObject>();
        private readonly IList<TObject> _used = new List<TObject>();

        public Pool(Func<IEnumerable<TObject>> factory)
        {
            _factory = factory;
        }

        TObject IPool<TObject>.Get()
        {
            try
            {
                if (!_available.Any())
                {
                    var batch = Create();
                    foreach (var item in batch)
                    {
                        _available.Enqueue(item);
                    }
                }
            }
            catch (Exception error)
            {
                Debug.LogError(error);
                return default;
            }

            return Get();
        }

        private TObject Get()
        {
            var item = _available.Dequeue();

            item.Released += OnReleased;
            item.Init();
            _used.Add(item);

            return item;

            void OnReleased()
            {
                item.Released -= OnReleased;
                _used.Remove(item);
                _available.Enqueue(item);
            }
        }

        private IEnumerable<TObject> Create()
        {
            var items = _factory();
            if (items == null || !items.Any())
            {
                throw new Exception("No objects to intialize pool!");
            }

            foreach (var item in items)
            {
                item.Release();
            }

            return items;
        }
    }
}