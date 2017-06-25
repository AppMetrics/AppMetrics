// <copyright file="HealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics;
using App.Metrics.Benchmarks.Fixtures;
using App.Metrics.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Benchmarks.Facts
{
    public class HealthCheck : IClassFixture<MetricsCoreWithHealthTestFixture>
    {
        private readonly MetricsCoreWithHealthTestFixture _fixture;

        public HealthCheck(MetricsCoreWithHealthTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void ReadHealthStatus()
        {
            SimpleBenchmarkRunner.Run(
                async () =>
                {
                    var result = await _fixture.Metrics.Health.ReadStatusAsync();
                    Debug.WriteLine("Health status: " + result.Status);
                });
        }
    }
}
