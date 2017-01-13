// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Data
{
    public sealed class MetricsDataValueSource
    {
        public static readonly MetricsDataValueSource Empty = new MetricsDataValueSource(
            DateTime.MinValue,
            EnvironmentInfo.Empty,
            Enumerable.Empty<MetricsContextValueSource>());

        public MetricsDataValueSource(
            DateTime timestamp,
            EnvironmentInfo environment,
            IEnumerable<MetricsContextValueSource> contexts)
        {
            Timestamp = timestamp;
            Environment = environment;
            Contexts = contexts;
        }

        public IEnumerable<MetricsContextValueSource> Contexts { get; }

        public EnvironmentInfo Environment { get; }

        public DateTime Timestamp { get; }

        public MetricsDataValueSource Filter(IMetricsFilter filter)
        {
            var contexts = Contexts.FilterBy(filter).WhereNotEmpty();
            var environment = filter.ReportEnvironment ? Environment : EnvironmentInfo.Empty;

            return new MetricsDataValueSource(Timestamp, environment, contexts);
        }
    }
}