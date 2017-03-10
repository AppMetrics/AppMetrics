// <copyright file="IProvideHealth.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Health.Abstractions
{
    /// <summary>
    ///     Provides access to the current health status of the application by executing regsitered <see cref="HealthCheck" />s
    /// </summary>
    public interface IProvideHealth
    {
        /// <summary>
        ///     Executes all regsitered health checks within the application
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     The current health status of the application. A single health check failure will result in an un-healthy
        ///     result
        /// </returns>
        Task<HealthStatus> ReadStatusAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}