// <copyright file="UniformReservoirTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.ReservoirSampling.Uniform;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Sampling.Facts
{
    public class UniformReservoirTests
    {
        [Fact]
        public void Can_records_user_value()
        {
            var reservoir = new DefaultAlgorithmRReservoir(100);

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            reservoir.GetSnapshot().MinUserValue.Should().Be("A");
            reservoir.GetSnapshot().MaxUserValue.Should().Be("B");
        }

        [Fact]
        public void Of_100_out_of_1000_elements()
        {
            var reservoir = new DefaultAlgorithmRReservoir(100);

            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(i);
            }

            reservoir.Size.Should().Be(100);
            reservoir.GetSnapshot().Size.Should().Be(100);
            reservoir.GetSnapshot().Values.Should().OnlyContain(v => v >= 0 && v < 1000);
        }
    }
}