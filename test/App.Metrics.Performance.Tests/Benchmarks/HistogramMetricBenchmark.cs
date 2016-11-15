using App.Metrics.Core.Options;
using App.Metrics.Performance.Tests.Setup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class HistogramMetricBenchmark
    {
        private static readonly HistogramOptions Histogram = new HistogramOptions
        {
            Name = "test_histogram",
            MeasurementUnit = Unit.Threads
        };

        private static IMetrics _metrics;

        [Params("item1")]
        public string UserValue { get; set; }


        [Setup]
        public void Setup()
        {
            _metrics = MetricsFactory.Instance();
        }

        [Benchmark]
        public void UpdateHistogram()
        {
            _metrics.Update(Histogram, 1);
        }

        [Benchmark]
        public void UpdateHistogramWithUserValue()
        {
            _metrics.Update(Histogram, 1, UserValue);
        }
    }
}