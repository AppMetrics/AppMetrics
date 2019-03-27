// <copyright file="IReportHealthStatus.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    public interface IReportHealthStatus
    {
        /// <summary>
        ///     Gets <see cref="TimeSpan" /> interval to flush metrics values. Defaults to
        ///     <see cref="HealthConstants.Reporting.DefaultReportInterval" />.
        /// </summary>
        TimeSpan ReportInterval { get; set; }

        Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default);
    }
}