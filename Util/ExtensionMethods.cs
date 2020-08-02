using System;
using System.Collections.Generic;

namespace SEEL.LinguisticProcessor.Util
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Adds specified element to our dictionary<string, int>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void TryAdd(this Dictionary<string, int> d, string key, int val = 1)
        {
            if (!d.ContainsKey(key))
            {
                d.Add(key, val);
            }
            else
                d[key] += val;
        }

        public static void TryAdd(this Dictionary<Word, int> d, Word key, int val = 1)
        {
            if (!d.ContainsKey(key))
            {
                d.Add(key, val);
            }
            else
                d[key] += val;
        }
        public static void AddDic<K, V>(this Dictionary<K, V> d, Dictionary<K, V> other)
        {
            foreach (var kvp in other)
            {
                if (!d.ContainsKey(kvp.Key))
                {
                    d.Add(kvp.Key, kvp.Value);
                }
            }
        }
    }
}
