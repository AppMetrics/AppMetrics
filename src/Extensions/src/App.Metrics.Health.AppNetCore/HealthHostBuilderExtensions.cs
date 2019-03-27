// <copyright file="HealthHostBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.Reflection;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Hosting
    // ReSharper restore CheckNamespace
{
    public static class HealthHostBuilderExtensions
    {
        private static bool _healthBuilt;

        public static IHostBuilder ConfigureHealthWithDefaults(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, IHealthBuilder> configureHealth,
            DependencyContext dependencyContext = null)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var healthBuilder = AppMetricsHealth.CreateDefaultBuilder();

                    configureHealth(context, healthBuilder);

                    healthBuilder.HealthChecks.RegisterFromAssembly(services, dependencyContext ?? GetDependencyContext());
                    healthBuilder.Configuration.ReadFrom(context.Configuration);

                    if (healthBuilder.CanReport())
                    {
                        services.AddHealthReportingHostedService();
                    }

                    services.AddHealth(healthBuilder);
                    _healthBuilt = true;
                });
        }

        public static IHostBuilder ConfigureMetrics(
            this IHostBuilder hostBuilder,
            IHealthRoot health)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    if (health.Options.ReportingEnabled && health.Reporters != null && health.Reporters.Any())
                    {
                        services.AddHealthReportingHostedService();
                    }

                    services.AddHealth(health);

                    _healthBuilt = true;
                });
        }

        public static IHostBuilder ConfigureHealthWithDefaults(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, IServiceCollection, IHealthBuilder> configureHealth,
            DependencyContext dependencyContext = null)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    var healthBuilder = AppMetricsHealth.CreateDefaultBuilder();

                    configureHealth(context, services, healthBuilder);

                    healthBuilder.HealthChecks.RegisterFromAssembly(services, dependencyContext ?? GetDependencyContext());
                    healthBuilder.Configuration.ReadFrom(context.Configuration);

                    if (healthBuilder.CanReport())
                    {
                        services.AddHealthReportingHostedService();
                    }

                    services.AddHealth(healthBuilder);

                    _healthBuilt = true;
                });
        }

        public static IHostBuilder ConfigureHealthWithDefaults(
            this IHostBuilder hostBuilder,
            Action<IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            hostBuilder.ConfigureHealthWithDefaults(
                (context, builder) => { configureHealth(builder); });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureHealth(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealth(
                        healthBuilder =>
                        {
                            configureHealth(context, healthBuilder);
                            healthBuilder.Configuration.ReadFrom(context.Configuration);

                            if (healthBuilder.CanReport())
                            {
                                services.AddHealthReportingHostedService();
                            }

                            _healthBuilt = true;
                        });
                });
        }

        public static IHostBuilder ConfigureHealth(
            this IHostBuilder hostBuilder,
            Action<HostBuilderContext, IServiceCollection, IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    services.AddHealth(
                        healthBuilder =>
                        {
                            configureHealth(context, services, healthBuilder);
                            healthBuilder.Configuration.ReadFrom(context.Configuration);

                            if (healthBuilder.CanReport())
                            {
                                services.AddHealthReportingHostedService();
                            }

                            _healthBuilt = true;
                        });
                });
        }

        public static IHostBuilder ConfigureHealth(this IHostBuilder hostBuilder, Action<IHealthBuilder> configureHealth)
        {
            if (_healthBuilt)
            {
                throw new InvalidOperationException("HealthBuilder allows creation only of a single instance of IHealth");
            }

            hostBuilder.ConfigureHealth(
                (context, healthBuilder) => { configureHealth(healthBuilder); });

            return hostBuilder;
        }

        public static IHostBuilder ConfigureHealth(
            this IHostBuilder hostBuilder,
            DependencyContext dependencyContext = null)
        {
            if (_healthBuilt)
            {
                return hostBuilder;
            }

            return hostBuilder.ConfigureServices(
                (context, services) =>
                {
                    if (!_healthBuilt)
                    {
                        var builder = AppMetricsHealth.CreateDefaultBuilder().Configuration.ReadFrom(context.Configuration).HealthChecks.
                                                       RegisterFromAssembly(services, dependencyContext ?? GetDependencyContext());

                        services.AddHealth(builder);
                        _healthBuilt = true;
                    }
                });
        }

        internal static DependencyContext GetDependencyContext() { return Assembly.GetEntryAssembly() != null ? DependencyContext.Default : null; }
    }
}