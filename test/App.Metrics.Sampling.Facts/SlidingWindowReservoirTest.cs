// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using System.Reflection;
using App.Metrics.Core;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts
{
    public class SlidingWindowReservoirTest
    {
        private readonly DefaultSlidingWindowReservoir _reservoir = new DefaultSlidingWindowReservoir(3);

        [Fact]
        public void can_store_small_sample()
        {
            _reservoir.Update(1L);
            _reservoir.Update(2L);

            _reservoir.GetSnapshot().Values.Should().ContainInOrder(1L, 2L);
        }

        [Fact]
        public void can_reset()
        {
            _reservoir.Update(1L);
            _reservoir.Reset();
            _reservoir.Update(2L);
            _reservoir.Update(3L);

            _reservoir.GetSnapshot().Values.Should().NotContain(1L);
            _reservoir.GetSnapshot().Values.Should().ContainInOrder(2L, 3L);
        }

        [Fact]
        public void only_stores_last_values()
        {
            _reservoir.Update(1L);
            _reservoir.Update(2L);
            _reservoir.Update(3L);
            _reservoir.Update(4L);
            _reservoir.Update(5L);

            _reservoir.GetSnapshot().Values.Should().ContainInOrder(3L, 4L, 5L);
        }

        [Fact]
        public void returns_empty_uniform_snapshot_when_zero_size()
        {
            var reservoir = new DefaultSlidingWindowReservoir(0);

            reservoir.GetSnapshot().Should().BeOfType<UniformSnapshot>();
            reservoir.GetSnapshot().Size.Should().Be(0);
            reservoir.GetSnapshot().Values.Should().BeEmpty();
        }

        [Fact]
        public void can_reset_reservoir_when_getting_snapshot()
        {
            _reservoir.Update(1L);
            _reservoir.Update(2L);

            _reservoir.GetSnapshot().Values.Should().ContainInOrder(1L, 2L);

            var snapshotAfterReset = _reservoir.GetSnapshot(true);

            snapshotAfterReset.Values.Should().ContainInOrder(1L, 2L);
            snapshotAfterReset.Count.Should().Be(2);

            var emptySnapshot = _reservoir.GetSnapshot();

            emptySnapshot.Count.Should().Be(0);
            emptySnapshot.Values.Should().BeEmpty();
        }

        [Fact]
        public void records_user_value()
        {
            _reservoir.Update(2L, "B");
            _reservoir.Update(1L, "A");

            _reservoir.GetSnapshot().MinUserValue.Should().Be("A");
            _reservoir.GetSnapshot().MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void parameterless_ctor_uses_default_sample_size()
        {
            var reservoir = new DefaultSlidingWindowReservoir();

            var fields = typeof(DefaultSlidingWindowReservoir).GetFields(BindingFlags.Instance
                                           | BindingFlags.GetField
                                           | BindingFlags.SetField
                                           | BindingFlags.NonPublic);

            var field = fields.FirstOrDefault(feildInfo => feildInfo.Name == "_values");

            ((UserValueWrapper[])field.GetValue(reservoir)).Length.Should().Be(Constants.ReservoirSampling.DefaultSampleSize);
        }
    }
}