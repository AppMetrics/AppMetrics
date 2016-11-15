using App.Metrics.Core;
using App.Metrics.Performance.Tests.Setup;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class CounterMetricBenchmark
    {
        private static readonly CounterOptions Counter = new CounterOptions
        {
            Name = "test_counter",
            MeasurementUnit = Unit.Items
        };

        private static IMetrics _metrics;

        [Params("item1")]
        public string Item { get; set; }

        [Benchmark]
        public void DecrementCounter()
        {
            _metrics.Decrement(Counter);
        }

        [Benchmark]
        public void DecrementItemCounter()
        {
            _metrics.Decrement(Counter, Item);
        }

        [Benchmark]
        public void IncrementCounter()
        {
            _metrics.Increment(Counter);
        }

        [Benchmark]
        public void IncrementItemCounter()
        {
            _metrics.Increment(Counter, Item);
        }


        [Setup]
        public void Setup()
        {
            _metrics = MetricsFactory.Instance();
        }
    }
}