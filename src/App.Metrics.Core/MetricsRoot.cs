// <copyright file="MetricsRoot.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Infrastructure;
using App.Metrics.Reporting;

namespace App.Metrics
{
    internal class MetricsRoot : IMetricsRoot, IMetrics
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
            IRunMetricsReports reporter,
            IScheduleMetricsReporting reportScheduler)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            Reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
            ReportScheduler = reportScheduler ?? throw new ArgumentNullException(nameof(reportScheduler));
            _environmentInfoProvider = new EnvironmentInfoProvider();
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
        public MetricsFormatterCollection OutputMetricsFormatters { get; }

        /// <inheritdoc />
        public IMetricsOutputFormatter DefaultOutputMetricsFormatter { get; }

        /// <inheritdoc />
        public IEnvOutputFormatter DefaultOutputEnvFormatter { get; }

        /// <inheritdoc />
        public EnvFormatterCollection OutputEnvFormatters { get; }

        /// <inheritdoc />
        public IRunMetricsReports Reporter { get; }

        /// <inheritdoc />
        public IScheduleMetricsReporting ReportScheduler { get; }

        /// <inheritdoc />
        public MetricsOptions Options { get; }

        /// <inheritdoc />
        public EnvironmentInfo EnvironmentInfo => _environmentInfoProvider.Build();
    }
}