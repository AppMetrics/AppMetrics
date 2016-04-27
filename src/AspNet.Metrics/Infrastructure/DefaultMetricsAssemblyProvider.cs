using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace AspNet.Metrics.Infrastructure
{
    public class DefaultMetricsAssemblyProvider : IAssemblyProvider
    {
        private readonly ILibraryManager _libraryManager;

        public DefaultMetricsAssemblyProvider(ILibraryManager libraryManager)
        {
            _libraryManager = libraryManager;
        }

        /// <inheritdoc />
        public IEnumerable<Assembly> CandidateAssemblies
        {
            get
            {
                return GetCandidateLibraries().SelectMany(l => l.Assemblies)
                    .Select(Load);
            }
        }

        /// <summary>
        ///     Gets the set of assembly names that are used as root for discovery of
        ///     MVC controllers, view components and views.
        /// </summary>
        // DefaultControllerTypeProvider uses CandidateAssemblies to determine if the base type of a POCO controller
        // lives in an assembly that references Metrics.NET. CandidateAssemblies excludes all assemblies from the
        // ReferenceAssemblies set. Consequently adding WebApiCompatShim to this set would cause the ApiController to
        // fail this test.
        protected virtual HashSet<string> ReferenceAssemblies { get; } = new HashSet<string>(StringComparer.Ordinal)
        {
            "Metrics.Net",
            "AspNet.Metrics"
        };

        /// <summary>
        ///     Returns a list of libraries that references the assemblies in <see cref="ReferenceAssemblies" />.
        ///     By default it returns all assemblies that reference any of the primary MVC assemblies
        ///     while ignoring MVC assemblies.
        /// </summary>
        /// <returns>A set of <see cref="Microsoft.Extensions.DependencyModel.Library" />.</returns>
        protected virtual IEnumerable<Library> GetCandidateLibraries()
        {
            if (ReferenceAssemblies == null)
            {
                return Enumerable.Empty<Library>();
            }

            // GetReferencingLibraries returns the transitive closure of referencing assemblies
            // for a given assembly.
            return ReferenceAssemblies.SelectMany(_libraryManager.GetReferencingLibraries)
                                      .Distinct()
                                      .Where(IsCandidateLibrary);
        }

        private static Assembly Load(AssemblyName assemblyName)
        {
            return Assembly.Load(assemblyName);
        }

        private bool IsCandidateLibrary(Library library)
        {
            Debug.Assert(ReferenceAssemblies != null);
            return !ReferenceAssemblies.Contains(library.Name);
        }
    }
}