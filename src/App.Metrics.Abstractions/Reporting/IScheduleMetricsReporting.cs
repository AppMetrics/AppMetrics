// <copyright file="IScheduleMetricsReporting.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;

namespace App.Metrics.Reporting
{
    public interface IScheduleMetricsReporting : IDisposable
    {
        void ScheduleAll(CancellationToken cancellationToken = default);

        void ScheduleAll(TimeSpan interval, CancellationToken cancellationToken = default);
    }
}