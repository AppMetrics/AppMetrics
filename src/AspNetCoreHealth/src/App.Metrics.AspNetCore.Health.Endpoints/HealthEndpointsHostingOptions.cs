// <copyright file="HealthEndpointsHostingOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Endpoints.Internal;

namespace App.Metrics.AspNetCore.Health.Endpoints
{
    /// <summary>
    ///     Provides programmatic configuration for metrics endpoints hosting in the App Metrics framework.
    /// </summary>
    public class HealthEndpointsHostingOptions
    {
        /// <summary>
        ///     Gets or sets the port to host available endpoints provided by App Metrics.
        /// </summary>
        /// <remarks>
        ///     This overrides all endpoing specific port configuration allowing a the port to be specific on a single
        ///     setting.
        /// </remarks>
        /// <value>
        ///     The App Metrics available endpoint's port.
        /// </value>
        public int? AllEndpointsPort { get; set; }

        /// <summary>
        ///     Gets or sets the health endpoint, defaults to /health.
        /// </summary>
        /// <value>
        ///     The health endpoint.
        /// </value>
        public string HealthEndpoint { get; set; } = HealthMiddlewareConstants.DefaultRoutePaths.HealthEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the port to host the health endpoint.
        /// </summary>
        public int? HealthEndpointPort { get; set; }

        /// <summary>
        ///     Gets or sets the ping endpoint, defaults to /ping.
        /// </summary>
        /// <value>
        ///     The ping endpoint.
        /// </value>
        public string PingEndpoint { get; set; } = HealthMiddlewareConstants.DefaultRoutePaths.PingEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the port to host the ping endpoint.
        /// </summary>
        /// <value>
        ///     The pint endpoint's port.
        /// </value>
        public int? PingEndpointPort { get; set; }
    }
}