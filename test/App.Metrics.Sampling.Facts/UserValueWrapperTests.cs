// <copyright file="UserValueWrapperTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using FluentAssertions;
using Xunit;

namespace App.Metrics.Sampling.Facts
{
    public class UserValueWrapperTests
    {
        [Fact]
        public void Can_determine_if_user_values_are_diff()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L, "value");

            first.Should().NotBe(second);
        }

        [Fact]
        public void Can_determine_if_user_values_are_diff_using_operator()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L, "value");

            Assert.False(first == second);
        }

        [Fact]
        public void Can_determine_if_user_values_are_same()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L);

            first.Should().Be(second);
        }

        [Fact]
        public void Can_determine_if_user_values_are_same_using_operator()
        {
            var first = new UserValueWrapper(1L);
            var second = new UserValueWrapper(1L);

            Assert.True(first == second);
        }
    }
}