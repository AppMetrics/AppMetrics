using App.Metrics.Core;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Sampling.Facts
{
    public class UserValueWrapperTests
    {
        [Fact]
        public void can_determine_if_user_values_are_diff()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L, "value");

            first.Should().NotBe(second);
        }

        [Fact]
        public void can_determine_if_user_values_are_diff_using_operator()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L, "value");

            Assert.False(first == second);
        }

        [Fact]
        public void can_determine_if_user_values_are_same()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L);

            first.Should().Be(second);
        }

        [Fact]
        public void can_determine_if_user_values_are_same_using_operator()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L);

            Assert.True(first == second);
        }
    }
}