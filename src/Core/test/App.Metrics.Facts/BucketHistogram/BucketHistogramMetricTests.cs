// <copyright file="HistogramMetricTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics.BucketHistogram;
using App.Metrics.Facts.TestHelpers;
using App.Metrics.Histogram;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.BucketHistogram
{
    public class BucketHistogramMetricTests
    {
        private readonly DefaultBucketHistogramMetric _histogram;

        public BucketHistogramMetricTests() { _histogram = new DefaultBucketHistogramMetric(new[] { 10d, 100d, 1000d }); }

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
            _histogram.Update(10L);
            _histogram.Update(100L);
            _histogram.Update(1000L);
            _histogram.Update(10000L);

            _histogram.Value.Count.Should().NotBe(0);
            _histogram.Value.Sum.Should().NotBe(0);
            _histogram.Value.Buckets[10].Should().NotBe(0);
            _histogram.Value.Buckets[100].Should().NotBe(0);
            _histogram.Value.Buckets[1000].Should().NotBe(0);
            _histogram.Value.Buckets[double.PositiveInfinity].Should().NotBe(0);

            _histogram.Reset();

            _histogram.Value.Count.Should().Be(0);
            _histogram.Value.Sum.Should().Be(0);
            _histogram.Value.Buckets[10].Should().Be(0);
            _histogram.Value.Buckets[100].Should().Be(0);
            _histogram.Value.Buckets[1000].Should().Be(0);
            _histogram.Value.Buckets[double.PositiveInfinity].Should().Be(0);
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
        public void Records_lifetime_count_of_observed_values()
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Parallel.For(
                1,
                10001,
                options,
                i => _histogram.Update(i));

            _histogram.Value.Count.Should().Be(10000L);
        }

        [Fact]
        public void Records_lifetime_count_of_each_bucket_values()
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            Parallel.For(
                1,
                10001,
                options,
                i => _histogram.Update(i));

            _histogram.Value.Buckets[10].Should().Be(Enumerable.Range(1, 10).Count());
            _histogram.Value.Buckets[100].Should().Be(Enumerable.Range(11, 90).Count());
            _histogram.Value.Buckets[1000].Should().Be(Enumerable.Range(101, 900).Count());
            _histogram.Value.Buckets[double.PositiveInfinity].Should().Be(Enumerable.Range(1001, 9000).Count());
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