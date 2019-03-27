// <copyright file="MetricsReporterBackgroundService.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.Logging;
using App.Metrics.Reporting;
using Microsoft.Extensions.Hosting;

namespace App.Metrics.Extensions.Hosting
{
    public class MetricsReporterBackgroundService : BackgroundService
    {
        private static readonly ILog Logger = LogProvider.For<MetricsReporterBackgroundService>();
        private static readonly TimeSpan WaitBetweenReportRunChecks = TimeSpan.FromMilliseconds(500);
        private readonly IMetrics _metrics;
        private readonly CounterOptions _successCounter;
        private readonly CounterOptions _failedCounter;
        private readonly MetricsOptions _options;
        private readonly List<SchedulerTaskWrapper> _scheduledReporters = new List<SchedulerTaskWrapper>();

        public MetricsReporterBackgroundService(
            IMetrics metrics,
            MetricsOptions options,
            IEnumerable<IReportMetrics> reporters)
        {
            _metrics = metrics;
            _options = options;

            var referenceTime = DateTime.UtcNow;

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

            foreach (var reporter in reporters)
            {
                _scheduledReporters.Add(
                    new SchedulerTaskWrapper
                    {
                        Interval = reporter.FlushInterval,
                        Reporter = reporter,
                        NextRunTime = referenceTime
                    });
            }
        }

        public event EventHandler<UnobservedTaskExceptionEventArgs> UnobservedTaskException;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_scheduledReporters.Any())
            {
                await Task.CompletedTask;
            }

            while (!cancellationToken.IsCancellationRequested
                && _options.Enabled
                && _options.ReportingEnabled)
            {
                await ExecuteOnceAsync(cancellationToken);

                Logger.Trace($"Delaying for {WaitBetweenReportRunChecks}");
                await Task.Delay(WaitBetweenReportRunChecks, cancellationToken);
            }
        }

        private async Task ExecuteOnceAsync(CancellationToken cancellationToken)
        {
            var taskFactory = new TaskFactory(TaskScheduler.Current);
            var referenceTime = DateTime.UtcNow;

            foreach (var flushTask in _scheduledReporters)
            {
                if (!flushTask.ShouldRun(referenceTime))
                {
                    Logger.Trace($"Skipping {flushTask.Reporter.GetType().FullName}, next run in {flushTask.NextRunTime.Subtract(referenceTime).Milliseconds} ms");
                    continue;
                }

                flushTask.Increment();

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            Logger.Trace($"Executing reporter {flushTask.Reporter.GetType().FullName} FlushAsync");

                            var result = await flushTask.Reporter.FlushAsync(
                                _metrics.Snapshot.Get(flushTask.Reporter.Filter),
                                cancellationToken);

                            if (result)
                            {
                                _metrics.Measure.Counter.Increment(_successCounter, flushTask.Reporter.GetType().FullName);
                                Logger.Trace($"Reporter {flushTask.Reporter.GetType().FullName} FlushAsync executed successfully");
                            }
                            else
                            {
                                _metrics.Measure.Counter.Increment(_failedCounter, flushTask.Reporter.GetType().FullName);
                                Logger.Warn($"Reporter {flushTask.Reporter.GetType().FullName} FlushAsync failed");
                            }
                        }
                        catch (Exception ex)
                        {
                            _metrics.Measure.Counter.Increment(_failedCounter, flushTask.Reporter.GetType().FullName);

                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            Logger.Error($"Reporter {flushTask.Reporter.GetType().FullName} FlushAsync failed", ex);

                            UnobservedTaskException?.Invoke(this, args);

                            if (!args.Observed)
                            {
                                throw;
                            }
                        }
                    },
                    cancellationToken);
            }
        }

        private class SchedulerTaskWrapper
        {
            public TimeSpan Interval { get; set; }

            public DateTime LastRunTime { get; set; }

            public DateTime NextRunTime { get; set; }

            public IReportMetrics Reporter { get; set; }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = DateTime.UtcNow.Add(Interval);
            }

            public bool ShouldRun(DateTime currentTime) { return NextRunTime < currentTime && LastRunTime != NextRunTime; }
        }
    }
}