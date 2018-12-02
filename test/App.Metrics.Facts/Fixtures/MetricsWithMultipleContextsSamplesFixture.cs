// <copyright file="MetricsWithMultipleContextsSamplesFixture.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Counter;
using App.Metrics.FactsCommon.Fixtures;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsWithMultipleContextsSamplesFixture : MetricsFixture
    {
        public MetricsWithMultipleContextsSamplesFixture() { RecordSomeMetrics(); }

        private void RecordSomeMetrics()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test_counter",
                                     MeasurementUnit = Unit.Requests,
                                     Tags = new MetricTags("tag1", "value"),
                                     Context = "test_context1"
                                 };

            var meterOptions = new MeterOptions
                               {
                                   Name = "test_meter",
                                   MeasurementUnit = Unit.None,
                                   Tags = new MetricTags("tag2", "value")
                               };

            var timerOptions = new TimerOptions
                               {
                                   Name = "test_timer",
                                   MeasurementUnit = Unit.Requests,
                                   Context = "test_context2"
                               };

            var histogramOptions = new HistogramOptions
                                   {
                                       Name = "test_histogram",
                                       MeasurementUnit = Unit.Requests
                                   };

            var gaugeOptions = new GaugeOptions
                               {
                                   Name = "test_gauge"
                               };

            Metrics.Measure.Counter.Increment(counterOptions);
            Metrics.Measure.Meter.Mark(meterOptions);
            Metrics.Measure.Timer.Time(timerOptions, () => Metrics.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Measure.Histogram.Update(histogramOptions, 5);
            Metrics.Measure.Gauge.SetValue(gaugeOptions, () => 8);
        }
    }
}