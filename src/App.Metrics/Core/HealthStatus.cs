// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Core
{
    /// <summary>
    ///     Structure describing the status of executing all the health checks operations.
    /// </summary>
    public struct HealthStatus
    {
        /// <summary>
        ///     Flag indicating whether any checks are registered
        /// </summary>
        public readonly bool HasRegisteredChecks;

        /// <summary>
        ///     Result of each health check operation
        /// </summary>
        public readonly HealthCheck.Result[] Results;

        public HealthStatus(IEnumerable<HealthCheck.Result> results)
        {
            Results = results.ToArray();

            HasRegisteredChecks = Results.Length > 0;
        }

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