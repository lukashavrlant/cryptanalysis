using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionMethods
{
    public static class Dictionary
    {
        public static Dictionary<K, V> Create<K, V>(K[] key, V[] values)
        {
            var dict = new Dictionary<K, V>();
            int limit = Math.Min(key.Length, values.Length);

            for (int i = 0; i < limit; i++)
                dict[key[i]] = values[i];

            return dict;
        }

        public static Dictionary<K, V> Create<K, V>(List<K> key, List<V> values)
        {
            return Create(key.ToArray(), values.ToArray());
        }

        public static void ForEach<K, V>(this Dictionary<K, V> dic, Action<KeyValuePair<K, V>> action)
        {
            foreach (KeyValuePair<K, V> pair in dic)
                action(pair);
        }

        public static void Adds<K, V>(this Dictionary<K, V> dictionary, params object[] values)
        {
            for (int i = 0; i < values.Length; i += 2)
            {
                dictionary[(K)values[i]] = (V)values[i + 1];
            }
        }

        public static Dictionary<K, V> Create<K, V>(params object[] values)
        {
            Dictionary<K, V> dictionary = new Dictionary<K, V>();
            dictionary.Adds(values);
            return dictionary;
        }

        public static Dictionary<K, V[]> Create<K, V>(V[] array, Func<V, K> sorter)
        {
            var result = new Dictionary<K, List<V>>();

            foreach (var item in array)
            {
                K key = sorter(item);
                if (result.ContainsKey(key))
                    result[key].Add(item);
                else
                    result[key] = new List<V>() { item };
            }

            return result.ToDictionary(x => x.Key, x => x.Value.ToArray());
        }

        public static Dictionary<K, List<V>> SafeAdd<K, V>(this Dictionary<K, List<V>> dictionary, K key, V item)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(item);
            }
            else
            {
                dictionary[key] = new List<V>() { item };
            }

            return dictionary;
        }
    }
}
