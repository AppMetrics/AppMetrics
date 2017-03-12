// <copyright file="DefaultMetricsAssemblyDiscoveryProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace App.Metrics.DependencyInjection.Internal
{
    internal static class DefaultMetricsAssemblyDiscoveryProvider
    {
        // ReSharper disable MemberCanBePrivate.Global
        internal static HashSet<string> ReferenceAssemblies { get; } = new HashSet<string>(StringComparer.Ordinal)
                                                                       {
                                                                           "App.Metrics"
                                                                       };

        internal static IEnumerable<Assembly> DiscoverAssemblies(string entryPointAssemblyName)
        {
            var entryAssembly = Assembly.Load(new AssemblyName(entryPointAssemblyName));
            var context = DependencyContext.Load(Assembly.Load(new AssemblyName(entryPointAssemblyName)));

            return GetCandidateAssemblies(entryAssembly, context);
        }

        internal static IEnumerable<Assembly> GetCandidateAssemblies(Assembly entryAssembly, DependencyContext dependencyContext)
        {
            if (dependencyContext == null)
            {
                // Use the entry assembly as the sole candidate.
                return new[] { entryAssembly };
            }

            return GetCandidateLibraries(dependencyContext)
                .SelectMany(library => library.GetDefaultAssemblyNames(dependencyContext))
                .Select(Assembly.Load);
        }

        internal static IEnumerable<RuntimeLibrary> GetCandidateLibraries(DependencyContext dependencyContext)
        {
            if (ReferenceAssemblies == null)
            {
                return Enumerable.Empty<RuntimeLibrary>();
            }

            return dependencyContext.RuntimeLibraries.Where(IsCandidateLibrary);
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library)
        {
            Debug.Assert(ReferenceAssemblies != null, "reference assemblies not null");

            return !ReferenceAssemblies.Contains(library.Name) &&
                   library.Dependencies.Any(dependency => ReferenceAssemblies.Contains(dependency.Name));
        }

        // ReSharper restore MemberCanBePrivate.Global
    }
}