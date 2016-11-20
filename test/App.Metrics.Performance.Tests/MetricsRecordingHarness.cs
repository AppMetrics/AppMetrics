using App.Metrics.Performance.Tests.Benchmarks;
using BenchmarkDotNet.Running;
using Xunit;

namespace App.Metrics.Performance.Tests
{
    public class MetricsRecordingHarness
    {
        [Fact]
        public void AdvancedMetrics()
        {
            BenchmarkRunner.Run<MetricsAdvancedBenchmarks>();
        }

        [Fact]
        public void CounterMetrics()
        {
            BenchmarkRunner.Run<CounterMetricBenchmark>();
        }

        [Fact]
        public void GaugeMetrics()
        {
            BenchmarkRunner.Run<GauageMetricBenchmark>();
        }

        [Fact]
        public void HistogramMetrics()
        {
            BenchmarkRunner.Run<HistogramMetricBenchmark>();
        }

        [Fact]
        public void MeterMetrics()
        {
            BenchmarkRunner.Run<MeterMetricBenchmark>();
        }

        [Fact]
        public void Sampling()
        {
            BenchmarkRunner.Run<SamplingBenchmark>();
        }

        [Fact]
        public void TimerMetrics()
        {
            BenchmarkRunner.Run<TimerMetricBenchmark>();
        }
    }
}