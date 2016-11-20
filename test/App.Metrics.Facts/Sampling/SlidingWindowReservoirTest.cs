using App.Metrics.Sampling;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Sampling
{
    public class SlidingWindowReservoirTest
    {
        private readonly SlidingWindowReservoir _reservoir = new SlidingWindowReservoir(3);

        [Fact]
        public void can_store_small_sample()
        {
            _reservoir.Update(1L);
            _reservoir.Update(2L);

            _reservoir.GetSnapshot().Values.Should().ContainInOrder(1L, 2L);
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
        public void records_user_value()
        {
            _reservoir.Update(2L, "B");
            _reservoir.Update(1L, "A");

            _reservoir.GetSnapshot().MinUserValue.Should().Be("A");
            _reservoir.GetSnapshot().MaxUserValue.Should().Be("B");
        }
    }
}