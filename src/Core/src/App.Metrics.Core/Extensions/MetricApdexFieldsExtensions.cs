// <copyright file="MetricApdexFieldsExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricApdexFieldsExtensions
    {
        public static void Exclude(this IDictionary<ApdexFields, string> metricFields, params ApdexFields[] fields)
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

        public static void OnlyInclude(this IDictionary<ApdexFields, string> metricFields, params ApdexFields[] fields)
        {
            var apdex = new Dictionary<ApdexFields, string>(metricFields);

            foreach (var key in apdex.Keys)
            {
                if (!fields.Contains(key))
                {
                    metricFields.Remove(key);
                }
            }
        }

        public static void Set(this IDictionary<ApdexFields, string> metricFields, ApdexFields field, string value)
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