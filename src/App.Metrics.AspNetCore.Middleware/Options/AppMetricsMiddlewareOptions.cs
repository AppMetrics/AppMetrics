// <copyright file="AppMetricsMiddlewareOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using App.Metrics.AspNetCore.Middleware.Internal;
using App.Metrics.Internal;

namespace App.Metrics.AspNetCore.Middleware.Options
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    // ReSharper disable CollectionNeverUpdated.Global
    // ReSharper disable MemberCanBePrivate.Global
    [ExcludeFromCodeCoverage]
    public class AppMetricsMiddlewareOptions
    {
        public AppMetricsMiddlewareOptions()
        {
            MetricsEndpointEnabled = true;
            MetricsTextEndpointEnabled = true;
            PingEndpointEnabled = true;
            EnvironmentInfoEndpointEnabled = true;
            OAuth2TrackingEnabled = true;
            ApdexTrackingEnabled = true;
            ApdexTSeconds = ReservoirSamplingConstants.DefaultApdexTSeconds;
            DefaultTrackingEnabled = true;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the [default http metric tracking enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [all available tracking middleware should be registered]; otherwise, <c>false</c>.
        /// </value>
        public bool DefaultTrackingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the [overall web application's apdex should be measured].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [apdex should be measured]; otherwise, <c>false</c>.
        /// </value>
        public bool ApdexTrackingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the
        ///     <see href="https://alhardy.github.io/app-metrics-docs/getting-started/metric-types/apdex.html">apdex t seconds</see>
        /// </summary>
        /// <value>
        ///     The apdex t seconds.
        /// </value>
        public double ApdexTSeconds { get; set; }

        /// <summary>
        ///     Gets or sets the ignored HTTP status codes as a result of a request where metrics should not be measured.
        /// </summary>
        /// <value>
        ///     The ignored HTTP status codes.
        /// </value>
        public IList<int> IgnoredHttpStatusCodes { get; set; } = new List<int>();

        /// <summary>
        ///     Gets or sets the ignored request routes where metrics should not be measured.
        /// </summary>
        /// <value>
        ///     The ignored routes regex patterns.
        /// </value>
        public IList<string> IgnoredRoutesRegexPatterns { get; set; } = new List<string>();

        /// <summary>
        ///     Gets or sets the metrics endpoint, defaults to /metrics.
        /// </summary>
        /// <value>
        ///     The metrics endpoint.
        /// </value>
        public string MetricsEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.MetricsEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics endpoint should be enabled], if disabled endpoint responds with
        ///     404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [metrics endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the metrics text endpoint, defaults to metrics-text.
        /// </summary>
        /// <value>
        ///     The metrics text endpoint.
        /// </value>
        public string MetricsTextEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.MetricsTextEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics text endpoint should be enabled], if disabled endpoint responds
        ///     with 404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [metrics text endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsTextEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [oauth2 client tracking should be enabled], if disabled endpoint responds
        ///     with 404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [o auth2 tracking enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool OAuth2TrackingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the ping endpoint, defaults to /ping.
        /// </summary>
        /// <value>
        ///     The ping endpoint.
        /// </value>
        public string PingEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.PingEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets the environment info endpoint, defaults to /env.
        /// </summary>
        /// <value>
        ///     The environment info endpoint.
        /// </value>
        public string EnvironmentInfoEndpoint { get; set; } = MiddlewareConstants.DefaultRoutePaths.EnvironmentInfoEndpoint.EnsureLeadingSlash();

        /// <summary>
        ///     Gets or sets a value indicating whether [ping endpoint should be enabled], if disabled endpoint responds with 404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [ping endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool PingEndpointEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [environment info endpoint should be enabled], if disabled endpoint
        ///     responds with 404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [environment info endpoint enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool EnvironmentInfoEndpointEnabled { get; set; }
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
        // ReSharper restore CollectionNeverUpdated.Global
        // ReSharper restore MemberCanBePrivate.Global
    }
}