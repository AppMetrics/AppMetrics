// <copyright file="SampleHealthStatusReporter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;

namespace HealthSandbox
{
    public class SampleHealthStatusReporter : IReportHealthStatus
    {
        /// <inheritdoc />
        public TimeSpan ReportInterval { get; set; }

        /// <inheritdoc />
        public Task ReportAsync(HealthOptions options, HealthStatus status, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"{options.ApplicationName} - Overall status: {status.Status}");

            return Task.CompletedTask;
        }
    }
}