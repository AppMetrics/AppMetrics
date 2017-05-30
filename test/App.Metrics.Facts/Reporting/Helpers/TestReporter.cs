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
using App.Metrics.Reporting;
using App.Metrics.Reporting.Abstractions;
using App.Metrics.Tagging;
using App.Metrics.Timer;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestReporter : IMetricReporter
    {
        private readonly IMetricPayloadBuilder<TestMetricPayload> _payloadBuilder;
        private bool _disposed;

        public TestReporter(IMetricPayloadBuilder<TestMetricPayload> payloadBuilder)
        {
            _payloadBuilder = payloadBuilder;
           
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public TimeSpan ReportInterval { get; }

        /// <inheritdoc />
        public Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            return AppMetricsTaskCache.SuccessTask;
        }

        /// <inheritdoc />
        public void ReportEnvironment(EnvironmentInfo environmentInfo) { }

        /// <inheritdoc />
        public void ReportHealth(GlobalMetricTags globalTags, IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> degradedChecks, IEnumerable<HealthCheck.Result> unhealthyChecks) {  }

        /// <inheritdoc />
        public void ReportMetric<T>(string context, MetricValueSourceBase<T> valueSource)
        {
            if (typeof(T) == typeof(double))
            {
                ReportGauge(context, valueSource as MetricValueSourceBase<double>);
                return;
            }

            if (typeof(T) == typeof(CounterValue))
            {
                ReportCounter(context, valueSource as MetricValueSourceBase<CounterValue>);
                return;
            }

            if (typeof(T) == typeof(MeterValue))
            {
                ReportMeter(context, valueSource as MetricValueSourceBase<MeterValue>);
                return;
            }

            if (typeof(T) == typeof(TimerValue))
            {
                ReportTimer(context, valueSource as MetricValueSourceBase<TimerValue>);
                return;
            }

            if (typeof(T) == typeof(HistogramValue))
            {
                ReportHistogram(context, valueSource as MetricValueSourceBase<HistogramValue>);
                return;
            }

            if (typeof(T) == typeof(ApdexValue))
            {
                ReportApdex(context, valueSource as MetricValueSourceBase<ApdexValue>);
                return;
            }
        }

        /// <inheritdoc />
        public void StartReportRun(IMetrics metrics) { _payloadBuilder.Init(); }

        private void ReportApdex(string context, MetricValueSourceBase<ApdexValue> valueSource)
        {
            _payloadBuilder.PackApdex(context, valueSource);
        }

        private void ReportCounter(string context, MetricValueSourceBase<CounterValue> valueSource)
        {
            var counterValueSource = valueSource as CounterValueSource;
            _payloadBuilder.PackCounter(context, valueSource, counterValueSource);
        }

        private void ReportGauge(string context, MetricValueSourceBase<double> valueSource)
        {
            _payloadBuilder.PackGauge(context, valueSource);
        }

        private void ReportHistogram(string context, MetricValueSourceBase<HistogramValue> valueSource)
        {
            _payloadBuilder.PackHistogram(context, valueSource);
        }

        private void ReportMeter(string context, MetricValueSourceBase<MeterValue> valueSource)
        {
            _payloadBuilder.PackMeter(context, valueSource);
        }

        private void ReportTimer(string context, MetricValueSourceBase<TimerValue> valueSource)
        {
            _payloadBuilder.PackTimer(context, valueSource);
        }
    }
}