using BenchmarkDotNet.Running;
using Xunit;

namespace App.Metrics.Reservoir.Benchmarks.Harness
{
    public class BenchmarkHarness
    {
        [Fact, Trait("MediumRun", "ReservoirSamplingCompare")]
        public void ReservoirSamplingBenchmark()
        {
            BenchmarkRunner.Run<ReservoirSamplingBenchmarks>();
        }
    }
}