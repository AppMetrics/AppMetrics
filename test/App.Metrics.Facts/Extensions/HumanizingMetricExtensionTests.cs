// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core;
using App.Metrics.Counter;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Gauge;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using FluentAssertions;
using Xunit;

// ReSharper disable CheckNamespace
namespace App.Metrics.Facts.Extensions
{
    // ReSharper restore CheckNamespace

    public class HumanizingMetricExtensionTests
    {
        [Fact]
        public void can_hummanize_counter()
        {
            var expected = "\n             Count = 1 items\n";
            var counterValueSource = new CounterValueSource("test_counter", ConstantValue.Provider(new CounterValue(1)), Unit.Items, MetricTags.Empty);
            var result = counterValueSource.Hummanize();

            AssertStringResult(result, expected);
        }

        [Fact]
        public void can_hummanize_env_info()
        {
            var expected =
                "     Assembly Name = assembly\n  Assembly Version = entry\n         Host Name = host\n        Local Time = time\n      Machine Name = machine\n                OS = OS\n        OS Version = OS version\n      Process Name = process\n";
            var envInfo = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var result = envInfo.Hummanize();

            AssertStringResult(result, expected);
        }

        [Fact]
        public void can_hummanize_gauge()
        {
            var expected = "\n             value = 0.50 calls\n";
            var gaugeValueSource = new GaugeValueSource("test_gauge", ConstantValue.Provider(0.5), Unit.Calls, MetricTags.Empty);
            var result = gaugeValueSource.Hummanize();

            AssertStringResult(result, expected);
        }

        [Fact]
        public void can_hummanize_health()
        {
            var expected = "\n\t\n     healthy check = PASSED: healthy message\n";
            var healthCheckResult = new HealthCheck.Result("healthy check", HealthCheckResult.Healthy("healthy message"));

            var result = healthCheckResult.Hummanize();

            AssertStringResult(result, expected);
        }

        [Fact]
        public void can_hummanize_histogram()
        {
            var expected =
                "\n             Count = 1 items\n               Sum = 1.00 items\n              Last = 2.00 items\n   Last User Value = 3\n               Min = 7.00 items\n    Min User Value = 8\n               Max = 4.00 items\n    Max User Value = 5\n              Mean = 6.00 items\n            StdDev = 9.00 items\n            Median = 10.00 items\n              75% <= 11.00 items\n              95% <= 12.00 items\n              98% <= 13.00 items\n              99% <= 14.00 items\n            99.9% <= 15.00 items\n";
            var histogramValue = new HistogramValue(1, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var histogramValueSource = new HistogramValueSource(
                "test_histgram",
                ConstantValue.Provider(histogramValue),
                Unit.Items,
                MetricTags.Empty);
            var result = histogramValueSource.Hummanize();

            AssertStringResult(result, expected);
        }

        [Fact]
        public void can_hummanize_meter()
        {
            var expected =
                "\n             Count = 5 calls\n        Mean Value = 1.00 calls/s\n     1 Minute Rate = 2.00 calls/s\n     5 Minute Rate = 3.00 calls/s\n    15 Minute Rate = 4.00 calls/s\n";
            var meterValue = new MeterValue(
                5,
                1,
                2,
                3,
                4,
                TimeUnit.Seconds,
                new[]
                {
                    new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
                });
            var meterValueSource = new MeterValueSource(
                "test_meter",
                ConstantValue.Provider(meterValue),
                Unit.Calls,
                TimeUnit.Seconds,
                MetricTags.Empty);
            var result = meterValueSource.Hummanize();

            AssertStringResult(result, expected);
        }

        [Fact]
        public void can_hummanize_timer()
        {
            var expected =
                "\n\n   Active Sessions = 0\n             Count = 5 req\n        Mean Value = 1.00 req/s\n     1 Minute Rate = 2.00 req/s\n     5 Minute Rate = 3.00 req/s\n    15 Minute Rate = 4.00 req/s\n       Total Items = 1\n            Item 0 = 00.50%     1 req [item]\n             Count = 1 req\n        Mean Value = 2.00 req/s\n     1 Minute Rate = 3.00 req/s\n     5 Minute Rate = 4.00 req/s\n    15 Minute Rate = 5.00 req/s\n\n             Count = 5 req\n               Sum = 0.00 req\n              Last = 0.00 req\n   Last User Value = 3\n               Min = 0.00 req\n    Min User Value = 8\n               Max = 0.00 req\n    Max User Value = 5\n              Mean = 0.00 req\n            StdDev = 0.00 req\n            Median = 0.00 req\n              75% <= 0.00 req\n              95% <= 0.00 req\n              98% <= 0.00 req\n              99% <= 0.00 req\n            99.9% <= 0.00 req\n";
            const int count = 5;

            var meterValue = new MeterValue(
                count,
                1,
                2,
                3,
                4,
                TimeUnit.Seconds,
                new[]
                {
                    new MeterValue.SetItem("item", 0.5, new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]))
                });
            var histogramValue = new HistogramValue(count, 1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);

            var timerValue = new TimerValue(meterValue, histogramValue, 0, TimeUnit.Nanoseconds);
            var timerValueSource = new TimerValueSource(
                "test_timer",
                ConstantValue.Provider(timerValue),
                Unit.Requests,
                TimeUnit.Seconds,
                TimeUnit.Milliseconds,
                MetricTags.Empty);
            var result = timerValueSource.Hummanize();

            AssertStringResult(result, expected);
        }

        private static void AssertStringResult(string result, string expected)
        {
            (string.CompareOrdinal(result, expected.Replace("\n", Environment.NewLine)) == 0).Should().BeTrue();
        }
    }
}