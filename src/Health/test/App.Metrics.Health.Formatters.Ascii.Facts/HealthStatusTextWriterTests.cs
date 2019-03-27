// <copyright file="HealthStatusTextWriterTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Threading.Tasks;
using App.Metrics.Health.Builder;
using App.Metrics.Health.Serialization;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Health.Formatters.Ascii.Facts
{
    public class HealthStatusTextWriterTests
    {
        [Fact]
        public async Task Can_apply_ascii_health_formatting()
        {
            // Arrange
            var health = new HealthBuilder()
                .OutputHealth.AsPlainText()
                .HealthChecks.AddCheck("test", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                .Build();

            var serializer = new HealthStatusSerializer();

            // Act
            var healthStatus = await health.HealthCheckRunner.ReadAsync();

            using (var sw = new StringWriter())
            {
                using (var writer = new HealthStatusTextWriter(sw))
                {
                    serializer.Serialize(writer, healthStatus);
                }

                // Assert
                sw.ToString().Should().Be(
                    "# OVERALL STATUS: Healthy\n--------------------------------------------------------------\n# CHECK: test\n\n           MESSAGE = OK\n            STATUS = Healthy\n--------------------------------------------------------------\n");
            }
        }
    }
}