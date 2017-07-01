// <copyright file="AppMetricsMiddlewareHealthChecksOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.AspNetCore.Health.Internal;

namespace App.Metrics.AspNetCore.Health.Options
{
    [ExcludeFromCodeCoverage]
    public class AppMetricsMiddlewareHealthChecksOptions
    {
        public AppMetricsMiddlewareHealthChecksOptions()
        {
            HealthEndpointEnabled = true;
        }

        /// <summary>
        ///     Gets or sets the health endpoint, defaults to /health.
        /// </summary>
        /// <value>
        ///     The health endpoint.
        /// </value>
        public string HealthEndpoint { get; set; } = MiddlewareHealthChecksConstants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [health endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [health endpoint is enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool HealthEndpointEnabled { get; set; }
    }
}