#region copyright

// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#endregion

using System.Collections.Generic;
using System.Linq;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
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

            AssertValues(snapshot);
        }

        [Fact]
        public void SlidingWindowResevoir()
        {
            var reservoir = new DefaultSlidingWindowReservoir(Constants.ReservoirSampling.DefaultSampleSize);

            foreach (var sample in _samples)
            {
                reservoir.Update(sample);
            }

            var snapshot = reservoir.GetSnapshot();

            AssertValues(snapshot);
        }

        [Fact]
        public void UniformResevoir()
        {
            var reservoir = new DefaultAlgorithmRReservoir(Constants.ReservoirSampling.DefaultSampleSize);

            foreach (var sample in _samples)
            {
                reservoir.Update(sample);
            }

            var snapshot = reservoir.GetSnapshot();

            AssertValues(snapshot);
        }

        private void AssertValues(IReservoirSnapshot snapshot)
        {
            snapshot.Count.Should().Be(4);
            snapshot.Max.Should().Be(5);

            if (snapshot is WeightedSnapshot)
            {
                snapshot.Mean.Should().Be(2.5);
                snapshot.Median.Should().Be(4.0);
                snapshot.Percentile75.Should().Be(5.0);
            }
            else
            {
                snapshot.Mean.Should().BeApproximately(2.5, 1.0);
                snapshot.Percentile75.Should().BeApproximately(4.75, 2.0);
                snapshot.Median.Should().BeApproximately(2.5, 1.0);
            }

            snapshot.Min.Should().Be(0);
            snapshot.Percentile95.Should().Be(5.0);
            snapshot.Percentile98.Should().Be(5.0);
            snapshot.Percentile99.Should().Be(5.0);
            snapshot.Percentile999.Should().Be(5.0);
            snapshot.Size.Should().Be(4);
            snapshot.StdDev.Should().BeApproximately(2.3, 1);
        }
    }
}