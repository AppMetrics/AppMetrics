// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using App.Metrics.Internal.Interfaces;
using App.Metrics.Sampling.Interfaces;

namespace App.Metrics.Configuration
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
        ///     Gets or sets the default sampling size of all histogram <see cref="IReservoir">reservoirs</see>
        /// </summary>
        /// <value>
        ///     The  default sampling size of all histogram reservoirs. Defaults to 1028.
        /// </value>
        public int DefaultSamplingSize { get; set; } = 1028;

        /// <summary>
        ///     Gets or sets The default <see cref="SamplingType" /> to use to generate the resevoir of values on
        ///     <see cref="MetricType" /> which require sampling.
        /// </summary>
        /// <remarks>
        ///     This sampling type is applied when there is no sampling type provided on a supported metric being recorded.
        ///     Defaults to <see cref="SamplingType.ExponentiallyDecaying" />
        /// </remarks>
        /// <value>
        ///     The default type of the sampling to use.
        /// </value>
        public SamplingType DefaultSamplingType { get; set; } = SamplingType.ExponentiallyDecaying;

        /// <summary>
        ///     Gets or sets the global tags to apply on all metrics when reporting.
        /// </summary>
        /// <value>
        ///     The global tags applied to on all metrics when reporting.
        /// </value>
        public Dictionary<string, string> GlobalTags { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///     Gets or sets a value indicating whether [metrics enabled].
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
    }
}