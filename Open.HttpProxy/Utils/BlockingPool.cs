﻿using System;
using System.Collections.Generic;

namespace Open.Tcp.Utils
{
    class BlockingPool<T>
    {
        private readonly Func<T> _factory;
        private readonly Queue<T> _pool;

        public BlockingPool(Func<T> factory)
        {
            _factory = factory;
            _pool = new Queue<T>();
        }

        public int Count
        {
            get { return _pool.Count; }
        }

        public void Add(T item)
        {
            Guard.NotNull(item, "item");

            lock (_pool)
            {
                _pool.Enqueue(item);
            }
        }

        public T Take()
        {
            lock (_pool)
            {
                return _pool.Count > 0 ? _pool.Dequeue() : _factory();
            }
        }

        internal void PreAllocate(int initialCount)
        {
            for (int i = 0; i < initialCount; i++)
            {
                Add(_factory());
            }
        }
    }
}

