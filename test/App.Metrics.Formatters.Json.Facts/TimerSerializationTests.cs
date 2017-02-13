// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Timer;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class TimerSerializationTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly MetricDataSerializer _serializer;
        private readonly TimerValueSource _timer;
        private readonly TimerValueSource _timerWithGroup;

        public TimerSerializationTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _serializer = new MetricDataSerializer();
            _timer = fixture.Timers.First(x => x.Name == fixture.TimerNameDefault);
            _timerWithGroup = fixture.Timers.First(x => x.Name == fixture.TimerNameWithGroup);
        }

        [Fact]
        public void can_deserialize()
        {
            var jsonTimer = MetricType.Timer.SampleJson();

            var result = _serializer.Deserialize<TimerValueSource>(jsonTimer.ToString());

            result.Name.Should().BeEquivalentTo(_timer.Name);
            result.Unit.Should().Be(_timer.Unit);
            result.Value.Histogram.Count.Should().Be(_timer.Value.Histogram.Count);
            result.Value.Histogram.LastValue.Should().Be(_timer.Value.Histogram.LastValue);
            result.Value.Histogram.LastUserValue.Should().Be(_timer.Value.Histogram.LastUserValue);
            result.Value.Histogram.Max.Should().Be(_timer.Value.Histogram.Max);
            result.Value.Histogram.MaxUserValue.Should().Be(_timer.Value.Histogram.MaxUserValue);
            result.Value.Histogram.Mean.Should().Be(_timer.Value.Histogram.Mean);
            result.Value.Histogram.Min.Should().Be(_timer.Value.Histogram.Min);
            result.Value.Histogram.MinUserValue.Should().Be(_timer.Value.Histogram.MinUserValue);
            result.Value.Histogram.StdDev.Should().Be(_timer.Value.Histogram.StdDev);
            result.Value.Histogram.Median.Should().Be(_timer.Value.Histogram.Median);
            result.Value.Histogram.Percentile75.Should().Be(_timer.Value.Histogram.Percentile75);
            result.Value.Histogram.Percentile95.Should().Be(_timer.Value.Histogram.Percentile95);
            result.Value.Histogram.Percentile98.Should().Be(_timer.Value.Histogram.Percentile98);
            result.Value.Histogram.Percentile99.Should().Be(_timer.Value.Histogram.Percentile99);
            result.Value.Histogram.Percentile999.Should().Be(_timer.Value.Histogram.Percentile999);
            result.Value.Histogram.SampleSize.Should().Be(_timer.Value.Histogram.SampleSize);
            result.Value.Rate.Count.Should().Be(_timer.Value.Rate.Count);
            result.Value.Rate.FifteenMinuteRate.Should().Be(_timer.Value.Rate.FifteenMinuteRate);
            result.Value.Rate.FiveMinuteRate.Should().Be(_timer.Value.Rate.FiveMinuteRate);
            result.Value.Rate.OneMinuteRate.Should().Be(_timer.Value.Rate.OneMinuteRate);
            result.Value.Rate.MeanRate.Should().Be(_timer.Value.Rate.MeanRate);
            result.Value.Rate.RateUnit.Should().Be(_timer.Value.Rate.RateUnit);
            result.Value.ActiveSessions.Should().Be(_timer.Value.ActiveSessions);
            result.Value.TotalTime.Should().Be(_timer.Value.TotalTime);
            result.Tags.Keys.Should().Contain(_timer.Tags.Keys.ToArray());
            result.Tags.Values.Should().Contain(_timer.Tags.Values.ToArray());
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = MetricType.Timer.SampleJson();

            var result = _serializer.Serialize(_timer).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_expected_json_with_group()
        {
            var expected = MetricTypeSamples.TimerWithGroup.SampleJson();

            var result = _serializer.Serialize(_timerWithGroup).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_Json()
        {
            var json = _serializer.Serialize(_timer);
            _output.WriteLine("Json Timer: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}