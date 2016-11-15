using App.Metrics.Core;
using App.Metrics.Core.Options;
using App.Metrics.Performance.Tests.Setup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class GauageMetricBenchmark
    {
        private static readonly GaugeOptions Gauge = new GaugeOptions
        {
            Name = "test_gauage",
            MeasurementUnit = Unit.Threads
        };

        private static IMetrics _metrics;

        [Benchmark]
        public void MarkeGauage()
        {
            _metrics.Gauge(Gauge, () => 1);
        }


        [Setup]
        public void Setup()
        {
            _metrics = MetricsFactory.Instance();
        }
    }
}