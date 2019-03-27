// <copyright file="IRunHealthChecks.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health
{
    /// <summary>
    ///     Provides access to the current health status of the application by executing registered <see cref="HealthCheck" />s
    /// </summary>
    public interface IRunHealthChecks
    {
        /// <summary>
        ///     Executes all registered health checks within the application
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     The current health status of the application. A single health check failure will result in an un-healthy
        ///     result
        /// </returns>
        ValueTask<HealthStatus> ReadAsync(CancellationToken cancellationToken = default);
    }
}