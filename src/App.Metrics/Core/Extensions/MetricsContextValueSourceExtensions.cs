// <copyright file="MetricsContextValueSourceExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core
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