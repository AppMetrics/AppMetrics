// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Apdex;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    public class ApdexValueSourceExtensionsTests
    {
        [Fact]
        public void returns_empty_apdex_when_null()
        {
            var metric = new CustomApdex();

            var apdex = metric.GetValueOrDefault();

            apdex.Frustrating.Should().Be(0);
            apdex.SampleSize.Should().Be(0);
            apdex.Tolerating.Should().Be(0);
            apdex.Satisfied.Should().Be(0);
        }
    }
}