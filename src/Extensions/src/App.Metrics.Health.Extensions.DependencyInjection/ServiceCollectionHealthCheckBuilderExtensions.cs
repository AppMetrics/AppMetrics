// <copyright file="ServiceCollectionHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.Health;
using App.Metrics.Health.Extensions.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionHealthCheckBuilderExtensions
    {
        /// <summary>
        ///     Scan the executing assembly and it's refererenced assemblies for health checks and automatically register them.
        /// </summary>
        /// <param name="healthCheckBuilder">The <see cref="IHealthBuilder" /> to add any found health checks.</param>
        /// <param name="services">The <see cref="IServiceCollection" /> where found health checks should be registered.</param>
        /// <param name="dependencyContext">
        ///     The dependency context from which health checks can be located. If not supplied, the platform
        ///     default will be used.
        /// </param>
        /// <returns>
        ///     An <see cref="IHealthBuilder" /> that can be used to further configure App Metrics Health.
        /// </returns>
        public static IHealthBuilder RegisterFromAssembly(
            this IHealthCheckBuilder healthCheckBuilder,
            IServiceCollection services,
            DependencyContext dependencyContext = null)
        {
            HealthChecksAsServices.AddHealthChecksAsServices(services, HealthAssemblyDiscoveryProvider.DiscoverAssemblies(dependencyContext));

            return healthCheckBuilder.Builder;
        }
    }
}