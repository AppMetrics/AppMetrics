// <copyright file="SampleMetricsRunner.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Scheduling;

namespace ReportingSandbox.JustForTesting
{
    public static class SampleMetricsRunner
    {
        public static void ScheduleSomeSampleMetrics(IMetrics metrics)
        {
            var processSample = new ProcessSample(metrics);
            var userValueSample = new UserValueTimerSample(metrics);
            var simpleMetrics = new SampleMetrics(metrics);
            var setCounterSample = new SetCounterSample(metrics);
            var setMeterSample = new SetMeterSample(metrics);
            var userValueHistogramSample = new UserValueHistogramSample(metrics);

            var samplesScheduler = new AppMetricsTaskScheduler(
                TimeSpan.FromMilliseconds(300),
                () =>
                {
                    using (metrics.Measure.Apdex.Track(AppMetricsRegistry.ApdexScores.AppApdex))
                    {
                        processSample.Run();
                        setCounterSample.Run();
                        setMeterSample.Run();
                        userValueHistogramSample.Run();
                        userValueSample.Run();
                        simpleMetrics.Run();
                    }

                    SetGaugeValues(metrics);

                    return Task.CompletedTask;
                });

            samplesScheduler.Start();
        }

        private static void SetGaugeValues(IMetrics metrics)
        {
            metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.Errors, () => 1);
            metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.PercentGauge, () => 1);
            metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ApmGauge, () => 1);
            metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.ParenthesisGauge, () => 1);
            metrics.Measure.Gauge.SetValue(AppMetricsRegistry.Gauges.GaugeWithNoValue, () => double.NaN);
        }
    }
}
