// <copyright file="StaticMetricsAssemblyProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.DependencyInjection.Internal
{
    /// <summary>
    ///     A <see cref="IMetricsAssemblyProvider" /> with a fixed set of candidate assemblies.
    /// </summary>
    internal sealed class StaticMetricsAssemblyProvider : IMetricsAssemblyProvider
    {
        /// <summary>
        ///     Gets the list of candidate assemblies.
        /// </summary>
        /// <value>
        ///     The candidate assemblies.
        /// </value>
        public IList<Assembly> CandidateAssemblies { get; } = new List<Assembly>();

        /// <summary>
        ///     Gets the sequence of candidate <see cref="Assembly" /> instances that the application
        ///     uses for discovery of App.Metrics specific types.
        /// </summary>
        /// <value>
        ///     The candidate assemblies.
        /// </value>
        IEnumerable<Assembly> IMetricsAssemblyProvider.CandidateAssemblies => CandidateAssemblies;
    }
}