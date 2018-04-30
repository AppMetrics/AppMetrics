// <copyright file="DefaultMetricsReportRunner.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.Formatters;
using App.Metrics.Logging;
using App.Metrics.Reporting;

namespace App.Metrics.Internal
{
    public class DefaultMetricsReportRunner : IRunMetricsReports
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsReportRunner>();
        private readonly IMetrics _metrics;
        private readonly CounterOptions _failedCounter;
        private readonly MetricsReporterCollection _reporters;
        private readonly CounterOptions _successCounter;

        public DefaultMetricsReportRunner(IMetrics metrics, MetricsReporterCollection reporters)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _reporters = reporters ?? throw new ArgumentNullException(nameof(reporters));

            _successCounter = new CounterOptions
                              {
                                  Context = AppMetricsConstants.InternalMetricsContext,
                                  MeasurementUnit = Unit.Items,
                                  ResetOnReporting = true,
                                  ReportItemPercentages = false,
                                  ReportSetItems = false,
                                  Name = "report_success"
                              };

            _failedCounter = new CounterOptions
                             {
                                 Context = AppMetricsConstants.InternalMetricsContext,
                                 MeasurementUnit = Unit.Items,
                                 ResetOnReporting = true,
                                 ReportItemPercentages = false,
                                 ReportSetItems = false,
                                 Name = "report_failed"
                             };
        }

        /// <inheritdoc />
        public IEnumerable<Task> RunAllAsync(CancellationToken cancellationToken = default)
        {
            return _reporters.Select(reporter => FlushMetrics(_metrics, cancellationToken, reporter));
        }

        /// <inheritdoc />
        public Task RunAsync<TMetricReporter>(CancellationToken cancellationToken = default)
            where TMetricReporter : IReportMetrics
        {
            var reporter = _reporters.GetType<TMetricReporter>();

            if (reporter == null)
            {
                throw new InvalidOperationException($"Metric Reporter of type {typeof(TMetricReporter)} is not available");
            }

            return FlushMetrics(_metrics, cancellationToken, reporter);
        }

        private async Task FlushMetrics(IMetrics metrics, CancellationToken cancellationToken, IReportMetrics reporter)
        {
            try
            {
                Logger.ReportRunning(reporter);

                var result = await reporter.FlushAsync(metrics.Snapshot.Get(reporter.Filter), cancellationToken);

                if (result)
                {
                    metrics.Measure.Counter.Increment(_successCounter, reporter.GetType().FullName);
                }
                else
                {
                    metrics.Measure.Counter.Increment(_failedCounter, reporter.GetType().FullName);
                    Logger.ReportFailed(reporter);
                }
            }
            catch (Exception ex)
            {
                metrics.Measure.Counter.Increment(_failedCounter, reporter.GetType().FullName);
                Logger.ReportFailed(reporter, ex);
            }
        }
    }
}