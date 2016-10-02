using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Infrastructure
{
    /// <summary>
    ///     Specifies the contract for discovering assemblies that may contain Metrics.Net specific types such as health
    ///     checks.
    /// </summary>
    public interface IMetricsAssemblyProvider
    {
        /// <summary>
        ///     Gets the sequence of candidate <see cref="Assembly" /> instances that the application
        ///     uses for discovery of Metrics.Net specific types.
        /// </summary>
        IEnumerable<Assembly> CandidateAssemblies { get; }
    }
}