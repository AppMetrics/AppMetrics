// <copyright file="MetricsReportingBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Builder;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Internal.NoOp;
using App.Metrics.Reporting;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring <see cref="IReportMetrics" />s used for reporting <see cref="IMetrics" />s.
    /// </summary>
    public class MetricsReportingBuilder : IMetricsReportingBuilder
    {
        private static readonly IFilterMetrics NullMetricsFilter = new NullMetricsFilter();
        private readonly MetricsFormatterCollection _formatters;
        private readonly Action<IReportMetrics> _reporters;

        internal MetricsReportingBuilder(
            IMetricsBuilder metricsBuilder,
            MetricsFormatterCollection formatters,
            Action<IReportMetrics> reporters)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _formatters = formatters ?? throw new ArgumentNullException(nameof(formatters));
            _reporters = reporters ?? throw new ArgumentNullException(nameof(reporters));
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder Using(IReportMetrics reporter)
        {
            if (reporter == null)
            {
                throw new ArgumentNullException(nameof(reporter));
            }

            EnsureRequiredProperties(reporter);

            _reporters(reporter);

            return Builder;
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>()
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics();

            UseUserConfiguredFormatter(reporter);

            return Using(reporter);
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>(IFilterMetrics filter)
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics { Filter = filter ?? new NullMetricsFilter() };

            UseUserConfiguredFormatter(reporter);

            return Using(reporter);
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>(IMetricsOutputFormatter formatter)
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics { Formatter = formatter };

            return Using(reporter);
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>(IMetricsOutputFormatter formatter, TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics { Formatter = formatter, FlushInterval = flushInterval };

            return Using(reporter);
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>(TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics { FlushInterval = flushInterval };

            UseUserConfiguredFormatter(reporter);

            return Using(reporter);
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>(IFilterMetrics filter, TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics { Filter = filter, FlushInterval = flushInterval };

            UseUserConfiguredFormatter(reporter);

            return Using(reporter);
        }

        /// <inheritdoc />
        public IMetricsBuilder Using<TReportMetrics>(IMetricsOutputFormatter formatter, IFilterMetrics filter, TimeSpan flushInterval)
            where TReportMetrics : IReportMetrics, new()
        {
            var reporter = new TReportMetrics { Formatter = formatter, Filter = filter, FlushInterval = flushInterval };

            return Using(reporter);
        }

        private static void EnsureRequiredProperties(IReportMetrics reporter)
        {
            reporter.FlushInterval = reporter.FlushInterval <= TimeSpan.Zero
                ? AppMetricsConstants.Reporting.DefaultFlushInterval
                : reporter.FlushInterval;

            reporter.Filter = reporter.Filter == default(IFilterMetrics)
                ? NullMetricsFilter
                : reporter.Filter;
        }

        /// <summary>
        /// If the user as explictity set a formatter us this as the default formatter to output metrics.
        /// </summary>
        /// <param name="reporter">The reporter to set the user configured formatter</param>
        private void UseUserConfiguredFormatter(IReportMetrics reporter)
        {
            var formatter = _formatters.FirstOrDefault();

            if (formatter != null)
            {
                reporter.Formatter = formatter;
            }
        }
    }
}