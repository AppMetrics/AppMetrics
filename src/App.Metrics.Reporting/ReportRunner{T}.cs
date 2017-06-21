// <copyright file="ReportRunner{T}.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Counter;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting
{
    public class ReportRunner<T> : IMetricReporter
    {
        private readonly Func<IMetricPayloadBuilder<T>, Task<bool>> _flushAsync;
        private readonly ILogger<ReportRunner<T>> _logger;
        private readonly IMetricPayloadBuilder<T> _payloadBuilder;
        private bool _disposed;

        public ReportRunner(
            Func<IMetricPayloadBuilder<T>, Task<bool>> flushAsync,
            IMetricPayloadBuilder<T> payloadBuilder,
            TimeSpan reportInterval,
            ILoggerFactory loggerFactory)
            : this(
                flushAsync,
                payloadBuilder,
                reportInterval,
                typeof(ReportRunner<T>).Name,
                loggerFactory)
        {
        }

        public ReportRunner(
            Func<IMetricPayloadBuilder<T>, Task<bool>> flushAsync,
            IMetricPayloadBuilder<T> payloadBuilder,
            TimeSpan reportInterval,
            string name,
            ILoggerFactory loggerFactory)
        {
            ReportInterval = reportInterval;
            Name = name;

            _payloadBuilder = payloadBuilder;
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

        private void ReportApdex(string context, MetricValueSourceBase<ApdexValue> valueSource) { _payloadBuilder.PackApdex(context, valueSource); }

        private void ReportCounter(string context, MetricValueSourceBase<CounterValue> valueSource)
        {
            var counterValueSource = valueSource as CounterValueSource;
            _payloadBuilder.PackCounter(context, valueSource, counterValueSource);
        }

        private void ReportGauge(string context, MetricValueSourceBase<double> valueSource) { _payloadBuilder.PackGauge(context, valueSource); }

        private void ReportHistogram(string context, MetricValueSourceBase<HistogramValue> valueSource)
        {
            _payloadBuilder.PackHistogram(context, valueSource);
        }

        private void ReportMeter(string context, MetricValueSourceBase<MeterValue> valueSource) { _payloadBuilder.PackMeter(context, valueSource); }

        private void ReportTimer(string context, MetricValueSourceBase<TimerValue> valueSource) { _payloadBuilder.PackTimer(context, valueSource); }
    }
}