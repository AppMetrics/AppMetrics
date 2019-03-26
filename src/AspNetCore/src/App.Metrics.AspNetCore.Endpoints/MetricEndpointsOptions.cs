// <copyright file="MetricEndpointsOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Formatters;
using Microsoft.AspNetCore.Builder;

namespace App.Metrics.AspNetCore.Endpoints
{
    /// <summary>
    ///     Provides programmatic configuration for metrics endpoints in the App Metrics framework.
    /// </summary>
    public class MetricEndpointsOptions
    {
        public MetricEndpointsOptions()
        {
            MetricsEndpointEnabled = true;
            MetricsTextEndpointEnabled = true;
            EnvironmentInfoEndpointEnabled = true;
        }

        /// <summary>
        ///     Gets or sets the <see cref="IEnvOutputFormatter" /> used to write environment information when the env endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IEnvOutputFormatter" /> used to write metrics.
        /// </value>
        public IEnvOutputFormatter EnvInfoEndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [environment info endpoint should be enabled], if disabled endpoint
        ///     responds with 404.
        /// </summary>
        /// <remarks>Only valid if UseEnvInfoEndpoint configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [environment info endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool EnvironmentInfoEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <remarks>Only valid if UseMetricsEndpoints configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [metrics endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics when the metrics endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsEndpointOutputFormatter { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics text endpoint should be enabled], if disabled endpoint responds
        ///     with 404.
        /// </summary>
        /// <remarks>Only valid if UseMetricsEndpoints configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [metrics text endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsTextEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="IMetricsOutputFormatter" /> used to write metrics when the metrics text endpoint is
        ///     requested.
        /// </summary>
        /// <value>
        ///     The <see cref="IMetricsOutputFormatter" /> used to write metrics.
        /// </value>
        public IMetricsOutputFormatter MetricsTextEndpointOutputFormatter { get; set; }

        public IReadOnlyCollection<IMetricsOutputFormatter> MetricsOutputFormatters { get; set; }
    }
}