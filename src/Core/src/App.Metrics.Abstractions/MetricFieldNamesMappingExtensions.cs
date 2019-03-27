// <copyright file="MetricFieldNamesMappingExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics;

// ReSharper disable CheckNamespace
namespace System.Collections.Generic
    // ReSharper restore CheckNamespace
{
    public static class MetricFieldNamesMappingExtensions
    {
        [DebuggerStepThrough]
        public static void TryAddValuesForKeyIfNotNanOrInfinity<TKey>(this IDictionary<TKey, string> fieldMapping, IDictionary<string, object> values, TKey key, double value)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value) && fieldMapping.ContainsKey(key))
            {
                values.Add(fieldMapping[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKeyIfPresent<TKey>(this IDictionary<TKey, string> fieldMapping, IDictionary<string, object> values, TKey key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && fieldMapping.ContainsKey(key))
            {
                values.Add(fieldMapping[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKey<TKey>(this IDictionary<TKey, string> fieldMapping, IDictionary<string, object> values, TKey key, int value)
        {
            if (fieldMapping.ContainsKey(key))
            {
                values.Add(fieldMapping[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKey<TKey>(this IDictionary<TKey, string> fieldMapping, IDictionary<string, object> values, TKey key, long value)
        {
            if (fieldMapping.ContainsKey(key))
            {
                values.Add(fieldMapping[key], value);
            }
        }

        [DebuggerStepThrough]
        public static void TryAddValuesForKey<TKey>(this IDictionary<TKey, string> fieldMapping, IDictionary<string, object> values, TKey key, double value)
        {
            if (fieldMapping.ContainsKey(key))
            {
                values.Add(fieldMapping[key], value);
            }
        }
    }
}