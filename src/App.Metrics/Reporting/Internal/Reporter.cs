// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Configuration;
using App.Metrics.Core.Internal;
using App.Metrics.Core.Options;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting.Internal
{
    internal sealed class Reporter : IReporter
    {
        private readonly CounterOptions _failedCounter;
        private readonly ILogger<Reporter> _logger;
        private readonly ILoggerFactory _loggerFactory;

        private readonly Dictionary<Type, IMetricReporter> _metricReporters;
        private readonly IMetrics _metrics;
        private readonly Dictionary<Type, IReporterProvider> _providers;
        private readonly DefaultReportGenerator _reportGenerator;
        private readonly IScheduler _scheduler;

        private readonly CounterOptions _successCounter;

        public Reporter(
            AppMetricsOptions options,
            ReportFactory reportFactory,
            IMetrics metrics,
            IScheduler scheduler,
            ILoggerFactory loggerFactory)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (reportFactory == null)
            {
                throw new ArgumentNullException(nameof(reportFactory));
            }

            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _reportGenerator = new DefaultReportGenerator(options, loggerFactory);
            _metrics = metrics;
            _scheduler = scheduler;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Reporter>();

            _providers = reportFactory.GetProviders();

            if (_providers.Count <= 0)
            {
                return;
            }

            _metricReporters = new Dictionary<Type, IMetricReporter>(_providers.Count);

            foreach (var provider in _providers)
            {
                _metricReporters.Add(provider.Key, provider.Value.CreateMetricReporter(provider.Key.Name, _loggerFactory));
            }

            _successCounter = new CounterOptions
                              {
                                  Context = Constants.InternalMetricsContext,
                                  MeasurementUnit = Unit.Items,
                                  ResetOnReporting = true,
                                  Name = "report_success"
                              };

            _failedCounter = new CounterOptions
                             {
                                 Context = Constants.InternalMetricsContext,
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

        public void RunReports(IMetrics context, CancellationToken token)
        {
            if (_metricReporters == null)
            {
                return;
            }

            var reportTasks = new List<Task>();

            foreach (var metricReporter in _metricReporters)
            {
                var logger = _loggerFactory.CreateLogger(metricReporter.Value.GetType());

                var provider = _providers[metricReporter.Key];

                logger.ReportRunning(metricReporter.Value);

                reportTasks.Add(ScheduleReport(context, token, metricReporter, provider).WithAggregateException());
            }

            try
            {
                Task.WaitAll(reportTasks.ToArray(), token);
            }
            catch (OperationCanceledException ex)
            {
                _logger.ReportingCancelled(ex);
            }
            catch (AggregateException ex)
            {
                _logger.ReportingFailedDuringExecution(ex);
            }
            catch (ObjectDisposedException ex)
            {
                _logger.ReportingDisposedDuringExecution(ex);
            }
        }

        private Task ScheduleReport(
            IMetrics context,
            CancellationToken token,
            KeyValuePair<Type, IMetricReporter> metricReporter,
            IReporterProvider provider)
        {
            return _scheduler.Interval(
                metricReporter.Value.ReportInterval,
                TaskCreationOptions.LongRunning,
                async () =>
                {
                    try
                    {
                        var result = await _reportGenerator.GenerateAsync(
                            metricReporter.Value,
                            context,
                            provider.Filter,
                            token);

                        if (result)
                        {
                            _metrics.Measure.Counter.Increment(_successCounter, metricReporter.Key.Name);
                        }
                        else
                        {
                            _metrics.Measure.Counter.Increment(_failedCounter, metricReporter.Key.Name);
                            _logger.ReportFailed(metricReporter.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        _metrics.Measure.Counter.Increment(_failedCounter, metricReporter.Key.Name);
                        _logger.ReportFailed(metricReporter.Value, ex);
                    }
                },
                token);
        }
    }
}