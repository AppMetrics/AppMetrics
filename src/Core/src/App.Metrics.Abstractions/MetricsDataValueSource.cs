// <copyright file="MetricsDataValueSource.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Filters;

namespace App.Metrics
{
    public sealed class MetricsDataValueSource
    {
        public static readonly MetricsDataValueSource Empty = new MetricsDataValueSource(
            DateTime.MinValue,
            Enumerable.Empty<MetricsContextValueSource>());

        public MetricsDataValueSource(
            DateTime timestamp,
            IEnumerable<MetricsContextValueSource> contexts)
        {
            Timestamp = timestamp;
            Contexts = contexts;
        }

        public IEnumerable<MetricsContextValueSource> Contexts { get; }

        public DateTime Timestamp { get; }

        public MetricsDataValueSource Filter(IFilterMetrics filter)
        {
            var contexts = Contexts.FilterBy(filter).WhereNotEmpty();

            return new MetricsDataValueSource(Timestamp, contexts);
        }
    }
}