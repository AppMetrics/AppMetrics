using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspNet.Metrics.Health;
using AspNet.Metrics.Infrastructure;
using Metrics.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspNet.Metrics.Internal
{
    public static class HealthChecksAsServices
    {
        public static void AddHealthChecksAsServices(IServiceCollection services, IEnumerable<Type> types)
        {
            var healthCheckTypeProvider = new StaticHealthCheckTypeProvider();
            foreach (var type in types)
            {
                services.TryAddTransient(typeof(HealthCheck), type);
                healthCheckTypeProvider.HealthCheckTypes.Add(type.GetTypeInfo());
            }

            services.Replace(ServiceDescriptor.Instance<IHealthCheckTypeProvider>(healthCheckTypeProvider));
        }

        public static void AddHealthChecksAsServices(IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var assemblyProvider = new StaticMetricsAssemblyProvider();
            foreach (var assembly in assemblies)
            {
                assemblyProvider.CandidateAssemblies.Add(assembly);
            }

            var controllerTypeProvider = new DefaultHealthCheckTypeProvider(assemblyProvider);
            var controllerTypes = controllerTypeProvider.HealthCheckTypes;

            AddHealthChecksAsServices(services, controllerTypes.Select(type => type.AsType()));
        }
    }
}