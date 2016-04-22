using System.Collections.Generic;
using System.Reflection;

namespace AspNet.Metrics.Infrastructure
{
    /// <summary>
    /// Specifies the contract for discovering assemblies that may contain Mvc specific types such as controllers,
    /// view components and precompiled views.
    /// </summary>
    public interface IAssemblyProvider
    {
        /// <summary>
        /// Gets the sequence of candidate <see cref="Assembly"/> instances that the application
        /// uses for discovery of Mvc specific types.
        /// </summary>
        IEnumerable<Assembly> CandidateAssemblies { get; }
    }
}
