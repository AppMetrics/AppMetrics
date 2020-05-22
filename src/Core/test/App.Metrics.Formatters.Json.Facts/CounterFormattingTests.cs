// <copyright file="CounterFormattingTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Counter;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class CounterFormattingTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly MetricsDataValueSource _metrics;
        private readonly ITestOutputHelper _output;
        private readonly IMetricsOutputFormatter _formatter;

        public CounterFormattingTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _formatter = new MetricsJsonOutputFormatter();
            _metrics = fixture.CounterContext;
        }

        [Fact(Skip = "https://github.com/AppMetrics/AppMetrics/issues/501")]
        public async Task Produces_expected_json()
        {
            // Arrange
            JToken result;
            var expected = MetricType.Counter.SampleJson();

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

        [Fact(Skip = "https://github.com/AppMetrics/AppMetrics/issues/501")]
        public async Task Counter_is_reset()
        {
            MetricProviderTestFixture metricProviderTestFixture = new MetricProviderTestFixture();

            JToken result;

            var expected = MetricType.Counter.SampleJson();

            using (var stream = new MemoryStream())
            {
              await _formatter.WriteAsync(stream, metricProviderTestFixture.ResetCounterContext);

              result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            result.Should().BeEquivalentTo(expected);

            expected = TestHelperExtensions.SampleJson("counter_reset");

            using (var stream = new MemoryStream())
            {
              await _formatter.WriteAsync(stream, metricProviderTestFixture.ResetCounterContext);

              result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            result.Should().BeEquivalentTo(expected);
        }

      [Fact]
      public void Counter_report_set_iterms()
      {
        var tags = new MetricTags("x", "y");

        var counter = new DefaultCounterMetric();
        counter.Increment(new MetricSetItem("key", "value"));
        counter.Increment(new MetricSetItem("key1", "value"));

        // Test reportSetItems = true & reportItemPercentages = true
        CounterValueSource counterValueSource = new CounterValueSource("test", counter, Unit.Items, tags);
        var serilized = counterValueSource.ToSerializableMetric();
        serilized.Count.Should().Be(2);
        serilized.Items.Should().NotBeEmpty();
        serilized.Items.First().Item.Should().Be("key:value");
        serilized.Items.First().Percent.Should().Be(50);

        // Test reportSetItems = true & reportItemPercentages = false
        counterValueSource = new CounterValueSource("test", counter, Unit.Items, tags, false, false);
        serilized = counterValueSource.ToSerializableMetric();
        serilized.Count.Should().Be(2);
        serilized.Items.Should().NotBeEmpty();
        serilized.Items.First().Item.Should().Be("key:value");
        serilized.Items.First().Percent.Should().Be(default);

        // Test reportSetItems = false
        counterValueSource = new CounterValueSource("test", counter, Unit.Items, tags, false, true, false);
        serilized = counterValueSource.ToSerializableMetric();
        serilized.Count.Should().Be(2);
        serilized.Items.Should().BeEmpty();
      }
    }
}