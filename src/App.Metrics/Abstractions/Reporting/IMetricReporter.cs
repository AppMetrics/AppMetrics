// <copyright file="IMetricReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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
        // ReSharper disable UnusedMemberInSuper.Global
        string Name { get; }
        // ReSharper restore UnusedMemberInSuper.Global

        TimeSpan ReportInterval { get; }

        Task<bool> EndAndFlushReportRunAsync(IMetrics metrics);

        void ReportEnvironment(EnvironmentInfo environmentInfo);

        // ReSharper disable UnusedParameter.Global
        void ReportHealth(
            GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks);
        // ReSharper restore UnusedParameter.Global

        void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource);

        void StartReportRun(IMetrics metrics);
    }
}