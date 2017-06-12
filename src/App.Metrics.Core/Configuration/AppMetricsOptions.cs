// <copyright file="AppMetricsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Tagging;
using App.Metrics.Registry;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace App.Metrics.Core.Configuration
{
    /// <summary>
    ///     Top level container for all configuration settings of AppMetrics
    /// </summary>
    public sealed class AppMetricsOptions
    {
        private const string DefaultContext = "Application";

        /// <summary>
        ///     Gets or sets the default context label using for grouping metrics in contexts.
        /// </summary>
        /// <remarks>Defaults to "Application"</remarks>
        /// <value>
        ///     The default context label used for grouping metrics within the <see cref="IMetricsRegistry" />.
        /// </value>
        public string DefaultContextLabel { get; set; } = DefaultContext;

        /// <summary>
        ///     Gets or sets the global tags to apply on all metrics when reporting.
        /// </summary>
        /// <value>
        ///     The global tags applied to on all metrics when reporting.
        /// </value>
        public GlobalMetricTags GlobalTags { get; set; } = new GlobalMetricTags();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics tracking enabled]. This will also avoid registering all metric
        ///     tracking middleware if using App.Metrics.Extensions.Middleware.
        /// </summary>
        /// <remarks>If disabled no metrics will be recorded or stored in memory</remarks>
        /// <value>
        ///     <c>true</c> if [metrics enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether [reporting enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reporting enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportingEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not to [add default global tags]. e.g. app, server, env
        /// </summary>
        /// <value>
        /// <c>true</c> if [add default global tags]; otherwise, <c>false</c>.
        /// </value>
        public bool AddDefaultGlobalTags { get; set; } = true;
    }

    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
}