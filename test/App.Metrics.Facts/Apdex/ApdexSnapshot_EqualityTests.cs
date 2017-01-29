// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    public class ApdexSnapshot_EqualityTests
    {
        [Fact]
        public void equality_with_equals_false_when_not_same()
        {
            var snapshot = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L }, 0.5);

            snapshot.Equals(other).Should().Be(false);
        }

        [Fact]
        public void equality_with_equals_operator()
        {
            var snapshot = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);

            (snapshot == other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_true_when_same()
        {
            var snapshot = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);

            snapshot.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_equals_true_when_same_object()
        {
            var snapshot = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            object other = snapshot;

            snapshot.Equals(other).Should().Be(true);
        }

        [Fact]
        public void equality_with_not_equals_operator()
        {
            var snapshot = new ApdexSnapshot(new[] { 1L, 2L, 3L }, 0.5);
            var other = new ApdexSnapshot(new[] { 1L, 2L }, 0.5);

            (snapshot != other).Should().Be(true);
        }
    }
}