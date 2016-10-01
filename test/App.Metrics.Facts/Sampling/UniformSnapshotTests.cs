using System;
using System.Linq;
using App.Metrics.Sampling;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Sampling
{
    public class UniformSnapshotTest
    {
        private readonly UniformSnapshot snapshot = new UniformSnapshot(5, new [] { 5L, 1, 2, 3, 4 });

        [Fact]
        public void UniformSnapshot_SmallQuantilesAreTheFirstValue()
        {
            snapshot.GetValue(0.0).Should().BeApproximately(1, 0.1);
        }

        [Fact]
        public void UniformSnapshot_BigQuantilesAreTheLastValue()
        {
            snapshot.GetValue(1.0).Should().BeApproximately(5, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAMedian()
        {
            snapshot.Median.Should().BeApproximately(3, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp75()
        {
            snapshot.Percentile75.Should().BeApproximately(4.5, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp95()
        {
            snapshot.Percentile95.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp98()
        {
            snapshot.Percentile98.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp99()
        {
            snapshot.Percentile99.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp999()
        {
            snapshot.Percentile999.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasValues()
        {
            snapshot.Values.Should().ContainInOrder(1L, 2L, 3L, 4L, 5L);
        }

        [Fact]
        public void UniformSnapshot_HasASize()
        {
            snapshot.Size.Should().Be(5);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheMinimumValue()
        {
            snapshot.Min.Should().Be(1);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheMaximumValue()
        {
            snapshot.Max.Should().Be(5);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheMeanValue()
        {
            snapshot.Mean.Should().Be(3.0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheStdDev()
        {
            snapshot.StdDev.Should().BeApproximately(1.5811, 0.0001);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAMinOfZeroForAnEmptySnapshot()
        {
            Snapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.Min.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAMaxOfZeroForAnEmptySnapshot()
        {
            Snapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.Max.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAMeanOfZeroForAnEmptySnapshot()
        {
            Snapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.Mean.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAStdDevOfZeroForAnEmptySnapshot()
        {
            Snapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.StdDev.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAStdDevOfZeroForASingletonSnapshot()
        {
            Snapshot snapshot = new UniformSnapshot(0, new[] { 1L });
            snapshot.StdDev.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_ThrowsOnBadQuantileValue()
        {
            ((Action)(() => snapshot.GetValue(-0.5))).ShouldThrow<ArgumentException>();
            ((Action)(() => snapshot.GetValue(1.5))).ShouldThrow<ArgumentException>();
            ((Action)(() => snapshot.GetValue(double.NaN))).ShouldThrow<ArgumentException>();
        }
    }
}
