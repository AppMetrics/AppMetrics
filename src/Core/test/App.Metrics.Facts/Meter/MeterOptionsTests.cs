// <copyright file="MeterOptionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Meter
{
    public class MeterOptionsTests
    {
        private readonly MeterOptions _meterOptions;

        public MeterOptionsTests()
        {
            _meterOptions = new MeterOptions();
        }

        [Fact]
        public void Has_correct_default_values()
        {
            _meterOptions.RateUnit.Should().Be(TimeUnit.Minutes);
            _meterOptions.ReportSetItems.Should().BeTrue();
        }
    }
}
