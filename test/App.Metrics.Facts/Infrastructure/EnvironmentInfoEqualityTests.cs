// <copyright file="EnvironmentInfoEqualityTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Infrastructure;
using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Infrastructure
{
    public class EnvironmentInfoEqualityTests
    {
        [Fact]
        public void Different_hashcodes_when_values_differ()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var env2 = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS2", "OS version2", "process", "4");

            (env.GetHashCode() == env2.GetHashCode()).Should().BeFalse();
        }

        [Fact]
        public void Equality_with_equals()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var other = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            env.Equals(other).Should().Be(true);
        }

        [Fact]
        public void Equality_with_equals_false_when_same_object()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            object other = env;

            env.Equals(other).Should().Be(true);
        }

        [Fact]
        public void Equality_with_equals_operator()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var other = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            (env == other).Should().Be(true);
        }

        [Fact]
        public void Equality_with_not_equals_operator()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var other = new EnvironmentInfo("assembly2", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            (env != other).Should().Be(true);
        }

        [Fact]
        public void Same_hashcodes_when_values_match()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var env2 = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            (env.GetHashCode() == env2.GetHashCode()).Should().BeTrue();
        }
    }
}