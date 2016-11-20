using App.Metrics.Sampling;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;

namespace App.Metrics.Performance.Tests.Benchmarks
{
    [MarkdownExporter]
    public class SamplingBenchmark
    {
        private const int Size = 10;
        private ExponentiallyDecayingReservoir _exponentiallyDecayingReservoir;
        private SlidingWindowReservoir _slidingWindowReservoir;
        private UniformReservoir _uniformReservoir;


        [Benchmark]
        public void ExponentiallyDecayingReservoir()
        {
            for (var i = 0; i < Size; i++)
            {
                _exponentiallyDecayingReservoir.Update(i);
            }
            _exponentiallyDecayingReservoir.GetSnapshot();
        }

        [Setup]
        public void SetUp()
        {
            _exponentiallyDecayingReservoir = new ExponentiallyDecayingReservoir(Size, 0.01);
            _uniformReservoir = new UniformReservoir(Size);
            _slidingWindowReservoir = new SlidingWindowReservoir(Size);
        }

        [Benchmark]
        public void SlidingWindowReservoir()
        {
            for (var i = 0; i < Size; i++)
            {
                _slidingWindowReservoir.Update(i);
            }

            _slidingWindowReservoir.GetSnapshot();
        }

        [Benchmark]
        public void UniformReservoir()
        {
            for (var i = 0; i < Size; i++)
            {
                _uniformReservoir.Update(i);
            }

            _uniformReservoir.GetSnapshot();
        }
    }
}