// <copyright file="MetricsAppEnvironment.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using Microsoft.Extensions.PlatformAbstractions;

#if NET452
using System;
using System.Reflection;
using App.Metrics.Core.Internal;
#endif

namespace App.Metrics.Infrastructure
{
    public sealed class MetricsAppEnvironment : IMetricsEnvironment
    {
#if NET452
        [AppMetricsExcludeFromCodeCoverage] // DEVNOTE: Excluding for now, don't think the it's worth the effort in testing net452 at this time.
        public MetricsAppEnvironment(ApplicationEnvironment applicationEnvironment, AssemblyName executingAssemblyName)
        {
            if (applicationEnvironment == null)
            {
                throw new ArgumentNullException(nameof(applicationEnvironment));
            }

            if (executingAssemblyName == null)
            {
                throw new ArgumentNullException(nameof(executingAssemblyName));
            }

            ApplicationName = executingAssemblyName.Name;
            ApplicationVersion = executingAssemblyName.Version.ToString();
            RuntimeFramework = applicationEnvironment.RuntimeFramework.Identifier;
            RuntimeFrameworkVersion = applicationEnvironment.RuntimeFramework.Version.ToString();
        }
#endif

#if !NET452
        public MetricsAppEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            ApplicationName = applicationEnvironment.ApplicationName;
            ApplicationVersion = applicationEnvironment.ApplicationVersion;
            RuntimeFramework = applicationEnvironment.RuntimeFramework.Identifier;
            RuntimeFrameworkVersion = applicationEnvironment.RuntimeFramework.Version.ToString();
        }
#endif

        public string ApplicationName { get; }

        public string ApplicationVersion { get; }

        public string RuntimeFramework { get; }

        public string RuntimeFrameworkVersion { get; }
    }
}