// <copyright file="StaticHealthAssemblyProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Health.Extensions.DependencyInjection.Internal
{
    /// <summary>
    ///     Fixed set of candidate assemblies.
    /// </summary>
    internal sealed class StaticHealthAssemblyProvider
    {
        /// <summary>
        ///     Gets the list of candidate assemblies.
        /// </summary>
        public IList<Assembly> CandidateAssemblies { get; } = new List<Assembly>();
    }
}