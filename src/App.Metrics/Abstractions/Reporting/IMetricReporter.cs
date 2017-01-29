// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using App.Metrics.Tagging;

namespace App.Metrics.Abstractions.Reporting
{
    public interface IMetricReporter : IHideObjectMembers, IDisposable
    {
        string Name { get; }

        TimeSpan ReportInterval { get; }

        Task<bool> EndAndFlushReportRunAsync(IMetrics metrics);

        void ReportEnvironment(EnvironmentInfo environmentInfo);

        void ReportHealth(
            GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks);

        void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource);

        void StartReportRun(IMetrics metrics);
    }
}