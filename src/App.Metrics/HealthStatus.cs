// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System.Collections.Generic;
using System.Linq;

namespace App.Metrics
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
        ///     All health checks passed.
        /// </summary>
        public readonly bool IsHealthy;

        /// <summary>
        ///     Result of each health check operation
        /// </summary>
        public readonly HealthCheck.Result[] Results;

        public HealthStatus(IEnumerable<HealthCheck.Result> results)
        {
            Results = results.ToArray();
            IsHealthy = Results.All(r => r.Check.IsHealthy);
            HasRegisteredChecks = Results.Length > 0;
        }
    }
}