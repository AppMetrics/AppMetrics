using App.Metrics.Core;
using App.Metrics.Performance.Tests.Setup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class MeterMetricBenchmark
    {
        private static readonly MeterOptions Meter = new MeterOptions
        {
            Name = "test_meter",
            MeasurementUnit = Unit.Requests,
            RateUnit = TimeUnit.Nanoseconds
        };

        private static IMetrics _metrics;


        [Params("item1")]
        public string Item { get; set; }

        [Benchmark]
        public void MarkMeter()
        {
            _metrics.Mark(Meter, Item);
        }


        [Setup]
        public void Setup()
        {
            _metrics = MetricsFactory.Instance();
        }
    }
}