using App.Metrics.Internal;
using App.Metrics.Performance.Tests.Setup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class MetricsAdvancedBenchmarks
    {
        private static readonly DefaultMetricsFilter Filter = new DefaultMetricsFilter().WhereMetricNameStartsWith("test_");

        private static IMetrics _metrics;

        [Params("item1")]
        public string Item { get; set; }

        [Benchmark]
        public void ReadDataAsync()
        {
            _metrics.Advanced.Data.ReadDataAsync().GetAwaiter().GetResult();
        }

        [Benchmark]
        public void ReadDataAsyncWithFilter()
        {
            _metrics.Advanced.Data.ReadDataAsync(Filter).GetAwaiter().GetResult();
        }


        [Benchmark]
        public void ReadStatusAsync()
        {
            _metrics.Advanced.Health.ReadStatusAsync().GetAwaiter().GetResult();
        }


        [Setup]
        public void Setup()
        {
            _metrics = MetricsFactory.Instance();
        }
    }
}