// <copyright file="EnvironmentInfoSerializationTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Infrastructure;
using App.Metrics.Middleware.Formatters.Json.Facts.Helpers;
using App.Metrics.Middleware.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Middleware.Formatters.Json.Serialization;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Middleware.Formatters.Json.Facts
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
        public void Can_deserialize()
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
        public void Produces_expected_json()
        {
            var expected = _env.SampleJson();

            var result = _serializer.Serialize(_env).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void Produces_valid_Json()
        {
            var json = _serializer.Serialize(_env);
            _output.WriteLine("Json Env Info: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}