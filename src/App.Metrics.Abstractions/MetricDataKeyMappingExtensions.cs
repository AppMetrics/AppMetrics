// <copyright file="MetricDataKeyMappingExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics;

// ReSharper disable CheckNamespace
namespace System.Collections.Generic
    // ReSharper restore CheckNamespace
{
    public static class MetricDataKeyMappingExtensions
    {
        [DebuggerStepThrough]
        public static void TryAddValuesForKeyIfNotNanOrInfinity<TKey>(this IDictionary<TKey, string> dataKeys, IDictionary<string, object> values, TKey key, double value)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value) && dataKeys.ContainsKey(key))
            {
                values.Add(dataKeys[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKeyIfPresent<TKey>(this IDictionary<TKey, string> dataKeys, IDictionary<string, object> values, TKey key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && dataKeys.ContainsKey(key))
            {
                values.Add(dataKeys[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKey<TKey>(this IDictionary<TKey, string> dataKeys, IDictionary<string, object> values, TKey key, int value)
        {
            if (dataKeys.ContainsKey(key))
            {
                values.Add(dataKeys[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKey<TKey>(this IDictionary<TKey, string> dataKeys, IDictionary<string, object> values, TKey key, long value)
        {
            if (dataKeys.ContainsKey(key))
            {
                values.Add(dataKeys[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKey<TKey>(this IDictionary<TKey, string> dataKeys, IDictionary<string, object> values, TKey key, double value)
        {
            if (dataKeys.ContainsKey(key))
            {
                values.Add(dataKeys[key], value);
            }
        }
    }
}