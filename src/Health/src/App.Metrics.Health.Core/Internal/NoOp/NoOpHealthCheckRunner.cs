// <copyright file="NoOpHealthCheckRunner.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#if !NETSTANDARD1_6

#endif

namespace App.Metrics.Health.Internal.NoOp
{
    [ExcludeFromCodeCoverage]
    public sealed class NoOpHealthCheckRunner : IRunHealthChecks
    {
        /// <inheritdoc />
        public ValueTask<HealthStatus> ReadAsync(CancellationToken cancellationToken = default)
        {
            return new ValueTask<HealthStatus>(new HealthStatus(Enumerable.Empty<HealthCheck.Result>()));
        }

        public Task FormatAsync(Stream output, HealthStatus status, CancellationToken cancellationToken = default)
        {
#if NETSTANDARD1_6
            return Task.CompletedTask;
#else
            return AppMetricsHealthTaskHelper.CompletedTask();
#endif
        }
    }
}