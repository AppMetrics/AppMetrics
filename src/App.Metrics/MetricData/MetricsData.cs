// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Infrastructure;

namespace App.Metrics.MetricData
{
    public sealed class MetricsData
    {
        public static readonly MetricsData Empty = new MetricsData(
            string.Empty,
            DateTime.MinValue,
            EnvironmentInfo.Empty,
            Enumerable.Empty<MetricsDataGroup>());

        public readonly string ContextName;
        public readonly EnvironmentInfo Environment;
        public readonly IEnumerable<MetricsDataGroup> Groups;
        public readonly DateTime Timestamp;

        public MetricsData(string contextName,
            DateTime timestamp,
            EnvironmentInfo environment,
            IEnumerable<MetricsDataGroup> groups)
        {
            ContextName = contextName;
            Timestamp = timestamp;
            Environment = environment;
            Groups = groups;
        }

        public MetricsData Filter(IMetricsFilter filter)
        {
            var groups = Groups.Select(g => g.Filter(filter));


            return new MetricsData(ContextName, Timestamp, Environment, groups);
        }
    }

    public sealed class MetricsDataGroup
    {
        public static readonly MetricsDataGroup Empty = new MetricsDataGroup(string.Empty,
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

        public MetricsDataGroup(string groupName,
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

        public MetricsDataGroup Filter(IMetricsFilter filter)
        {
            if (!filter.IsMatch(GroupName))
            {
                return Empty;
            }

            return new MetricsDataGroup(GroupName,
                Gauges.Where(filter.IsMatch),
                Counters.Where(filter.IsMatch),
                Meters.Where(filter.IsMatch),
                Histograms.Where(filter.IsMatch),
                Timers.Where(filter.IsMatch));
        }
    }
}