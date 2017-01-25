// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Gauge;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class GaugeSerializationTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly GaugeValueSource _gauge;
        private readonly ITestOutputHelper _output;
        private readonly MetricDataSerializer _serializer;

        public GaugeSerializationTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _serializer = new MetricDataSerializer();
            _gauge = fixture.Gauges.First();
        }

        [Fact]
        public void can_deserialize()
        {
            var result = _serializer.Deserialize<GaugeValueSource>(MetricType.Gauge.SampleJson().ToString());

            result.Name.Should().BeEquivalentTo(_gauge.Name);
            result.Unit.Should().Be(_gauge.Unit);
            result.Value.Should().Be(_gauge.Value);
            result.Tags.Should().ContainKeys(_gauge.Tags.Select(t => t.Key));
            result.Tags.Should().ContainValues(_gauge.Tags.Select(t => t.Value));
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = MetricType.Gauge.SampleJson();

            var result = _serializer.Serialize(_gauge).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_json()
        {
            var json = _serializer.Serialize(_gauge);
            _output.WriteLine("Json Gauge: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}