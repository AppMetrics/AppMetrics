// <copyright file="HealthCheck.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Diagnostics;
using App.Metrics.Health.Benchmarks.Fixtures;
using App.Metrics.Health.Benchmarks.Support;
using Xunit;

namespace App.Metrics.Health.Benchmarks.Facts
{
    public class HealthCheck : IClassFixture<HealthTestFixture>
    {
        private readonly HealthTestFixture _fixture;

        public HealthCheck(HealthTestFixture fixture) { _fixture = fixture; }

        [Fact]
        public void ReadHealthStatus()
        {
            SimpleBenchmarkRunner.Run(
                async () =>
                {
                    var result = await _fixture.Health.ReadStatusAsync();
                    Debug.WriteLine("Health status: " + result.Status);
                });
        }
    }
}
