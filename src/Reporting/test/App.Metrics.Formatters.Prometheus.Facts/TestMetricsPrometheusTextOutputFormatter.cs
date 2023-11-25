using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.BucketHistogram;
using App.Metrics.BucketTimer;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Formatters.Prometheus.Facts
{
    public class TestMetricsPrometheusTextOutputFormatter
    {
        private readonly IClock _clock = new TestClock();
        private readonly IReservoir _defaultReservoir = new DefaultForwardDecayingReservoir();

        private readonly MetricsPrometheusTextOutputFormatter _metricsPrometheusTextOutputFormatter =
            new MetricsPrometheusTextOutputFormatter();

        private readonly DateTime _timestamp = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);

        [Fact]
        public async Task Apdex_output_contains_description()
        {
            const string expected =
                "# HELP test_apdex apdex description\n# TYPE test_apdex gauge\ntest_apdex 0\n\n";

            var apdex = new DefaultApdexMetric(_defaultReservoir, _clock, false);
            var apdexValueSource = new ApdexValueSource(
                "apdex",
                ConstantValue.Provider(apdex.Value),
                MetricTags.Empty,
                description: "apdex description");

            var output = await GetFormatterOutput(CreateValueSource(apdexScores: apdexValueSource));
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Counter_output_contains_description()
        {
            const string expected =
                "# HELP test_counter counter_description\n# TYPE test_counter gauge\ntest_counter 0\n\n";

            var counter = new DefaultCounterMetric();
            var counterValueSource = new CounterValueSource("counter", ConstantValue.Provider(counter.Value),
                Unit.Calls,
                MetricTags.Empty,
                description: "counter_description");

            var output = await GetFormatterOutput(CreateValueSource(counters: counterValueSource));
            output.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Bucket_histogram_output_contains_description()
        {
            const string expected =
                "# HELP test_bucket_histogram bucket histogram description\n# TYPE test_bucket_histogram histogram\ntest_bucket_histogram_sum 0\ntest_bucket_histogram_count 0\ntest_bucket_histogram_bucket{le=\"0.5\"} 0\ntest_bucket_histogram_bucket{le=\"+Inf\"} 0\n\n";
            var bucketHistogram = new DefaultBucketHistogramMetric(new[] {0.5});
            var bucketHistogramValueSource = new BucketHistogramValueSource("bucket_histogram",
                ConstantValue.Provider(bucketHistogram.Value),
                Unit.Calls,
                MetricTags.Empty,
                "bucket histogram description");
            var metricsContextValueSource = CreateValueSource(bucketHistograms: bucketHistogramValueSource);

            var output = await GetFormatterOutput(metricsContextValueSource);
            output.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Bucket_timer_output_contains_description()
        {
            const string expected =
                "# HELP test_bucket_timer bucket timer description\n# TYPE test_bucket_timer histogram\ntest_bucket_timer_sum 0\ntest_bucket_timer_count 0\ntest_bucket_timer_bucket{le=\"0.5\"} 0\ntest_bucket_timer_bucket{le=\"+Inf\"} 0\n\n";
            var bucketHistogram = new DefaultBucketHistogramMetric(new[] {0.5});
            var bucketTimer = new DefaultBucketTimerMetric(bucketHistogram,_clock,TimeUnit.Milliseconds);
            var bucketTimerValueSource = new BucketTimerValueSource(
                "bucket_timer",
                ConstantValue.Provider(bucketTimer.Value),
                Unit.Calls,
                TimeUnit.Milliseconds,
                TimeUnit.Milliseconds,
                MetricTags.Empty,
                "bucket timer description");
            var metricsContextValueSource = CreateValueSource(bucketTimers: bucketTimerValueSource);

            var output = await GetFormatterOutput(metricsContextValueSource);
            output.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Gauge_output_contains_description()
        {
            const string expected =
                "# HELP test_gauge gauge description\n# TYPE test_gauge gauge\ntest_gauge 1\n\n";
            var gauge = new FunctionGauge(() => 1);
            var gaugeValueSource = new GaugeValueSource(
                "gauge",
                ConstantValue.Provider(gauge.Value),
                Unit.None,
                MetricTags.Empty,
                description: "gauge description");
            var metricsContextValueSource = CreateValueSource(gauges: gaugeValueSource);

            var output = await GetFormatterOutput(metricsContextValueSource);
            output.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Histogram_output_contains_description()
        {
            const string expected =
                "# HELP test_histogram histogram description\n# TYPE test_histogram summary\ntest_histogram_sum 0\ntest_histogram_count 0\ntest_histogram{quantile=\"0.5\"} 0\ntest_histogram{quantile=\"0.75\"} 0\ntest_histogram{quantile=\"0.95\"} 0\ntest_histogram{quantile=\"0.99\"} 0\n\n";
            var histogram = new DefaultHistogramMetric(_defaultReservoir);
            var histogramValueSource = new HistogramValueSource(
                "histogram",
                ConstantValue.Provider(histogram.Value),
                Unit.None,
                MetricTags.Empty,
                description: "histogram description");
            
            var metricsContextValueSource = CreateValueSource(histograms: histogramValueSource);

            var output = await GetFormatterOutput(metricsContextValueSource);
            output.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Meter_output_contains_description()
        {
            const string expected =
                "# HELP test_meter_total meter description\n# TYPE test_meter_total counter\ntest_meter_total 0\n\n";
            var clock = new TestClock();
            var meter = new DefaultMeterMetric(clock);
            
            var meterValueSource = new MeterValueSource(
                "meter",
                ConstantValue.Provider(meter.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                MetricTags.Empty,
                description: "meter description");
            
            var metricsContextValueSource = CreateValueSource(meters: meterValueSource);

            var output = await GetFormatterOutput(metricsContextValueSource);
            output.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public async Task Timer_output_contains_description()
        {
            const string expected =
                "# HELP test_timer timer description\n# TYPE test_timer summary\ntest_timer_sum 0\ntest_timer_count 0\ntest_timer{quantile=\"0.5\"} 0\ntest_timer{quantile=\"0.75\"} 0\ntest_timer{quantile=\"0.95\"} 0\ntest_timer{quantile=\"0.99\"} 0\n\n";
            var clock = new TestClock();
            var timer = new DefaultTimerMetric(_defaultReservoir, clock);
            var timerValueSource = new TimerValueSource(
                "timer",
                ConstantValue.Provider(timer.Value),
                Unit.None,
                TimeUnit.Milliseconds,
                TimeUnit.Milliseconds,
                MetricTags.Empty,
                description: "timer description");

            // Act
            var metricsContextValueSource = CreateValueSource(timers: timerValueSource);

            var output = await GetFormatterOutput(metricsContextValueSource);
            output.Should().BeEquivalentTo(expected);
        }

        private async Task<string> GetFormatterOutput(MetricsContextValueSource metricsContextValueSource)
        {
            await using var stream = new MemoryStream();
            await _metricsPrometheusTextOutputFormatter.WriteAsync(stream,
                new MetricsDataValueSource(_timestamp,
                    new[] {metricsContextValueSource}));

            var output = Encoding.UTF8.GetString(stream.ToArray());
            return output;
        }

        private static MetricsContextValueSource CreateValueSource(
            string context="test",
            GaugeValueSource gauges = null,
            CounterValueSource counters = null,
            MeterValueSource meters = null,
            HistogramValueSource histograms = null,
            BucketHistogramValueSource bucketHistograms = null,
            TimerValueSource timers = null,
            BucketTimerValueSource bucketTimers = null,
            ApdexValueSource apdexScores = null)
        {
            var gaugeValues = gauges != null ? new[] {gauges} : Enumerable.Empty<GaugeValueSource>();
            var counterValues = counters != null ? new[] {counters} : Enumerable.Empty<CounterValueSource>();
            var meterValues = meters != null ? new[] {meters} : Enumerable.Empty<MeterValueSource>();
            var histogramValues = histograms != null ? new[] {histograms} : Enumerable.Empty<HistogramValueSource>();
            var bucketHistogramValues = bucketHistograms != null
                ? new[] {bucketHistograms}
                : Enumerable.Empty<BucketHistogramValueSource>();
            var timerValues = timers != null ? new[] {timers} : Enumerable.Empty<TimerValueSource>();
            var bucketTimerValues =
                bucketTimers != null ? new[] {bucketTimers} : Enumerable.Empty<BucketTimerValueSource>();
            var apdexScoreValues = apdexScores != null ? new[] {apdexScores} : Enumerable.Empty<ApdexValueSource>();

            return new MetricsContextValueSource(
                context,
                gaugeValues,
                counterValues,
                meterValues,
                histogramValues,
                bucketHistogramValues,
                timerValues,
                bucketTimerValues,
                apdexScoreValues);
        }
    }
}