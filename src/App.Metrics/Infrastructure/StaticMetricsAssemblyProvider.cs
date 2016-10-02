using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Infrastructure
{
    /// <summary>
    ///     A <see cref="IMetricsAssemblyProvider" /> with a fixed set of candidate assemblies.
    /// </summary>
    public class StaticMetricsAssemblyProvider : IMetricsAssemblyProvider
    {
        /// <summary>
        ///     Gets the list of candidate assemblies.
        /// </summary>
        public IList<Assembly> CandidateAssemblies { get; } = new List<Assembly>();

        IEnumerable<Assembly> IMetricsAssemblyProvider.CandidateAssemblies => CandidateAssemblies;
    }
}