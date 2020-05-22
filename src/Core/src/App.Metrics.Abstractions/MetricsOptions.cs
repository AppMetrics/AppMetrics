// <copyright file="MetricsOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Registry;

namespace App.Metrics
{
    /// <summary>
    ///     Provides programmatic configuration for the App Metrics framework.
    /// </summary>
    public class MetricsOptions
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
        ///     Gets or sets the contextual tags to apply on all metrics when reporting.
        /// </summary>
        /// <value>
        ///     The contextual tags applied to on all metrics when reporting.
        /// </value>
        public ContextualMetricTagProviders ContextualTags { get; set; } = new ContextualMetricTagProviders();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics tracking enabled]. This will also avoid registering all metric
        ///     tracking middleware if using App.Metrics.Middleware.
        /// </summary>
        /// <remarks>If disabled no metrics will be recorded or stored in memory</remarks>
        /// <value>
        ///     <c>true</c> if [metrics enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether [reporting enabled].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [reporting enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool ReportingEnabled { get; set; } = true;
    }
}