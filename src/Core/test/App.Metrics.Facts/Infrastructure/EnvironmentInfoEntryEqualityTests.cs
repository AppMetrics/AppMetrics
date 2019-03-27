// <copyright file="EnvironmentInfoEntryEqualityTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Infrastructure
{
    public class EnvironmentInfoEntryEqualityTests
    {
        [Theory]
        [InlineData("name", "value", true)]
        [InlineData("name1", "value", false)]
        [InlineData("name", "value1", false)]
        public void Equality_with_equals(string name, string value, bool expected)
        {
            var env = new EnvironmentInfoEntry("name", "value");
            var other = new EnvironmentInfoEntry(name, value);

            env.Equals(other).Should().Be(expected);
        }

        [Fact]
        public void Equality_with_equals_false_when_same_object()
        {
            var env = new EnvironmentInfoEntry("name", "value");

            object other = env;

            env.Equals(other).Should().Be(true);
        }

        [Theory]
        [InlineData("name", "value", true)]
        [InlineData("name1", "value", false)]
        [InlineData("name", "value1", false)]
        public void Equality_with_equals_operator(string name, string value, bool expected)
        {
            var env = new EnvironmentInfoEntry("name", "value");
            var other = new EnvironmentInfoEntry(name, value);

            (env == other).Should().Be(expected);
        }

        [Theory]
        [InlineData("name", "value", true)]
        [InlineData("name1", "value", false)]
        [InlineData("name", "value1", false)]
        public void Equality_with_not_equals_operator(string name, string value, bool expected)
        {
            var env = new EnvironmentInfoEntry("name", "value");
            var other = new EnvironmentInfoEntry(name, value);

            (env != other).Should().Be(!expected);
        }
    }
}