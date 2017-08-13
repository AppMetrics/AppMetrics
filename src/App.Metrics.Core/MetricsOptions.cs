// <copyright file="MetricsOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Formatters;
using App.Metrics.Registry;
using App.Metrics.Tagging;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace App.Metrics
{
    /// <summary>
    ///     Provides programmatic configuration for the App Metrics framework.
    /// </summary>
    public class MetricsOptions
    {
        private const string DefaultContext = "Application";

        public MetricsOptions()
        {
            OutputMetricsFormatters = new MetricsFormatterCollection();
            OutputEnvFormatters = new EnvFormatterCollection();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether or not to [add default global tags]. e.g. app, server, env
        /// </summary>
        /// <value>
        ///     <c>true</c> if [add default global tags]; otherwise, <c>false</c>.
        /// </value>
        public bool AddDefaultGlobalTags { get; set; } = true;

        /// <summary>
        ///     Gets or sets the default context label using for grouping metrics in contexts.
        /// </summary>
        /// <remarks>Defaults to "Application"</remarks>
        /// <value>
        ///     The default context label used for grouping metrics within the <see cref="IMetricsRegistry" />.
        /// </value>
        public string DefaultContextLabel { get; set; } = DefaultContext;

        /// <summary>
        ///     Gets or sets the default <see cref="IMetricsOutputFormatter" /> to use when metrics are attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IMetricsOutputFormatter" />s that is used by this application.
        /// </value>
        public IMetricsOutputFormatter DefaultOutputMetricsFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the default <see cref="IEnvOutputFormatter" /> to use when the environment's info is attempted to be formatted.
        /// </summary>
        /// <value>
        ///     The default <see cref="IEnvOutputFormatter" />s that is used by this application.
        /// </value>
        public IEnvOutputFormatter DefaultOutputEnvFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the global tags to apply on all metrics when reporting.
        /// </summary>
        /// <value>
        ///     The global tags applied to on all metrics when reporting.
        /// </value>
        public GlobalMetricTags GlobalTags { get; set; } = new GlobalMetricTags();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics tracking enabled]. This will also avoid registering all metric
        ///     tracking middleware if using App.Metrics.Middleware.
        /// </summary>
        /// <remarks>If disabled no metrics will be recorded or stored in memory</remarks>
        /// <value>
        ///     <c>true</c> if [metrics enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool MetricsEnabled { get; set; } = true;

        /// <summary>
        ///     Gets a list of <see cref="IMetricsOutputFormatter" />s that are used by this application to format metric
        ///     results.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IMetricsOutputFormatter" />s that are used by this application.
        /// </value>
        public MetricsFormatterCollection OutputMetricsFormatters { get; }

        /// <summary>
        ///     Gets a list of <see cref="IEnvOutputFormatter" />s that are used by this application to format environment info.
        /// </summary>
        /// <value>
        ///     A list of <see cref="IEnvOutputFormatter" />s that are used by this application.
        /// </value>
        public EnvFormatterCollection OutputEnvFormatters { get; }
    }

    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
}