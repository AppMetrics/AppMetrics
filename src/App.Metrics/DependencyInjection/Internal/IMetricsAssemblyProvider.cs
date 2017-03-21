// <copyright file="IMetricsAssemblyProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.DependencyInjection.Internal
{
    /// <summary>
    ///     Specifies the contract for discovering assemblies that may contain App.Metrics specific types such as health
    ///     checks.
    /// </summary>
    internal interface IMetricsAssemblyProvider
    {
        /// <summary>
        ///     Gets the sequence of candidate <see cref="Assembly" /> instances that the application
        ///     uses for discovery of App.Metrics specific types.
        /// </summary>
        /// <value>
        ///     The candidate assemblies.
        /// </value>
        IEnumerable<Assembly> CandidateAssemblies { get; }
    }
}