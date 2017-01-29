// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Metrics
{
    public class ApdexSnapshot_EqualityTests
    {
        [Fact]
        public void equality_with_equals_false_when_not_same()
        {
            var env = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L }, 0.5);

            env.Equals(other).Should().Be(false);
        }

        [Fact]
        public void equality_with_equals_operator()
        {
            var env = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);

            env.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_true_when_same()
        {
            var env = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);

            env.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_not_equals_operator()
        {
            var env = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L }, 0.5);

            env.Equals(other).Should().Be(false);
        }
    }
}