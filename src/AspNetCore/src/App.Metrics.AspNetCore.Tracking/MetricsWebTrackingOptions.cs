// <copyright file="MetricsWebTrackingOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;

namespace App.Metrics.AspNetCore.Tracking
{
    /// <summary>
    ///     Provides programmatic configuration for metrics tracking middleware in the App Metrics framework.
    /// </summary>
    public class MetricsWebTrackingOptions
    {
        public MetricsWebTrackingOptions()
        {
            OAuth2TrackingEnabled = true;
            ApdexTrackingEnabled = true;
            ApdexTSeconds = AppMetricsReservoirSamplingConstants.DefaultApdexTSeconds;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the [overall web application's apdex should be measured].
        /// </summary>
        /// <remarks>Only valid if UseMetricsApdexTrackingMiddleware configured on the <see cref="IApplicationBuilder" />.</remarks>
        /// <value>
        ///     <c>true</c> if [apdex should be measured]; otherwise, <c>false</c>.
        /// </value>
        public bool ApdexTrackingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets the
        ///     <see href="https://alhardy.github.io/app-metrics-docs/getting-started/metric-types/apdex.html">apdex t seconds</see>
        /// </summary>
        /// <remarks>Only valid if UseMetricsApdexTrackingMiddleware configured on the <see cref="IApplicationBuilder" />.</remarks>
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

        private IgnoredRoutesConfiguration _ignoredRoutesConfiguration = new IgnoredRoutesConfiguration();

        /// <summary>
        ///     Gets the ignored request routes where metrics should not be measured.
        /// </summary>
        /// <value>
        ///     The ignored routes regex patterns.
        /// </value>
        public IReadOnlyList<Regex> IgnoredRoutesRegex => _ignoredRoutesConfiguration.RegexPatterns;

        /// <summary>
        ///     Gets the ignored request ports where metrics should not be measured.
        /// </summary>
        /// <value>
        ///     The ignored ports.
        /// </value>
        public IReadOnlyList<int> IgnoredPorts { get; set; } = new List<int>();

        /// <summary>
        ///     Gets or sets the ignored request routes where metrics should not be measured.
        /// </summary>
        /// <value>
        ///     The ignored routes regex patterns.
        /// </value>
        public IList<string> IgnoredRoutesRegexPatterns
        {
            get => _ignoredRoutesConfiguration;
            set => _ignoredRoutesConfiguration = new IgnoredRoutesConfiguration(value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [oauth2 client tracking should be enabled], if disabled endpoint responds
        ///     with 404.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [o auth2 tracking enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool OAuth2TrackingEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [bucket histograms should be used], if disabled histograms will be used.
        /// </summary>
        /// <value>
        ///     <c>true</c> if [bucket histograms should be used]; otherwise, <c>false</c>.
        /// </value>
        public bool UseBucketHistograms { get; set; }


        /// <summary>
        ///     Gets or sets the buckets to use for request size bucket histograms
        /// </summary>
        /// <remarks>Only valid if UseBucketHistograms is true.</remarks>
        /// <value>
        ///     Array of buckets
        /// </value>
        public double[] RequestSizeHistogramBuckets { get; set; }


        /// <summary>
        ///     Gets or sets the buckets to use for request time bucket timer
        /// </summary>
        /// <remarks>Only valid if UseBucketHistograms is true.</remarks>
        /// <value>
        ///     Array of buckets
        /// </value>
        public double[] RequestTimeHistogramBuckets { get; set; }
    }
}