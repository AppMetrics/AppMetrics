// <copyright file="ServiceCollectionHealthBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Health;
using App.Metrics.Health.Formatters;
using App.Metrics.Health.Internal;
using App.Metrics.Health.Internal.NoOp;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionHealthBuilderExtensions
    {
        public static IHealthRoot BuildAndAddTo(
            this IHealthBuilder builder,
            IServiceCollection services)
        {
            var health = builder.Build();

            services.TryAddSingleton<IHealth>(provider =>
            {
                var checks = ResolveAllHealthChecks(provider, health);
                return new DefaultHealth(checks);
            });
            services.TryAddSingleton<IHealthRoot>(
                provider =>
                {
                    var resolvedHealth = provider.GetRequiredService<IHealth>();
                    var resolvedHealthChecksRunner = provider.GetRequiredService<IRunHealthChecks>();

                    return new HealthRoot(
                        resolvedHealth,
                        health.Options,
                        health.OutputHealthFormatters as HealthFormatterCollection,
                        health.DefaultOutputHealthFormatter,
                        resolvedHealthChecksRunner,
                        health.Reporters as HealthReporterCollection);
                });
            services.TryAddSingleton<IRunHealthChecks>(provider =>
            {
                var checks = ResolveAllHealthChecks(provider, health).ToList();

                if (!health.Options.Enabled || !checks.Any())
                {
                    return new NoOpHealthCheckRunner();
                }

                return new DefaultHealthCheckRunner(checks);
            });

            AppMetricsHealthServiceCollectionExtensions.AddCoreServices(services, health);

            return health;
        }

        private static IEnumerable<HealthCheck> ResolveAllHealthChecks(IServiceProvider provider, IHealth health)
        {
            var resolvedChecks = provider.GetRequiredService<IEnumerable<HealthCheck>>();
            var result = health.Checks.ToList();

            var existingNames = result.Select(c => c.Name).ToList();

            foreach (var check in resolvedChecks)
            {
                if (existingNames.Contains(check.Name))
                {
                    throw new InvalidOperationException($"Health check names should be unique, found more than one health check named `{check.Name}`");
                }

                result.Add(check);
            }

            return result;
        }
    }
}
