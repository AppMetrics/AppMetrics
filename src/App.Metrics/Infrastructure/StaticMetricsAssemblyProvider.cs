// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Infrastructure
{
    /// <summary>
    ///     A <see cref="IMetricsAssemblyProvider" /> with a fixed set of candidate assemblies.
    /// </summary>
    public sealed class StaticMetricsAssemblyProvider : IMetricsAssemblyProvider
    {
        /// <summary>
        ///     Gets the list of candidate assemblies.
        /// </summary>
        public IList<Assembly> CandidateAssemblies { get; } = new List<Assembly>();

        IEnumerable<Assembly> IMetricsAssemblyProvider.CandidateAssemblies => CandidateAssemblies;
    }
}