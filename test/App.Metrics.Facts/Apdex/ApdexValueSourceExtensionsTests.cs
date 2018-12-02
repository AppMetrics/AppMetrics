// <copyright file="ApdexValueSourceExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Apdex;
using App.Metrics.Facts.TestHelpers;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Apdex
{
    public class ApdexValueSourceExtensionsTests
    {
        [Fact]
        public void Returns_empty_apdex_when_null()
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