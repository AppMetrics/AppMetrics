// <copyright file="HealthReporterBackgroundService.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;
using Microsoft.Extensions.Hosting;

namespace App.Metrics.Health.Extensions.Hosting
{
    public class HealthReporterBackgroundService : BackgroundService
    {
        private static readonly ILog Logger = LogProvider.For<HealthReporterBackgroundService>();
        private static readonly TimeSpan WaitBetweenReportRunChecks = TimeSpan.FromMilliseconds(500);
        private readonly IHealthRoot _healthRoot;
        private readonly List<SchedulerTaskWrapper> _scheduledReporters = new List<SchedulerTaskWrapper>();

        public HealthReporterBackgroundService(
            IHealthRoot healthRoot,
            TimeSpan checkInterval)
        {
            if (checkInterval == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be greater than zero", nameof(checkInterval));
            }

            _healthRoot = healthRoot ?? throw new ArgumentNullException(nameof(healthRoot));

            var referenceTime = DateTime.UtcNow;

            foreach (var reporter in healthRoot.Reporters)
            {
                _scheduledReporters.Add(
                    new SchedulerTaskWrapper
                    {
                        Interval = checkInterval,
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
                && _healthRoot.Options.Enabled && _healthRoot.Options.ReportingEnabled)
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

            foreach (var reportTask in _scheduledReporters)
            {
                if (!reportTask.ShouldRun(referenceTime))
                {
                    Logger.Trace($"Skipping {reportTask.Reporter.GetType().FullName}, next run in {reportTask.NextRunTime.Subtract(referenceTime).Milliseconds} ms");
                    continue;
                }

                reportTask.Increment();

                await taskFactory.StartNew(
                    async () =>
                    {
                        try
                        {
                            Logger.Trace($"Executing reporter {reportTask.Reporter.GetType().FullName}");

                            var healthStatus = await _healthRoot.HealthCheckRunner.ReadAsync(cancellationToken);

                            await reportTask.Reporter.ReportAsync(_healthRoot.Options, healthStatus, cancellationToken);

                            Logger.Trace($"Reporter {reportTask.Reporter.GetType().FullName} executed successfully");
                        }
                        catch (Exception ex)
                        {
                            var args = new UnobservedTaskExceptionEventArgs(
                                ex as AggregateException ?? new AggregateException(ex));

                            Logger.Error($"Reporter {reportTask.Reporter.GetType().FullName} failed", ex);

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

            public IReportHealthStatus Reporter { get; set; }

            public void Increment()
            {
                LastRunTime = NextRunTime;
                NextRunTime = DateTime.UtcNow.Add(Interval);
            }

            public bool ShouldRun(DateTime currentTime) { return NextRunTime < currentTime && LastRunTime != NextRunTime; }
        }
    }
}