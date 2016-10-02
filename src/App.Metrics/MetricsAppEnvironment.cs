using System;
using Microsoft.Extensions.PlatformAbstractions;

namespace App.Metrics
{
    public class MetricsAppEnvironment : IMetricsEnvironment
    {
        public MetricsAppEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            if (applicationEnvironment == null) throw new ArgumentNullException(nameof(applicationEnvironment));

            ApplicationName = applicationEnvironment.ApplicationName;
            ApplicationVersion = applicationEnvironment.ApplicationVersion;
            RuntimeFramework = applicationEnvironment.RuntimeFramework.Identifier;
            RuntimeFrameworkVersion = applicationEnvironment.RuntimeFramework.Version.ToString();
        }

        public string ApplicationName { get; }

        public string ApplicationVersion { get; }

        public string RuntimeFramework { get; }

        public string RuntimeFrameworkVersion { get;  }
    }
}