// <copyright file="SimpleReservoirTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using App.Metrics.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Sampling.Facts
{
    public class SimpleReservoirTests
    {
        private readonly IEnumerable<long> _samples;

        public SimpleReservoirTests() { _samples = new long[] { 0, 4, 1, 5 }.AsEnumerable(); }

        [Fact]
        public void ExponentialDecayingReservoir()
        {
            var reservoir = new DefaultForwardDecayingReservoir(
                AppMetricsReservoirSamplingConstants.DefaultSampleSize,
                AppMetricsReservoirSamplingConstants.DefaultExponentialDecayFactor);

            foreach (var sample in _samples)
            {
                reservoir.Update(sample);
            }

            var snapshot = reservoir.GetSnapshot();

            AssertValues(snapshot);
        }

        [Fact]
        public void SlidingWindowReservoir()
        {
            var reservoir = new DefaultSlidingWindowReservoir(AppMetricsReservoirSamplingConstants.DefaultSampleSize);

            foreach (var sample in _samples)
            {
                reservoir.Update(sample);
            }

            var snapshot = reservoir.GetSnapshot();

            AssertValues(snapshot);
        }

        [Fact]
        public void UniformReservoir()
        {
            var reservoir = new DefaultAlgorithmRReservoir(AppMetricsReservoirSamplingConstants.DefaultSampleSize);

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
                snapshot.Mean.Should().BeApproximately(2.5, 1);
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
            snapshot.Percentile95.Should().BeApproximately(5.0, 1);
            snapshot.Percentile98.Should().BeApproximately(5.0, 1);
            snapshot.Percentile99.Should().BeApproximately(5.0, 1);
            snapshot.Percentile999.Should().BeApproximately(5.0, 1);
            snapshot.Size.Should().Be(4);
            snapshot.StdDev.Should().BeApproximately(2.3, 1);
        }
    }
}