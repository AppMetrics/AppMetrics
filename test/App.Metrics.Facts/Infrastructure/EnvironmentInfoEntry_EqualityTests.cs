// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Infrastructure
{
    public class EnvironmentInfoEntry_EqualityTests
    {
        [Theory]
        [InlineData("name", "value", true)]
        [InlineData("name1", "value", false)]
        [InlineData("name", "value1", false)]
        public void equality_with_equals(string name, string value, bool expected)
        {
            var env = new EnvironmentInfoEntry("name", "value");
            var other = new EnvironmentInfoEntry(name, value);

            env.Equals(other).Should().Be(expected);
        }

        [Fact]
        public void equality_with_equals_false_when_same_object()
        {
            var env = new EnvironmentInfoEntry("name", "value");

            object other = env;

            env.Equals(other).Should().Be(true);
        }

        [Theory]
        [InlineData("name", "value", true)]
        [InlineData("name1", "value", false)]
        [InlineData("name", "value1", false)]
        public void equality_with_equals_operator(string name, string value, bool expected)
        {
            var env = new EnvironmentInfoEntry("name", "value");
            var other = new EnvironmentInfoEntry(name, value);

            (env == other).Should().Be(expected);
        }

        [Theory]
        [InlineData("name", "value", true)]
        [InlineData("name1", "value", false)]
        [InlineData("name", "value1", false)]
        public void equality_with_not_equals_operator(string name, string value, bool expected)
        {
            var env = new EnvironmentInfoEntry("name", "value");
            var other = new EnvironmentInfoEntry(name, value);

            (env != other).Should().Be(!expected);
        }
    }
}