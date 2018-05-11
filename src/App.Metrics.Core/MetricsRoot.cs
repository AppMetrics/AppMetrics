// <copyright file="MetricsRoot.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Infrastructure;
using App.Metrics.Reporting;

namespace App.Metrics
{
    internal class MetricsRoot : IMetricsRoot
    {
        private readonly EnvironmentInfoProvider _environmentInfoProvider;
        private readonly IMetrics _metrics;

        public MetricsRoot(
            IMetrics metrics,
            MetricsOptions options,
            MetricsFormatterCollection metricsOutputFormatters,
            EnvFormatterCollection envOutputFormatters,
            IMetricsOutputFormatter defaultMetricsOutputFormatter,
            IEnvOutputFormatter defaultEnvOutputFormatter,
            EnvironmentInfoProvider environmentInfoProvider,
            MetricsReporterCollection reporterCollection,
            IRunMetricsReports reporter)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            ReportRunner = reporter ?? throw new ArgumentNullException(nameof(reporter));
            _environmentInfoProvider = new EnvironmentInfoProvider();
            Reporters = reporterCollection ?? new MetricsReporterCollection();
            OutputMetricsFormatters = metricsOutputFormatters ?? new MetricsFormatterCollection();
            OutputEnvFormatters = envOutputFormatters ?? new EnvFormatterCollection();
            DefaultOutputMetricsFormatter = defaultMetricsOutputFormatter;
            DefaultOutputEnvFormatter = defaultEnvOutputFormatter;
            _environmentInfoProvider = environmentInfoProvider;
        }

        /// <inheritdoc />
        public IBuildMetrics Build => _metrics.Build;

        /// <inheritdoc />
        public IClock Clock => _metrics.Clock;

        /// <inheritdoc />
        public IFilterMetrics Filter => _metrics.Filter;

        /// <inheritdoc />
        public IManageMetrics Manage => _metrics.Manage;

        /// <inheritdoc />
        public IMeasureMetrics Measure => _metrics.Measure;

        /// <inheritdoc />
        public IProvideMetrics Provider => _metrics.Provider;

        /// <inheritdoc />
        public IProvideMetricValues Snapshot => _metrics.Snapshot;

        /// <inheritdoc />
        public IReadOnlyCollection<IMetricsOutputFormatter> OutputMetricsFormatters { get; }

        /// <inheritdoc />
        public IMetricsOutputFormatter DefaultOutputMetricsFormatter { get; }

        /// <inheritdoc />
        public IEnvOutputFormatter DefaultOutputEnvFormatter { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<IEnvOutputFormatter> OutputEnvFormatters { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<IReportMetrics> Reporters { get; }

        /// <inheritdoc />
        public IRunMetricsReports ReportRunner { get; }

        /// <inheritdoc />
        public MetricsOptions Options { get; }

        /// <inheritdoc />
        public EnvironmentInfo EnvironmentInfo => _environmentInfoProvider.Build();
    }
}