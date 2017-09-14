// <copyright file="DefaultMetricsReportScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.Formatters;
using App.Metrics.Logging;
using App.Metrics.Reporting;
using App.Metrics.Scheduling;

namespace App.Metrics.Internal
{
    public class DefaultMetricsReportScheduler : IScheduleMetricsReporting
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsReportRunner>();
        private readonly CounterOptions _failedCounter;
        private readonly IMetrics _metrics;
        private readonly MetricsReporterCollection _reporters;
        private readonly IScheduler _scheduler;
        private readonly CounterOptions _successCounter;

        public DefaultMetricsReportScheduler(IMetrics metrics, MetricsReporterCollection reporters, IScheduler scheduler)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _reporters = reporters ?? throw new ArgumentNullException(nameof(reporters));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

            _successCounter = new CounterOptions
                              {
                                  Context = AppMetricsConstants.InternalMetricsContext,
                                  MeasurementUnit = Unit.Items,
                                  ResetOnReporting = true,
                                  Name = "report_success"
                              };

            _failedCounter = new CounterOptions
                             {
                                 Context = AppMetricsConstants.InternalMetricsContext,
                                 MeasurementUnit = Unit.Items,
                                 ResetOnReporting = true,
                                 Name = "report_failed"
                             };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_scheduler != null)
            {
                using (_scheduler)
                {
                }
            }
        }

        /// <inheritdoc />
        public void ScheduleAll(CancellationToken cancellationToken = default)
        {
            ScheduleReporters(cancellationToken: cancellationToken);
        }

        public void ScheduleAll(TimeSpan interval, CancellationToken cancellationToken = default) { ScheduleReporters(interval, cancellationToken); }

        private async Task FlushMetrics(IMetrics metrics, CancellationToken cancellationToken, IReportMetrics reporter)
        {
            try
            {
                var result = await reporter.FlushAsync(metrics.Snapshot.Get(reporter.Filter), cancellationToken);

                if (result)
                {
                    metrics.Measure.Counter.Increment(_successCounter, reporter.GetType().Name);
                }
                else
                {
                    metrics.Measure.Counter.Increment(_failedCounter, reporter.GetType().Name);
                    Logger.ReportFailed(reporter);
                }
            }
            catch (Exception ex)
            {
                metrics.Measure.Counter.Increment(_failedCounter, reporter.GetType().Name);
                Logger.ReportFailed(reporter, ex);
            }
        }

        private Task ScheduleReport(
            IMetrics metrics,
            CancellationToken cancellationToken,
            IReportMetrics reporter,
            TimeSpan? interval = default)
        {
            return _scheduler.Interval(
                interval ?? reporter.FlushInterval,
                TaskCreationOptions.LongRunning,
                async () => { await FlushMetrics(metrics, cancellationToken, reporter); },
                cancellationToken);
        }

        private void ScheduleReporters(TimeSpan? interval = null, CancellationToken cancellationToken = default)
        {
            var reportTasks = new List<Task>();

            foreach (var reporter in _reporters)
            {
                Logger.ReportRunning(reporter);

                reportTasks.Add(ScheduleReport(_metrics, cancellationToken, reporter, interval).WithAggregateException());
            }

            try
            {
                Task.WaitAll(reportTasks.ToArray(), cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                Logger.ReportingCancelled(ex);
            }
            catch (AggregateException ex)
            {
                Logger.ReportingFailedDuringExecution(ex);
            }
            catch (ObjectDisposedException ex)
            {
                Logger.ReportingDisposedDuringExecution(ex);
            }
        }
    }
}