// <copyright file="HealthCheckStatus.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Health
{
    /// <summary>
    ///     Possible status values of a health check result
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