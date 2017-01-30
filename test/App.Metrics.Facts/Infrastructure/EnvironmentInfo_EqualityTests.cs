// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Infrastructure
{
    public class EnvironmentInfo_EqualityTests
    {
        [Fact]
        public void equality_with_equals()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var other = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            env.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_false_when_same_object()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            object other = env;

            env.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_operator()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var other = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            (env == other).Should().Be(true);
        }

        [Fact]
        public void equality_with_not_equals_operator()
        {
            var env = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var other = new EnvironmentInfo("assembly2", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");

            (env != other).Should().Be(true);
        }
    }
}