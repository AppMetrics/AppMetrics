// <copyright file="IMetricReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Internal;

namespace App.Metrics.Reporting
{
    public interface IMetricReporter : IHideObjectMembers, IDisposable
    {
        string Name { get; }

        TimeSpan ReportInterval { get; }

        Task<bool> EndAndFlushReportRunAsync(IMetrics metrics);

        void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource);

        void StartReportRun(IMetrics metrics);
    }
}