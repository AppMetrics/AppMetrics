// <copyright file="MetricHistogramFieldsExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricHistogramFieldsExtensions
    {
        public static void Exclude(this IDictionary<HistogramFields, string> metricFields, params HistogramFields[] fields)
        {
            if (!fields.Any())
            {
                metricFields.Clear();

                return;
            }

            foreach (var key in fields)
            {
                if (metricFields.ContainsKey(key))
                {
                    metricFields.Remove(key);
                }
            }
        }

        public static void OnlyInclude(this IDictionary<HistogramFields, string> metricFields, params HistogramFields[] fields)
        {
            var histogram = new Dictionary<HistogramFields, string>(metricFields);

            foreach (var key in histogram.Keys)
            {
                if (!fields.Contains(key))
                {
                    metricFields.Remove(key);
                }
            }
        }

        public static void Set(this IDictionary<HistogramFields, string> metricFields, HistogramFields field, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("metric field name cannot be null or empty", nameof(value));
            }

            if (metricFields.ContainsKey(field))
            {
                metricFields[field] = value;
            }
        }
    }
}