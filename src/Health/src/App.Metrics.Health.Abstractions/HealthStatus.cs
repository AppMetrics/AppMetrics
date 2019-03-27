// <copyright file="HealthStatus.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Health
{
    /// <summary>
    ///     Structure describing the status of executing all the health checks operations.
    /// </summary>
    public struct HealthStatus
    {
        public HealthStatus(IEnumerable<HealthCheck.Result> results)
        {
            Results = results;
        }

        /// <summary>
        ///     Gets result of each health check operation
        /// </summary>
        /// <value>
        ///     The health check results.
        /// </value>
        public IEnumerable<HealthCheck.Result> Results { get; }

        /// <summary>
        ///     Gets all health checks passed.
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        public HealthCheckStatus Status
        {
            get
            {
                if (Results.Any(r => r.Check.Status.IsUnhealthy()))
                {
                    return HealthCheckStatus.Unhealthy;
                }

                return Results.Any(r => r.Check.Status.IsDegraded())
                    ? HealthCheckStatus.Degraded
                    : HealthCheckStatus.Healthy;
            }
        }
    }
}