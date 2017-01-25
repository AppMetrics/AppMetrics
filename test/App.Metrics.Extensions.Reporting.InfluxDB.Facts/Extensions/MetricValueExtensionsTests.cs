// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.Apdex;
using App.Metrics.Extensions.Reporting.InfluxDB.Extensions;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.ReservoirSampling.ExponentialDecay;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Middleware.Integration.Facts.Extensions
{
    public class MetricValueExtensionsTests
    {
        private readonly Lazy<IReservoir> _defaultReservoir = new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir());

        [Fact]
        public void can_add_apdex_values()
        {
            var clock = new TestClock();
            var apdex = new DefaultApdexMetric(_defaultReservoir, clock, true);
            apdex.Track(10000);
            var values = new Dictionary<string, object>();
            apdex.Value.AddApdexValues(values);

            values.Keys.Should().Contain("samples");
            values.Keys.Should().Contain("score");
            values.Keys.Should().Contain("satisfied");
            values.Keys.Should().Contain("tolerating");
            values.Keys.Should().Contain("frustrating");
        }

        [Fact]
        public void can_add_histgoram_values()
        {
            var histogramMetric = new DefaultHistogramMetric(_defaultReservoir);
            histogramMetric.Update(10000, "value");
            var values = new Dictionary<string, object>();
            histogramMetric.Value.AddHistogramValues(values);

            values.Keys.Should().Contain("samples");
            values.Keys.Should().Contain("last");
            values.Keys.Should().Contain("count.hist");
            values.Keys.Should().Contain("min");
            values.Keys.Should().Contain("max");
            values.Keys.Should().Contain("mean");
            values.Keys.Should().Contain("median");
            values.Keys.Should().Contain("stddev");
            values.Keys.Should().Contain("p999");
            values.Keys.Should().Contain("p99");
            values.Keys.Should().Contain("p98");
            values.Keys.Should().Contain("p95");
            values.Keys.Should().Contain("p75");
            values.Keys.Should().Contain("user.last");
            values.Keys.Should().Contain("user.min");
            values.Keys.Should().Contain("user.max");
        }
    }
}