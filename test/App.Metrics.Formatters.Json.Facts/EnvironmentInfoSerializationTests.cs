using System;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Infrastructure;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class EnvironmentInfoSerializationTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly EnvironmentInfo _env;
        private readonly ITestOutputHelper _output;
        private readonly EnvironmentInfoSerializer _serializer;

        public EnvironmentInfoSerializationTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _serializer = new EnvironmentInfoSerializer();
            _env = fixture.Env;
        }

        [Fact]
        public void can_deserialize()
        {
            var json = _env.SampleJson();

            var result = _serializer.Deserialize<EnvironmentInfo>(json.ToString());

            result.EntryAssemblyName.Should().BeEquivalentTo(_env.EntryAssemblyName);
            result.EntryAssemblyVersion.Should().BeEquivalentTo(_env.EntryAssemblyVersion);
            result.HostName.Should().BeEquivalentTo(_env.HostName);
            result.LocalTimeString.Should().BeEquivalentTo(_env.LocalTimeString);
            result.MachineName.Should().BeEquivalentTo(_env.MachineName);
            result.OperatingSystem.Should().BeEquivalentTo(_env.OperatingSystem);
            result.OperatingSystemVersion.Should().BeEquivalentTo(_env.OperatingSystemVersion);
            result.ProcessName.Should().BeEquivalentTo(_env.ProcessName);
            result.ProcessorCount.Should().Be(_env.ProcessorCount);
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = _env.SampleJson();

            var result = _serializer.Serialize(_env).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_Json()
        {
            var json = _serializer.Serialize(_env);
            _output.WriteLine("Json Env Info: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}