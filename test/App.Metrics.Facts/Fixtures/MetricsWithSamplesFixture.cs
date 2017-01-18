using App.Metrics.Core;
using App.Metrics.Core.Options;

namespace App.Metrics.Facts.Fixtures
{
    public class MetricsWithSamplesFixture : MetricsFixture
    {
        public MetricsWithSamplesFixture()
        {
            RecordSomeMetrics();
        }

        private void RecordSomeMetrics()
        {
            var counterOptions = new CounterOptions
            {
                Name = "test_counter",
                MeasurementUnit = Unit.Requests,
                Tags = new MetricTags().With("tag1", "value1")
            };

            var meterOptions = new MeterOptions
            {
                Name = "test_meter",
                MeasurementUnit = Unit.None,
                Tags = new MetricTags().With("tag2", "value2")
            };

            var timerOptions = new TimerOptions
            {
                Name = "test_timer",
                MeasurementUnit = Unit.Requests
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

            Metrics.Counter.Increment(counterOptions);
            Metrics.Meter.Mark(meterOptions);
            Metrics.Timer.Time(timerOptions, () => Metrics.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Histogram.Update(histogramOptions, 5);
            Metrics.Gauge.SetValue(gaugeOptions, () => 8);
        }
    }
}