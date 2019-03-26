// <copyright file="HealthAssemblyDiscoveryProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace App.Metrics.Health.Extensions.DependencyInjection.Internal
{
    internal static class HealthAssemblyDiscoveryProvider
    {
        private static readonly string ReferenceAssembliesPrefix = "app.metrics";

        // ReSharper disable MemberCanBePrivate.Global
        internal static IEnumerable<Assembly> DiscoverAssemblies(DependencyContext dependencyContext = null)
        {
            return GetCandidateAssemblies(dependencyContext ?? GetDependencyContext());
        }

        internal static IEnumerable<Assembly> GetCandidateAssemblies(DependencyContext dependencyContext)
        {
            var query = Enumerable.Empty<Assembly>();

            if (dependencyContext != null)
            {
                query = GetCandidateLibraries(dependencyContext).
                    SelectMany(library => library.GetDefaultAssemblyNames(dependencyContext)).
                    Select(Assembly.Load);
            }
            else
            {
                var dlls = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                var exes = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe");
                var files = dlls?.Concat(exes);
                var nonSystemAssemblies = from outputAssemblyPath in files
                                          let assemblyFileName = System.IO.Path.GetFileNameWithoutExtension(outputAssemblyPath)
                                          where assemblyFileName != null && !assemblyFileName.ToLowerInvariant().StartsWith("system.") && !assemblyFileName.ToLowerInvariant().StartsWith("microsoft.")
                                          select Assembly.Load(AssemblyName.GetAssemblyName(outputAssemblyPath));

                var assemblies = nonSystemAssemblies.ToArray();

                query = from assembly in assemblies
                        let referencedAssemblies = assembly.GetReferencedAssemblies()
                        where referencedAssemblies.Any(
                            referencedAssembly => referencedAssembly.Name.ToLowerInvariant().StartsWith(ReferenceAssembliesPrefix))
                        select assembly;
            }

            return query.ToArray();
        }

        internal static IEnumerable<RuntimeLibrary> GetCandidateLibraries(DependencyContext dependencyContext)
        {
            return dependencyContext.RuntimeLibraries.Where(IsCandidateLibrary);
        }

        internal static DependencyContext GetDependencyContext()
        {
            return Assembly.GetEntryAssembly() != null ? DependencyContext.Default : null;
        }

        private static bool IsCandidateLibrary(RuntimeLibrary library)
        {
            return library.Name.StartsWith(ReferenceAssembliesPrefix) || library.Dependencies.Any(d => d.Name.ToLowerInvariant().StartsWith(ReferenceAssembliesPrefix));
        }

        // ReSharper restore MemberCanBePrivate.Global
    }
}