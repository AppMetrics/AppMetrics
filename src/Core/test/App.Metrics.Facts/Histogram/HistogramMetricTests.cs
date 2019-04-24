// <copyright file="HistogramMetricTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.Histogram;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Histogram
{
    public class HistogramMetricTests
    {
        private readonly DefaultHistogramMetric _histogram;

        public HistogramMetricTests() { _histogram = new DefaultHistogramMetric(new DefaultForwardDecayingReservoir()); }

        [Fact]
        public void Can_count()
        {
            _histogram.Update(1L);
            _histogram.Value.Count.Should().Be(1);
            _histogram.Update(1L);
            _histogram.Value.Count.Should().Be(2);
        }

        [Fact]
        public void Can_reset()
        {
            _histogram.Update(1L);
            _histogram.Update(10L);

            _histogram.Value.Count.Should().NotBe(0);
            _histogram.Value.LastValue.Should().NotBe(0);
            _histogram.Value.Median.Should().NotBe(0);

            _histogram.Reset();

            _histogram.Value.Count.Should().Be(0);
            _histogram.Value.LastValue.Should().Be(0);
            _histogram.Value.Median.Should().Be(0);
        }

        [Fact]
        public void Records_lifetime_sum_of_observed_values()
        {
            double sum = Enumerable.Range(1, 10000).Sum();
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Parallel.For(
                1,
                10001,
                options,
                i => _histogram.Update(i));

            _histogram.Value.Sum.Should().Be(sum);
        }

        [Fact]
        public void Records_mean_for_one_element()
        {
            _histogram.Update(1L);
            _histogram.Value.Mean.Should().Be(1);
        }

        [Fact]
        public void Records_user_value()
        {
            _histogram.Update(1L, "A");
            _histogram.Update(10L, "B");

            _histogram.GetValueOrDefault().MinUserValue.Should().Be("A");
            _histogram.GetValueOrDefault().MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void Returns_empty_histogram_if_not_histogram_metric()
        {
            var histogram = new CustomHistogram();
            var value = histogram.GetValueOrDefault();
            value.Should().NotBeNull();
        }
    }
}