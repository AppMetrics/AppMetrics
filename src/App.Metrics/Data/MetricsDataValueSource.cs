// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Infrastructure;

namespace App.Metrics.Data
{
    public sealed class MetricsDataValueSource
    {
        public static readonly MetricsDataValueSource Empty = new MetricsDataValueSource(
            string.Empty,
            DateTime.MinValue,
            EnvironmentInfo.Empty,
            Enumerable.Empty<MetricsDataGroupValueSource>());

        public readonly string ContextName;
        public readonly EnvironmentInfo Environment;
        public readonly IEnumerable<MetricsDataGroupValueSource> Groups;
        public readonly DateTime Timestamp;

        public MetricsDataValueSource(string contextName,
            DateTime timestamp,
            EnvironmentInfo environment,
            IEnumerable<MetricsDataGroupValueSource> groups)
        {
            ContextName = contextName;
            Timestamp = timestamp;
            Environment = environment;
            Groups = groups;
        }

        public MetricsDataValueSource Filter(IMetricsFilter filter)
        {
            var groups = Groups.Select(g => g.Filter(filter));
            var environment = filter.ReportEnvironment ? EnvironmentInfo.Empty : Environment;

            return new MetricsDataValueSource(ContextName, Timestamp, environment, groups);
        }
    }

    public sealed class MetricsDataGroupValueSource
    {
        public static readonly MetricsDataGroupValueSource Empty = new MetricsDataGroupValueSource(string.Empty,
            Enumerable.Empty<GaugeValueSource>(),
            Enumerable.Empty<CounterValueSource>(),
            Enumerable.Empty<MeterValueSource>(),
            Enumerable.Empty<HistogramValueSource>(),
            Enumerable.Empty<TimerValueSource>());

        public readonly IEnumerable<CounterValueSource> Counters;
        public readonly IEnumerable<GaugeValueSource> Gauges;

        public readonly string GroupName;
        public readonly IEnumerable<HistogramValueSource> Histograms;
        public readonly IEnumerable<MeterValueSource> Meters;
        public readonly IEnumerable<TimerValueSource> Timers;

        public MetricsDataGroupValueSource(string groupName,
            IEnumerable<GaugeValueSource> gauges,
            IEnumerable<CounterValueSource> counters,
            IEnumerable<MeterValueSource> meters,
            IEnumerable<HistogramValueSource> histograms,
            IEnumerable<TimerValueSource> timers)
        {
            GroupName = groupName;
            Gauges = gauges;
            Counters = counters;
            Meters = meters;
            Histograms = histograms;
            Timers = timers;
        }

        public MetricsDataGroupValueSource Filter(IMetricsFilter filter)
        {
            if (!filter.IsMatch(GroupName))
            {
                return Empty;
            }

            return new MetricsDataGroupValueSource(GroupName,
                Gauges.Where(filter.IsMatch),
                Counters.Where(filter.IsMatch),
                Meters.Where(filter.IsMatch),
                Histograms.Where(filter.IsMatch),
                Timers.Where(filter.IsMatch));
        }
    }
}