// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Meter;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class MeterSerializationTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly MeterValueSource _meter;
        private readonly ITestOutputHelper _output;
        private readonly MetricDataSerializer _serializer;

        public MeterSerializationTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _serializer = new MetricDataSerializer();
            _meter = fixture.Meters.First(x => x.Name == fixture.MeterNameDefault);
        }

        [Fact]
        public void can_deserialize()
        {
            var jsonMeter = MetricType.Meter.SampleJson();

            var result = _serializer.Deserialize<MeterValueSource>(jsonMeter.ToString());

            result.Name.Should().BeEquivalentTo(_meter.Name);
            result.Unit.Should().Be(_meter.Unit);
            result.Value.Count.Should().Be(_meter.Value.Count);
            result.Value.FifteenMinuteRate.Should().Be(_meter.Value.FifteenMinuteRate);
            result.Value.FiveMinuteRate.Should().Be(_meter.Value.FiveMinuteRate);
            result.Value.OneMinuteRate.Should().Be(_meter.Value.OneMinuteRate);
            result.Value.MeanRate.Should().Be(_meter.Value.MeanRate);
            result.Value.RateUnit.Should().Be(_meter.Value.RateUnit);
            result.Tags.Keys.Should().Contain(_meter.Tags.Keys.ToArray());
            result.Tags.Values.Should().Contain(_meter.Tags.Values.ToArray());
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = MetricType.Meter.SampleJson();

            var result = _serializer.Serialize(_meter).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_Json()
        {
            var json = _serializer.Serialize(_meter);
            _output.WriteLine("Json Meter: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}