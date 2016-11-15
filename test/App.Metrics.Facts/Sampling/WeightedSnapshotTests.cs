using System;
using System.Linq;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Sampling
{
    public class WeightedSnapshotTests
    {
        private readonly WeightedSnapshot _snapshot = MakeSanpshot(new long[] { 5, 1, 2, 3, 4 }, new double[] { 1, 2, 3, 2, 2 });

        [Fact]
        public void WeightedSnapshot_BigQuantilesAreTheLastValue()
        {
            _snapshot.GetValue(1.0).Should().Be(5.0);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesAMaxOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = MakeSanpshot(new long[0], new double[0]);
            snapshot.Max.Should().Be(0);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesAMeanOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = MakeSanpshot(new long[0], new double[0]);
            snapshot.Mean.Should().Be(0);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesAMinOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = MakeSanpshot(new long[0], new double[0]);
            snapshot.Min.Should().Be(0);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesAStdDevOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = MakeSanpshot(new long[0], new double[0]);
            snapshot.StdDev.Should().Be(0);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesAStdDevOfZeroForASingletonSnapshot()
        {
            ISnapshot snapshot = MakeSanpshot(new long[0], new double[0]);
            snapshot.StdDev.Should().Be(0);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesTheMaximumValue()
        {
            _snapshot.Max.Should().Be(5);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesTheMeanValue()
        {
            _snapshot.Mean.Should().Be(2.7);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesTheMinimumValue()
        {
            _snapshot.Min.Should().Be(1);
        }

        [Fact]
        public void WeightedSnapshot_CalculatesTheStdDev()
        {
            _snapshot.StdDev.Should().BeApproximately(1.2688, 0.0001);
        }

        [Fact]
        public void WeightedSnapshot_HasAMedian()
        {
            _snapshot.Median.Should().Be(3.0);
        }

        [Fact]
        public void WeightedSnapshot_HasAp75()
        {
            _snapshot.Percentile75.Should().Be(4.0);
        }

        [Fact]
        public void WeightedSnapshot_HasAp95()
        {
            _snapshot.Percentile95.Should().Be(5.0);
        }

        [Fact]
        public void WeightedSnapshot_HasAp98()
        {
            _snapshot.Percentile98.Should().Be(5.0);
        }

        [Fact]
        public void WeightedSnapshot_HasAp99()
        {
            _snapshot.Percentile99.Should().Be(5.0);
        }

        [Fact]
        public void WeightedSnapshot_HasAp999()
        {
            _snapshot.Percentile999.Should().Be(5.0);
        }

        [Fact]
        public void WeightedSnapshot_HasSize()
        {
            _snapshot.Size.Should().Be(5);
        }

        [Fact]
        public void WeightedSnapshot_HasValues()
        {
            _snapshot.Values.Should().Equal(new long[] { 1, 2, 3, 4, 5 });
        }


        [Fact]
        public void WeightedSnapshot_SmallQuantilesAreTheFirstValue()
        {
            _snapshot.GetValue(0.0).Should().Be(1.0);
        }

        [Fact]
        public void WeightedSnapshot_ThrowsOnBadQuantileValue()
        {
            ((Action)(() => _snapshot.GetValue(-0.5))).ShouldThrow<ArgumentException>();
            ((Action)(() => _snapshot.GetValue(1.5))).ShouldThrow<ArgumentException>();
            ((Action)(() => _snapshot.GetValue(double.NaN))).ShouldThrow<ArgumentException>();
        }

        private static WeightedSnapshot MakeSanpshot(long[] values, double[] weights)
        {
            if (values.Length != weights.Length)
            {
                throw new ArgumentException("values and weights must have same number of elements");
            }

            var samples = Enumerable.Range(0, values.Length).Select(i => new WeightedSample(values[i], null, weights[i]));

            return new WeightedSnapshot(values.Length, samples);
        }
    }
}