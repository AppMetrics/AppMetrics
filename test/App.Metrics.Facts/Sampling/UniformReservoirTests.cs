using App.Metrics.Sampling;
using Xunit;
using FluentAssertions;

namespace App.Metrics.Facts.Sampling
{
    public class UniformReservoirTests
    {
        [Fact]
        public void UniformReservoir_Of100OutOf1000Elements()
        {
            UniformReservoir reservoir = new UniformReservoir(100);

            for (int i = 0; i < 1000; i++)
            {
                reservoir.Update(i);
            }

            reservoir.Size.Should().Be(100);
            reservoir.GetSnapshot().Size.Should().Be(100);
            reservoir.GetSnapshot().Values.Should().OnlyContain(v => 0 <= v && v < 1000);
        }

        [Fact]
        public void UniformReservoir_RecordsUserValue()
        {
            UniformReservoir reservoir = new UniformReservoir(100);

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            reservoir.GetSnapshot().MinUserValue.Should().Be("A");
            reservoir.GetSnapshot().MaxUserValue.Should().Be("B");
        }
    }
}
