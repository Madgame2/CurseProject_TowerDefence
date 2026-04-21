using System.Collections.Generic;
using UnityEngine;

namespace Common.Services.Global
{
    public class GlobalStorage
    {
        private readonly Dictionary<string, object> _data = new();

        public void Set<T>(string key, T value)
        {
            _data[key] = value!;
        }

        public T Get<T>(string key)
        {
            return (T)_data[key];
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (_data.TryGetValue(key, out var obj) && obj is T typed)
            {
                value = typed;
                return true;
            }

            value = default!;
            return false;
        }
    }
}
