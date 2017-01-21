// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Extensions.Reservoirs.HdrHistogram.HdrHistogram;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Reservoirs.HdrHistogram.Facts
{
    public class HdrHistogramResevoirTests
    {
        [Fact]
        public void can_get_snap_shot_values()
        {
            var reservoir = new HdrHistogramReservoir(new Recorder(2));

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            var snapshot = reservoir.GetSnapshot();

            snapshot.Values.Should().Contain(new[] { 2L, 1L });
        }

        [Fact]
        public void can_records_user_value()
        {
            var reservoir = new HdrHistogramReservoir(new Recorder(2));

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            var snapshot = reservoir.GetSnapshot();

            snapshot.MinUserValue.Should().Be("A");
            snapshot.Min.Should().Be(1L);
            snapshot.MaxUserValue.Should().Be("B");
            snapshot.Max.Should().Be(2L);
        }

        [Fact]
        public void snap_shot_reflects_what_was_recorded()
        {
            var reservoir = new HdrHistogramReservoir(new Recorder(2));

            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(i);
            }

            var snapshot = reservoir.GetSnapshot();

            snapshot.Count.Should().Be(1000);
            snapshot.Median.Should().Be(499);
            snapshot.Mean.Should().BeApproximately(499, 1);
            snapshot.Min.Should().Be(0);
            snapshot.Max.Should().Be(999);
            snapshot.Size.Should().Be(4608);
            snapshot.Percentile75.Should().Be(751);
            snapshot.Percentile95.Should().Be(951);
            snapshot.Percentile98.Should().Be(979);
            snapshot.Percentile99.Should().Be(991);
            snapshot.Percentile999.Should().Be(999);
        }
    }
}