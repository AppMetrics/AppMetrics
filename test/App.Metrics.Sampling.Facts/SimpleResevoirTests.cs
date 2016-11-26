using System.Collections.Generic;
using System.Linq;
using App.Metrics.Internal;
using App.Metrics.Sampling;
using Xunit;

namespace App.Metrics.Facts
{
    public class SimpleResevoirTests
    {
        private readonly IEnumerable<long> _samples;

        public SimpleResevoirTests()
        {
            _samples = new long[] { 0, 4, 1, 5 }.AsEnumerable();
        }

        [Fact]
        public void ExponentialDecayingResevoir()
        {
            var reservoir = new ExponentiallyDecayingReservoir(Constants.ReservoirSampling.DefaultSampleSize, Constants.ReservoirSampling.DefaultExponentialDecayFactor);

            foreach (var sample in _samples)
            {
                reservoir.Update(sample);
            }

            var snapshot = reservoir.GetSnapshot();

            //TODO: Assert snapshot
            //snapshot.AssertValues(_samples);
        }

        [Fact]
        public void SlidingWindowResevoir()
        {
            var reservoir = new SlidingWindowReservoir(Constants.ReservoirSampling.DefaultSampleSize);

            foreach (var sample in _samples)
            {
                reservoir.Update((long)sample);
            }

            var snapshot = reservoir.GetSnapshot();

            //TODO: Assert snapshot
            //snapshot.AssertValues(_samples);
        }

        [Fact]
        public void UniformResevoir()
        {
            var reservoir = new UniformReservoir(Constants.ReservoirSampling.DefaultSampleSize);

            foreach (var sample in _samples)
            {
                reservoir.Update((long)sample);
            }

            var snapshot = reservoir.GetSnapshot();

            //TODO: Assert snapshot
            //snapshot.AssertValues(_samples);
        }
    }
}