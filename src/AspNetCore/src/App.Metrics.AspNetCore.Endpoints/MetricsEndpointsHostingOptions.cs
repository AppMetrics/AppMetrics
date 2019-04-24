// <copyright file="MetricsEndpointsHostingOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Internal;

namespace App.Metrics.AspNetCore.Endpoints
{
    /// <summary>
    ///     Provides programmatic configuration for metrics endpoints hosting in the App Metrics framework.
    /// </summary>
    public class MetricsEndpointsHostingOptions
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
        ///     Gets or sets the environment info endpoint, defaults to /env.
        /// </summary>
        /// <value>
        ///     The environment info endpoint.
        /// </value>
        public string EnvironmentInfoEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.EnvironmentInfoEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the port to host the env info endpoint.
        /// </summary>
        /// <value>
        ///     The env info endpoint's port.
        /// </value>
        public int? EnvironmentInfoEndpointPort { get; set; }

        /// <summary>
        ///     Gets or sets the metrics endpoint, defaults to /metrics.
        /// </summary>
        /// <value>
        ///     The metrics endpoint.
        /// </value>
        public string MetricsEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.MetricsEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the port to host the metrics endpoint.
        /// </summary>
        /// <value>
        ///     The metrics endpoint's port.
        /// </value>
        public int? MetricsEndpointPort { get; set; }

        /// <summary>
        ///     Gets or sets the metrics text endpoint, defaults to metrics-text.
        /// </summary>
        /// <value>
        ///     The metrics text endpoint.
        /// </value>
        public string MetricsTextEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.MetricsTextEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the port to host the metrics text endpoint.
        /// </summary>
        /// <value>
        ///     The metrics text endpoint's port.
        /// </value>
        public int? MetricsTextEndpointPort { get; set; }
    }
}