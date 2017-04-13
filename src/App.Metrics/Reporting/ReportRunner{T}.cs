// <copyright file="ReportRunner{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Abstractions.Reporting;
using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting
{
    public class ReportRunner<T> : IMetricReporter
    {
        private readonly MetricValueDataKeys _dataKeys;
        private readonly Func<IMetricPayloadBuilder<T>, Task<bool>> _flushAsync;
        private readonly ILogger<ReportRunner<T>> _logger;
        private readonly Func<string, string, string> _metricNameFormatter;
        private readonly IMetricPayloadBuilder<T> _payloadBuilder;
        private bool _disposed;

        public ReportRunner(
            Func<IMetricPayloadBuilder<T>, Task<bool>> flushAsync,
            IMetricPayloadBuilder<T> payloadBuilder,
            TimeSpan reportInterval,
            ILoggerFactory loggerFactory,
            Func<string, string, string> metricNameFormatter,
            MetricValueDataKeys customDataKeys = null)
            : this(
                flushAsync,
                payloadBuilder,
                reportInterval,
                typeof(ReportRunner<T>).Name,
                loggerFactory,
                metricNameFormatter,
                customDataKeys)
        {
        }

        public ReportRunner(
            Func<IMetricPayloadBuilder<T>, Task<bool>> flushAsync,
            IMetricPayloadBuilder<T> payloadBuilder,
            TimeSpan reportInterval,
            string name,
            ILoggerFactory loggerFactory,
            Func<string, string, string> metricNameFormatter,
            MetricValueDataKeys customDataKeys = null)
        {
            ReportInterval = reportInterval;
            Name = name;

            _payloadBuilder = payloadBuilder;
            _metricNameFormatter = metricNameFormatter;
            _dataKeys = customDataKeys ?? new MetricValueDataKeys();
            _logger = loggerFactory.CreateLogger<ReportRunner<T>>();
            _flushAsync = flushAsync;
        }

        public string Name { get; }

        public TimeSpan ReportInterval { get; }

        public void Dispose() { Dispose(true); }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    _payloadBuilder.Clear();
                }
            }

            _disposed = true;
        }

        public async Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            _logger.LogTrace($"Ending {Name} Run");

            var result = await _flushAsync(_payloadBuilder);

            _payloadBuilder.Clear();

            return result;
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo) { }

        public void ReportHealth(
            GlobalMetricTags globalTags,
            IEnumerable<HealthCheck.Result> healthyChecks,
            IEnumerable<HealthCheck.Result> degradedChecks,
            IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
            // Health checks are reported as metrics as well
        }

        public void ReportMetric<TMetric>(string context, MetricValueSourceBase<TMetric> valueSource)
        {
            _logger.LogTrace($"Packing Metric {typeof(T)} for {Name}");

            if (typeof(TMetric) == typeof(double))
            {
                ReportGauge(context, valueSource as MetricValueSourceBase<double>);
                return;
            }

            if (typeof(TMetric) == typeof(CounterValue))
            {
                ReportCounter(context, valueSource as MetricValueSourceBase<CounterValue>);
                return;
            }

            if (typeof(TMetric) == typeof(MeterValue))
            {
                ReportMeter(context, valueSource as MetricValueSourceBase<MeterValue>);
                return;
            }

            if (typeof(TMetric) == typeof(TimerValue))
            {
                ReportTimer(context, valueSource as MetricValueSourceBase<TimerValue>);
                return;
            }

            if (typeof(TMetric) == typeof(HistogramValue))
            {
                ReportHistogram(context, valueSource as MetricValueSourceBase<HistogramValue>);
                return;
            }

            if (typeof(TMetric) == typeof(ApdexValue))
            {
                ReportApdex(context, valueSource as MetricValueSourceBase<ApdexValue>);
                return;
            }

            _logger.LogTrace($"Finished Packing Metric {typeof(T)} for {Name}");
        }

        public void StartReportRun(IMetrics metrics)
        {
            _logger.LogTrace($"Starting {Name} Report Run");

            _payloadBuilder.Init();
        }

        private void ReportApdex(string context, MetricValueSourceBase<ApdexValue> valueSource)
        {
            _payloadBuilder.PackApdex(_metricNameFormatter, context, valueSource, _dataKeys.Apdex);
        }

        private void ReportCounter(string context, MetricValueSourceBase<CounterValue> valueSource)
        {
            var counterValueSource = valueSource as CounterValueSource;
            _payloadBuilder.PackCounter(_metricNameFormatter, context, valueSource, counterValueSource, _dataKeys.Counter);
        }

        private void ReportGauge(string context, MetricValueSourceBase<double> valueSource)
        {
            _payloadBuilder.PackGauge(_metricNameFormatter, context, valueSource);
        }

        private void ReportHistogram(string context, MetricValueSourceBase<HistogramValue> valueSource)
        {
            _payloadBuilder.PackHistogram(_metricNameFormatter, context, valueSource, _dataKeys.Histogram);
        }

        private void ReportMeter(string context, MetricValueSourceBase<MeterValue> valueSource)
        {
            _payloadBuilder.PackMeter(_metricNameFormatter, context, valueSource, _dataKeys.Meter);
        }

        private void ReportTimer(string context, MetricValueSourceBase<TimerValue> valueSource)
        {
            _payloadBuilder.PackTimer(_metricNameFormatter, context, valueSource, _dataKeys.Meter, _dataKeys.Histogram);
        }
    }
}