using System;
using System.Linq;
using App.Metrics.Data;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
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
            _metrics = fixture.DataWithOneGroup;
        }

        [Fact]
        public void can_deserialize()
        {
            var json = MetricDataSamples.SingleGroup.SampleJson();

            var result = _serializer.Deserialize<MetricsDataValueSource>(json.ToString());

            result.ContextName.Should().BeEquivalentTo(_metrics.ContextName);
            result.Timestamp.Should().Be(_metrics.Timestamp);
            result.Groups.Count().Should().Be(_metrics.Groups.Count());
            result.Environment.Entries.Count().Should().Be(_metrics.Environment.Entries.Count());
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = MetricDataSamples.SingleGroup.SampleJson();

            var result = _serializer.Serialize(_metrics).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_Json()
        {
            var json = _serializer.Serialize(_metrics);
            _output.WriteLine("Json Metrics Data: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}