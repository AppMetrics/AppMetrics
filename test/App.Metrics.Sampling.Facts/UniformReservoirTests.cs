using App.Metrics.Sampling;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts
{
    public class UniformReservoirTests
    {
        [Fact]
        public void of_100_out_of_1000_elements()
        {
            var reservoir = new UniformReservoir(100);

            for (var i = 0; i < 1000; i++)
            {
                reservoir.Update(i);
            }

            reservoir.Size.Should().Be(100);
            reservoir.GetSnapshot().Size.Should().Be(100);
            reservoir.GetSnapshot().Values.Should().OnlyContain(v => 0 <= v && v < 1000);
        }

        [Fact]
        public void can_records_user_value()
        {
            var reservoir = new UniformReservoir(100);

            reservoir.Update(2L, "B");
            reservoir.Update(1L, "A");

            reservoir.GetSnapshot().MinUserValue.Should().Be("A");
            reservoir.GetSnapshot().MaxUserValue.Should().Be("B");
        }
    }
}