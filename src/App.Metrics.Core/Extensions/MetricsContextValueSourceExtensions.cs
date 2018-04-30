// <copyright file="MetricsContextValueSourceExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricsContextValueSourceExtensions
    {
        public static T ValueFor<T>(this IEnumerable<MetricValueSourceBase<T>> values, string metricName)
        {
            var metricValueSources = values as MetricValueSourceBase<T>[] ?? values.ToArray();

            var value = metricValueSources.Where(t => t.Name == metricName).Select(t => t.Value).ToList();

            if (value.Any() && value.Count <= 1)
            {
                return value.Single();
            }

            return default(T);
        }
    }
}