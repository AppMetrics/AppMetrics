// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Serialization;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class HealthStatusSerializationTests
    {
        private readonly HealthStatus _healthStatus;
        private readonly ITestOutputHelper _output;
        private readonly HealthStatusSerializer _serializer;

        public HealthStatusSerializationTests(ITestOutputHelper output)
        {
            _output = output;
            _serializer = new HealthStatusSerializer(new TestClock());

            var healthyOne = new HealthCheck.Result("test_one_healthy", HealthCheckResult.Healthy("first check was good"));
            var healthyTwo = new HealthCheck.Result("test_two_healthy", HealthCheckResult.Healthy("second check was good"));
            var unhealthyOne = new HealthCheck.Result("test_three_unhealthy", HealthCheckResult.Unhealthy("something failed"));
            var unhealthyTwo = new HealthCheck.Result("test_four_unhealthy", HealthCheckResult.Unhealthy("something else failed"));
            var degradedOne = new HealthCheck.Result("test_five_degraded", HealthCheckResult.Degraded("degrading service"));

            var checks = new[] { healthyOne, healthyTwo, unhealthyOne, unhealthyTwo, degradedOne };

            _healthStatus = new HealthStatus(checks);
        }

        [Fact]
        public void can_deserialize()
        {
            var expected = HealthStatusSamples.Valid.SampleJson();

            var result = _serializer.Deserialize<HealthStatus>(expected.ToString());

            result.Status.Should().Be(HealthCheckStatus.Unhealthy);
            result.Results.Length.Should().Be(5);
            result.Results.Count(r => r.Check.Status.IsHealthy()).Should().Be(2);
            result.Results.Count(r => r.Check.Status == HealthCheckStatus.Unhealthy).Should().Be(2);
            result.Results.Count(r => r.Check.Status == HealthCheckStatus.Degraded).Should().Be(1);
        }

        [Fact]
        public void produces_expected_json()
        {
            var expected = HealthStatusSamples.Valid.SampleJson();

            var result = _serializer.Serialize(_healthStatus).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_expected_json_when_null_healthy_checks()
        {
            var expected = HealthStatusSamples.NullUnhealthy.SampleJson();
            var unhealthyOne = new HealthCheck.Result("test_three_unhealthy", HealthCheckResult.Unhealthy("something failed"));
            var unhealthyTwo = new HealthCheck.Result("test_four_unhealthy", HealthCheckResult.Unhealthy("something else failed"));

            var checks = new[] { unhealthyOne, unhealthyTwo };

            var healthStatus = new HealthStatus(checks);

            var result = _serializer.Serialize(healthStatus).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_expected_json_when_null_unhealthy_checks()
        {
            var expected = HealthStatusSamples.NullHealthy.SampleJson();
            var healthyOne = new HealthCheck.Result("test_one_healthy", HealthCheckResult.Healthy("first check was good"));
            var healthyTwo = new HealthCheck.Result("test_two_healthy", HealthCheckResult.Healthy("second check was good"));

            var checks = new[] { healthyOne, healthyTwo };

            var healthStatus = new HealthStatus(checks);

            var result = _serializer.Serialize(healthStatus).ParseAsJson();

            result.Should().Be(expected);
        }

        [Fact]
        public void produces_valid_Json()
        {
            var json = _serializer.Serialize(_healthStatus);
            _output.WriteLine("HealthStatus: {0}", json);

            Action action = () => JToken.Parse(json);
            action.ShouldNotThrow<Exception>();
        }
    }
}