// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using Xunit;

namespace App.Metrics.Facts
{
    public class SimpleResevoirTests
    {
        private readonly IEnumerable<long> _samples;

        public SimpleResevoirTests() { _samples = new long[] { 0, 4, 1, 5 }.AsEnumerable(); }

        [Fact]
        public void ExponentialDecayingResevoir()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultExponentialDecayFactor);

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
            var reservoir = new DefaultSlidingWindowReservoir(Constants.ReservoirSampling.DefaultSampleSize);

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
            var reservoir = new DefaultAlgorithmRReservoir(Constants.ReservoirSampling.DefaultSampleSize);

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