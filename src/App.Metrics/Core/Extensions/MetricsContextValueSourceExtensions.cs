// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Core.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics.Core
    // ReSharper restore CheckNamespace
{
    public static class MetricsContextValueSourceExtensions
    {
        public static T ValueFor<T>(this IEnumerable<MetricValueSource<T>> values, string context, string metricName)
        {
            var metricValueSources = values as MetricValueSource<T>[] ?? values.ToArray();

            var value = metricValueSources.Where(t => t.Name == metricName).Select(t => t.Value).ToList();

            if (value.Any() && value.Count <= 1)
            {
                return value.Single();
            }

            var availableNames = string.Join(",", metricValueSources.Select(v => v.Name));
            throw new InvalidOperationException($"No metric found with name {metricName} in context {context} Available names: {availableNames}");
        }
    }
}