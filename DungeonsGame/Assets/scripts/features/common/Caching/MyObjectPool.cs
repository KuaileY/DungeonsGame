
using System;
using System.Collections.Generic;

namespace Entitas
{
    public class MyObjectPool<T>
    {
        Func<T> _factoryMethod;
        Dictionary<string, T> _pool;

        public MyObjectPool()
        {
            _pool = new Dictionary<string, T>();
        }

        public T Get(string name)
        {
            return _pool[name];
        }

        public void Pop(T t,string name)
        {
            _pool.Add(name, t);
        }

    }
}
