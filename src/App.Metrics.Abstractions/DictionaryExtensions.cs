// <copyright file="DictionaryExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics;
using System.Linq;

// ReSharper disable CheckNamespace
namespace System.Collections.Generic
    // ReSharper restore CheckNamespace
{
    public static class DictionaryExtensions
    {
        [DebuggerStepThrough]
        public static void AddIfNotNanOrInfinity(this IDictionary<string, object> values, string key, double value)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                values.Add(key, value);
            }
        }

        [DebuggerStepThrough]
        public static void AddIfPresent(this IDictionary<string, object> values, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                values.Add(key, value);
            }
        }

        [DebuggerStepThrough]
        public static IDictionary<TKey, TValue> MergeDifference<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == null)
            {
                return second;
            }

            if (second == null)
            {
                return first;
            }

            return second.Concat(first).GroupBy(k => k.Key, v => v.Value).ToDictionary(d => d.Key, d => d.First());
        }
    }
}