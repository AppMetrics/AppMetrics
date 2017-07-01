// <copyright file="HealthCheckRegistryTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using App.Metrics.Builder;
using App.Metrics.Health.Facts.TestHelpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace App.Metrics.Health.Facts.DependencyInjection
{
    public class HealthCheckRegistryTests
    {
        [Fact]
        public async Task Can_register_inline_health_checks()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();

            services
                .AddHealthChecks()
                .AddChecks(
                    factory =>
                    {
                        factory.Register("DatabaseConnected", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy("Database Connection OK")));
                    });
            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadStatusAsync();

            result.HasRegisteredChecks.Should().BeTrue();
            result.Results.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_report_healthy_when_all_checks_pass()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();
            services.AddHealthChecks();

            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadStatusAsync();

            result.Status.Should().Be(HealthCheckStatus.Healthy);
        }

        [Fact]
        public async Task Should_report_unhealthy_when_all_checks_pass()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();
            services
                .AddHealthChecks()
                .AddChecks(
                    registry =>
                    {
                        registry.Register(
                            "DatabaseConnected",
                            () => new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy("Failed")));
                    });
            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadStatusAsync();

            result.Status.Should().Be(HealthCheckStatus.Unhealthy);
        }

        [Fact]
        public async Task Should_scan_assembly_and_register_health_checks_and_ignore_obsolete_checks()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IDatabase, Database>();
            services.AddHealthChecks();

            var provider = services.BuildServiceProvider();
            var healthProvider = provider.GetRequiredService<IProvideHealth>();

            var result = await healthProvider.ReadStatusAsync();

            result.HasRegisteredChecks.Should().BeTrue();
            result.Results.Should().HaveCount(1);
        }
    }
}