// <copyright file="ServiceCollectionHealthBuilderExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using FluentAssertions;
using Health.Extensions.DependencyInjection.Facts.Fixtures;
using Health.Extensions.DependencyInjection.Facts.TestHelpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Health.Extensions.DependencyInjection.Facts
{
    public class ServiceCollectionHealthBuilderExtensionsTests : IClassFixture<HealthFixture>
    {
        private readonly HealthFixture _fixture;

        public ServiceCollectionHealthBuilderExtensionsTests(HealthFixture fixture) { _fixture = fixture; }

        [Fact]
        public void Can_add_all_checks_from_assembly()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IDatabase, Database>();

            // Act
            var unused = new HealthBuilder()
                .HealthChecks.RegisterFromAssembly(services, _fixture.DependencyContext)
                .BuildAndAddTo(services);
            var provider = services.BuildServiceProvider();
            var health = provider.GetRequiredService<IHealth>();

            // Assert
            health.Checks.Count().Should().Be(2);
            health.Checks.First().Name.Should().Be("DatabaseCheck");
            health.Checks.Skip(1).First().Name.Should().Be("Referenced Health Check");
        }

        [Fact]
        public void Can_build_and_add_health_to_service_collection()
        {
            // Arrange
            const string checkName = "inline healthy";
            var services = new ServiceCollection();
            var health = new HealthBuilder().
                HealthChecks.AddCheck(checkName, () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                .Build();

            // Act
            services.AddHealth(health);
            var provider = services.BuildServiceProvider();
            var resolvedHealth = provider.GetRequiredService<IHealth>();

            // Assert
            resolvedHealth.Checks.Count().Should().Be(1);
            resolvedHealth.Checks.Single().Name.Should().Be(checkName);
        }

        [Fact]
        public void Should_scan_assembly_and_add_health_checks_and_ignore_obsolete_checks()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IDatabase, Database>();

            // Act
            var unused = new HealthBuilder()
                .HealthChecks.RegisterFromAssembly(services, _fixture.DependencyContext)
                .BuildAndAddTo(services);
            var provider = services.BuildServiceProvider();
            var health = provider.GetRequiredService<IHealth>();

            // Assert
            health.Checks.Count().Should().Be(2);
            health.Checks.First().Name.Should().Be("DatabaseCheck");
            health.Checks.Skip(1).First().Name.Should().Be("Referenced Health Check");
        }

        [Fact]
        public void Should_only_register_class_deriving_health_check()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IDatabase, Database>();

            // Act
            var unused = new HealthBuilder()
                         .HealthChecks.RegisterFromAssembly(services, _fixture.DependencyContext)
                         .BuildAndAddTo(services);
            var provider = services.BuildServiceProvider();
            var health = provider.GetRequiredService<IHealth>();

            // Assert
            health.Checks.FirstOrDefault(h => h.Name == nameof(DontRegisterHealthCheck)).Should().BeNull();
        }

        [Fact]
        public void Should_append_resolved_health_checks_to_those_explicitly_registered()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IDatabase, Database>();

            // Act
            var unused = new HealthBuilder()
                .HealthChecks.AddCheck("inline", () => new ValueTask<HealthCheckResult>(HealthCheckResult.Healthy()))
                .HealthChecks.RegisterFromAssembly(services, _fixture.DependencyContext)
                .BuildAndAddTo(services);
            var provider = services.BuildServiceProvider();
            var health = provider.GetRequiredService<IHealth>();

            // Assert
            health.Checks.Count().Should().Be(3);
            health.Checks.FirstOrDefault(c => c.Name == "DatabaseCheck").Should().NotBeNull();
            health.Checks.FirstOrDefault(c => c.Name == "Referenced Health Check").Should().NotBeNull();
            health.Checks.FirstOrDefault(c => c.Name == "inline").Should().NotBeNull();
        }
    }
}