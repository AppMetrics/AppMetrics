// <copyright file="MetricsFormatterReadonlyCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Formatters
{
    public static class MetricsFormatterReadonlyCollectionExtensions
    {
        public static IMetricsOutputFormatter GetType<TFormatter>(this IReadOnlyCollection<IMetricsOutputFormatter> formatters)
        {
            return formatters.GetType(typeof(TFormatter));
        }

        public static IMetricsOutputFormatter GetType(this IReadOnlyCollection<IMetricsOutputFormatter> formatters, Type formatterType)
        {
            for (var i = formatters.Count - 1; i >= 0; i--)
            {
                var formatter = formatters.ElementAt(i);
                if (formatter.GetType() == formatterType)
                {
                    return formatter;
                }
            }

            return default;
        }
    }
}