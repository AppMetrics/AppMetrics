// <copyright file="JsonOutputFormatterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Health.Formatters.Json.Facts.Helpers;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Health.Formatters.Json.Facts
{
    public class JsonOutputFormatterTests
    {
        private readonly HealthStatus _healthStatus;
        private readonly ITestOutputHelper _output;
        private readonly IHealthOutputFormatter _formatter;

        public JsonOutputFormatterTests(ITestOutputHelper output)
        {
            _output = output;
            _formatter = new HealthStatusJsonOutputFormatter(DefaultJsonSerializerSettings.CreateSerializerSettings());

            var healthyOne = new HealthCheck.Result("test_one_healthy", HealthCheckResult.Healthy("first check was good"));
            var healthyTwo = new HealthCheck.Result("test_two_healthy", HealthCheckResult.Healthy("second check was good"));
            var unhealthyOne = new HealthCheck.Result("test_three_unhealthy", HealthCheckResult.Unhealthy("something failed"));
            var unhealthyTwo = new HealthCheck.Result("test_four_unhealthy", HealthCheckResult.Unhealthy("something else failed"));
            var degradedOne = new HealthCheck.Result("test_five_degraded", HealthCheckResult.Degraded("degrading service"));

            var checks = new[] { healthyOne, healthyTwo, unhealthyOne, unhealthyTwo, degradedOne };

            _healthStatus = new HealthStatus(checks);
        }

        // DEVNOE: No need for deserialization for now
        // [Fact]
        // public async Task Can_deserialize()
        // {
        //     string result;
        //     var expected = HealthStatusSamples.Valid.SampleJson();
        //     using (var stream = new MemoryStream())
        //     {
        //         await _formatter.WriteAsync(stream, _healthStatus, Encoding.UTF8);
        //         result = Encoding.UTF8.GetString(stream.ToArray());
        //     }
        //     result.Status.Should().Be(HealthCheckStatus.Unhealthy);
        //     result.Results.Length.Should().Be(5);
        //     result.Results.Count(r => r.Check.Status.IsHealthy()).Should().Be(2);
        //     result.Results.Count(r => r.Check.Status == HealthCheckStatus.Unhealthy).Should().Be(2);
        //     result.Results.Count(r => r.Check.Status == HealthCheckStatus.Degraded).Should().Be(1);
        // }

        [Fact]
        public async Task Produces_expected_json()
        {
            // Arrange
            JToken result;
            var expected = HealthStatusSamples.Valid.SampleJson();

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _healthStatus);

                result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Produces_expected_json_when_null_healthy_checks()
        {
            // Arrange
            JToken result;
            var expected = HealthStatusSamples.NullUnhealthy.SampleJson();
            var unhealthyOne = new HealthCheck.Result("test_three_unhealthy", HealthCheckResult.Unhealthy("something failed"));
            var unhealthyTwo = new HealthCheck.Result("test_four_unhealthy", HealthCheckResult.Unhealthy("something else failed"));
            var checks = new[] { unhealthyOne, unhealthyTwo };
            var healthStatus = new HealthStatus(checks);

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, healthStatus);

                result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Produces_expected_json_when_null_unhealthy_checks()
        {
            // Arrange
            JToken result;
            var expected = HealthStatusSamples.NullHealthy.SampleJson();
            var healthyOne = new HealthCheck.Result("test_one_healthy", HealthCheckResult.Healthy("first check was good"));
            var healthyTwo = new HealthCheck.Result("test_two_healthy", HealthCheckResult.Healthy("second check was good"));
            var checks = new[] { healthyOne, healthyTwo };
            var healthStatus = new HealthStatus(checks);

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, healthStatus);

                result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Produces_valid_Json()
        {
            // Arrange
            string json;

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _healthStatus);

                json = Encoding.UTF8.GetString(stream.ToArray());
            }

            _output.WriteLine("HealthStatus: {0}", json);

            Action action = () => JToken.Parse(json);

            // Assert
            action.Should().NotThrow<Exception>();
        }
    }
}