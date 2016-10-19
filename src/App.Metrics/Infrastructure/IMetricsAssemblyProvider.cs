// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Reflection;

namespace App.Metrics.Infrastructure
{
    /// <summary>
    ///     Specifies the contract for discovering assemblies that may contain App.Metrics specific types such as health
    ///     checks.
    /// </summary>
    public interface IMetricsAssemblyProvider
    {
        /// <summary>
        ///     Gets the sequence of candidate <see cref="Assembly" /> instances that the application
        ///     uses for discovery of App.Metrics specific types.
        /// </summary>
        IEnumerable<Assembly> CandidateAssemblies { get; }
    }
}