// <copyright file="HealthChecksAsServices.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Metrics.Health.Extensions.DependencyInjection.Internal
{
    internal static class HealthChecksAsServices
    {
        // ReSharper disable MemberCanBePrivate.Global
        public static void AddHealthChecksAsServices(IServiceCollection services, IEnumerable<Type> types)
            // ReSharper restore MemberCanBePrivate.Global
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

            var assemblyProvider = new StaticHealthAssemblyProvider();
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