// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Metrics.Core;
using App.Metrics.Health;
using App.Metrics.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Metrics.DependencyInjection
{
    internal static class HealthChecksAsServices
    {
        /// <summary>
        ///     Adds the health checks as services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="types">The types.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static void AddHealthChecksAsServices(IServiceCollection services, IEnumerable<Type> types)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var healthCheckTypeProvider = new StaticHealthCheckTypeProvider();

            foreach (var type in types)
            {
                services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(HealthCheck), type));
                healthCheckTypeProvider.HealthCheckTypes.Add(type.GetTypeInfo());
            }

            services.Replace(ServiceDescriptor.Singleton<IHealthCheckTypeProvider>(healthCheckTypeProvider));
        }

        public static void AddHealthChecksAsServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var assemblyProvider = new StaticMetricsAssemblyProvider();
            foreach (var assembly in assemblies)
            {
                assemblyProvider.CandidateAssemblies.Add(assembly);
            }

            var healthCheckTypeProvider = new DefaultHealthCheckTypeProvider(assemblyProvider);
            var healthCheckTypes = healthCheckTypeProvider.HealthCheckTypes;

            AddHealthChecksAsServices(services, healthCheckTypes.Select(type => type.AsType()));
        }
    }
}