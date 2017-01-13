// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Core.Interfaces
{
    /// <summary>
    ///     Provides access to the current health status of the application by executing regsitered <see cref="HealthCheck" />s
    /// </summary>
    public interface IHealthStatusProvider
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