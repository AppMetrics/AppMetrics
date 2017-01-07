// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Reflection;
using Microsoft.Extensions.PlatformAbstractions;

namespace App.Metrics
{
    public sealed class MetricsAppEnvironment : IMetricsEnvironment
    {
        public MetricsAppEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            if (applicationEnvironment == null) throw new ArgumentNullException(nameof(applicationEnvironment));

#if NET452
            var assemblyName = Assembly.GetEntryAssembly().GetName();
            ApplicationName = assemblyName.Name;
#else
            ApplicationName = applicationEnvironment.ApplicationName;
#endif

#if NET452
            ApplicationVersion = assemblyName.Version.ToString();
#else
            ApplicationVersion = applicationEnvironment.ApplicationVersion;
#endif

            RuntimeFramework = applicationEnvironment.RuntimeFramework.Identifier;
            RuntimeFrameworkVersion = applicationEnvironment.RuntimeFramework.Version.ToString();
        }

        public string ApplicationName { get; }

        public string ApplicationVersion { get; }

        public string RuntimeFramework { get; }

        public string RuntimeFrameworkVersion { get; }
    }
}