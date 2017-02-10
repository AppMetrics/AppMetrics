// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Counter;
using App.Metrics.Data;
using App.Metrics.Formatting.Humanize;
using App.Metrics.Gauge;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class HumanizingMetricExtensionTests
    {
        [Fact]
        public void can_hummanize_counter()
        {
            var expected = "\r\n             Count = 1\r\n";
            var counterValueSource = new CounterValueSource("test_counter", ConstantValue.Provider(new CounterValue(1)), Unit.None, MetricTags.Empty);
            var result = counterValueSource.Hummanize();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void can_hummanize_env_info()
        {
            var expected =
                "     Assembly Name = assembly\r\n  Assembly Version = entry\r\n         Host Name = host\r\n        Local Time = time\r\n      Machine Name = machine\r\n                OS = OS\r\n        OS Version = OS version\r\n      Process Name = process\r\n";
            var envInfo = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var result = envInfo.Hummanize();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void can_hummanize_gauge()
        {
            var expected = "\r\n             value = 0.50 Calls\r\n";
            var gaugeValueSource = new GaugeValueSource("test_gauge", ConstantValue.Provider(0.5), Unit.Calls, MetricTags.Empty);
            var result = gaugeValueSource.Hummanize();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void can_hummanize_health()
        {
            var expected = "\r\n\t\r\n     healthy check = PASSED: healthy message\r\n";
            var healthCheckResult = new HealthCheck.Result("healthy check", HealthCheckResult.Healthy("healthy message"));

            var result = healthCheckResult.Hummanize();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void can_hummanize_histogram()
        {
            var expected =
                "\r\n             Count = 1 Items\r\n              Last = 2.00 Items\r\n   Last User Value = 3\r\n               Min = 7.00 Items\r\n    Min User Value = 8\r\n               Max = 4.00 Items\r\n    Max User Value = 5\r\n              Mean = 6.00 Items\r\n            StdDev = 9.00 Items\r\n            Median = 10.00 Items\r\n              75% <= 11.00 Items\r\n              95% <= 12.00 Items\r\n              98% <= 13.00 Items\r\n              99% <= 14.00 Items\r\n            99.9% <= 15.00 Items\r\n";
            var histogramValue = new HistogramValue(1, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);
            var histogramValueSource = new HistogramValueSource("test_histgram", ConstantValue.Provider(histogramValue), Unit.Items, MetricTags.Empty);
            var result = histogramValueSource.Hummanize();

            Assert.Equal(result, expected);
        }

        [Fact]
        public void can_hummanize_meter()
        {
            var expected =
                "\r\n             Count = 5 Calls\r\n        Mean Value = 1.00 Calls/s\r\n     1 Minute Rate = 2.00 Calls/s\r\n     5 Minute Rate = 3.00 Calls/s\r\n    15 Minute Rate = 4.00 Calls/s\r\n";
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

            Assert.Equal(result, expected);
        }

        [Fact]
        public void can_hummanize_timer()
        {
            var expected =
                "\r\n\r\n   Active Sessions = 0\r\n        Total Time = 0.00 ms\r\n             Count = 5 Requests\r\n        Mean Value = 1.00 Requests/s\r\n     1 Minute Rate = 2.00 Requests/s\r\n     5 Minute Rate = 3.00 Requests/s\r\n    15 Minute Rate = 4.00 Requests/s\r\n       Total Items = 1\r\n            Item 0 = 00.50%     1 Requests [item]\r\n             Count = 1 Requests\r\n        Mean Value = 2.00 Requests/s\r\n     1 Minute Rate = 3.00 Requests/s\r\n     5 Minute Rate = 4.00 Requests/s\r\n    15 Minute Rate = 5.00 Requests/s\r\n\r\n             Count = 5 Requests\r\n              Last = 0.00 Requests\r\n   Last User Value = 3\r\n               Min = 0.00 Requests\r\n    Min User Value = 8\r\n               Max = 0.00 Requests\r\n    Max User Value = 5\r\n              Mean = 0.00 Requests\r\n            StdDev = 0.00 Requests\r\n            Median = 0.00 Requests\r\n              75% <= 0.00 Requests\r\n              95% <= 0.00 Requests\r\n              98% <= 0.00 Requests\r\n              99% <= 0.00 Requests\r\n            99.9% <= 0.00 Requests\r\n";
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
            var histogramValue = new HistogramValue(count, 2, "3", 4, "5", 6, 7, "8", 9, 10, 11, 12, 13, 14, 15, 16);

            var timerValue = new TimerValue(meterValue, histogramValue, 0, 1, TimeUnit.Nanoseconds);
            var timerValueSource = new TimerValueSource(
                "test_timer",
                ConstantValue.Provider(timerValue),
                Unit.Requests,
                TimeUnit.Seconds,
                TimeUnit.Milliseconds,
                MetricTags.Empty);
            var result = timerValueSource.Hummanize();

            Assert.Equal(result, expected);
        }
    }
}