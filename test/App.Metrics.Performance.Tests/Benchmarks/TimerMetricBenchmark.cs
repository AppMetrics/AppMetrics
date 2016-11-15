using App.Metrics.Core;
using App.Metrics.Core.Options;
using App.Metrics.Performance.Tests.Setup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class TimerMetricBenchmark
    {
        private static readonly TimerOptions Timer = new TimerOptions
        {
            Name = "test_timer",
            MeasurementUnit = Unit.Threads
        };

        private static IMetrics _metrics;

        [Params("item1")]
        public string UserValue { get; set; }

        [Benchmark]
        public void RecordTime()
        {
            using (var timer = _metrics.Time(Timer))
            {
            }
        }

        [Benchmark]
        public void RecordTimeAsAction()
        {
            _metrics.Time(Timer, () => { });
        }

        [Benchmark]
        public void RecordTimeAsActionWithUserValue()
        {
            _metrics.Time(Timer, () => { }, UserValue);
        }

        [Benchmark]
        public void RecordTimeWithUserValue()
        {
            using (var timer = _metrics.Time(Timer, UserValue))
            {
            }
        }


        [Setup]
        public void Setup()
        {
            _metrics = MetricsFactory.Instance();
        }
    }
}