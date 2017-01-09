using System;
using System.Linq;
using App.Metrics.Data;
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
    public class ApdexSerializationTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly ApdexValueSource _apdex;
        private readonly ITestOutputHelper _output;
        private readonly MetricDataSerializer _serializer;

        public ApdexSerializationTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _serializer = new MetricDataSerializer();
            _apdex = fixture.ApdexScores.First();
        }

        [Fact]
        public void can_deserialize()
        {
            var jsonApdex = MetricType.Apdex.SampleJson();

            var result = _serializer.Deserialize<ApdexValueSource>(jsonApdex.ToString());

            result.Name.Should().BeEquivalentTo(_apdex.Name);
            result.Value.Score.Should().Be(_apdex.Value.Score);
            result.Value.SampleSize.Should().Be(_apdex.Value.SampleSize);
            result.Value.Satisfied.Should().Be(_apdex.Value.Satisfied);
            result.Value.Tolerating.Should().Be(_apdex.Value.Tolerating);
            result.Value.Frustrating.Should().Be(_apdex.Value.Frustrating);
            result.Tags.Should().ContainKeys(_apdex.Tags.Select(t => t.Key));
            result.Tags.Should().ContainValues(_apdex.Tags.Select(t => t.Value));
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = MetricType.Apdex.SampleJson();

            var result = _serializer.Serialize(_apdex).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_Json()
        {
            var json = _serializer.Serialize(_apdex);
            _output.WriteLine("Json Apdex Score: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}