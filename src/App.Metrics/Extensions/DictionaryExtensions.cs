// <copyright file="DictionaryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace

using System.Diagnostics;

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
            if (value.IsPresent())
            {
                values.Add(key, value);
            }
        }
    }
}