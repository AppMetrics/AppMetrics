// <copyright file="MetricsDataSerializationTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Linq;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Formatters.Json.Serialization;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class MetricsDataSerializationTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly MetricsDataValueSource _metrics;
        private readonly ITestOutputHelper _output;
        private readonly MetricDataSerializer _serializer;

        public MetricsDataSerializationTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _serializer = new MetricDataSerializer();
            _metrics = fixture.DataWithOneContext;
        }

        [Fact]
        public void Can_deserialize()
        {
            var json = MetricDataSamples.SingleContext.SampleJson();

            var result = _serializer.Deserialize<MetricsDataValueSource>(json.ToString());

            result.Timestamp.Should().Be(_metrics.Timestamp);
            result.Contexts.Count().Should().Be(_metrics.Contexts.Count());
        }

        [Fact]
        public void Produces_expected_json()
        {
            var expected = MetricDataSamples.SingleContext.SampleJson();

            var result = _serializer.Serialize(_metrics).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void Produces_valid_Json()
        {
            var json = _serializer.Serialize(_metrics);
            _output.WriteLine("Json Metrics Data: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}