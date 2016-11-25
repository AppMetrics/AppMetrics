using FluentAssertions;
using Xunit;

namespace App.Metrics.Concurrency.Facts
{
    public class ThreadLocalTests
    {
        [Fact]
        public void ThreadLocalRandom_RespectsMaxValue()
        {
            ThreadLocalRandom.Next(0).Should().Be(0);
            ThreadLocalRandom.Next(0, 0).Should().Be(0);

            ThreadLocalRandom.Next(1).Should().BeInRange(0, 1);
            ThreadLocalRandom.Next(0, 1).Should().BeInRange(0, 1);

            ThreadLocalRandom.NextLong(0L).Should().Be(0L);
            ThreadLocalRandom.NextLong(1L).Should().BeInRange(0L, 1L);
        }

        [Fact]
        public void ThreadLocalRandom_RespectsMinValue()
        {
            ThreadLocalRandom.Next(int.MaxValue - 1, int.MaxValue - 1).Should().Be(int.MaxValue - 1);
        }
    }
}