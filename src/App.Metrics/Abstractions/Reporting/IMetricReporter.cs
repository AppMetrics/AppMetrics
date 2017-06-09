// <copyright file="IMetricReporter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Core.Abstractions;
using App.Metrics.Core.Interfaces;

namespace App.Metrics.Abstractions.Reporting
{
    public interface IMetricReporter : IHideObjectMembers, IDisposable
    {
        // ReSharper disable UnusedMemberInSuper.Global
        string Name { get; }
        // ReSharper restore UnusedMemberInSuper.Global

        TimeSpan ReportInterval { get; }

        Task<bool> EndAndFlushReportRunAsync(IMetrics metrics);

        void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource);

        void StartReportRun(IMetrics metrics);
    }
}