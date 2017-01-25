// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace App.Metrics.Health
{
    /// <summary>
    ///     Posible status values of a health check result
    /// </summary>
    public enum HealthCheckStatus
    {
        /// <summary>
        ///     The check is healthy
        /// </summary>
        Healthy,

        /// <summary>
        ///     The check is degraded, failing but not critical
        /// </summary>
        Degraded,

        /// <summary>
        ///     The check is unhealthy
        /// </summary>
        Unhealthy,

        /// <summary>
        ///     The check was ignored
        /// </summary>
        Ignored
    }
}