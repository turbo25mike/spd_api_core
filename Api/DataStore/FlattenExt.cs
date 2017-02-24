using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.DataStore
{
    public static class FlattenExt
    {
        public static string Flatten<TKey, TValue>(this IDictionary<TKey, TValue> d)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));
            return string.Join(",", d.Select(c => $"{c.Key}={GetValue(c.Value)}"));
        }

        public static string FlattenKeys<TKey, TValue>(this IDictionary<TKey, TValue> d)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));
            return string.Join(",", d.Select(c => c.Key.ToString()));
        }

        public static string FlattenValues<TKey, TValue>(this IDictionary<TKey, TValue> d)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));
            return string.Join(",", d.Select(c => GetValue(c.Value)));
        }

        private static string GetValue(object d)
        {
            if (d == null)
                return "NULL";

            var t = d.GetType();

            if (t == typeof(int) || t == typeof(decimal) || t == typeof(long))
                return d.ToString();

            if (t == typeof(DateTime))
                return $"'{((DateTime)d):yyyy-MM-dd HH:mm:ss}'";

            return $"'{d}'";
        }
    }
}
