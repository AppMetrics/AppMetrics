// <copyright file="GaugeFormattingTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class GaugeFormattingTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly MetricsDataValueSource _metrics;
        private readonly ITestOutputHelper _output;
        private readonly IMetricsOutputFormatter _formatter;

        public GaugeFormattingTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _formatter = new MetricsJsonOutputFormatter();
            _metrics = fixture.GaugeContext;
        }

        [Fact(Skip = "https://github.com/AppMetrics/AppMetrics/issues/501")]
        public async Task Produces_expected_json()
        {
            // Arrange
            JToken result;
            var expected = MetricType.Gauge.SampleJson();

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _metrics);

                result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Produces_valid_Json()
        {
            // Arrange
            string result;

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _metrics);

                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            _output.WriteLine("Json Metrics Data: {0}", result);

            // Assert
            Action action = () => JToken.Parse(result);
            action.Should().NotThrow<Exception>();
        }
    }
}