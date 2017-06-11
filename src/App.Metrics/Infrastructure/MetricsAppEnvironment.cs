// <copyright file="MetricsAppEnvironment.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Runtime.Versioning;

namespace App.Metrics.Infrastructure
{
    public sealed class MetricsAppEnvironment : IMetricsEnvironment
    {
        public MetricsAppEnvironment()
        {
            ApplicationName = AppDomain.CurrentDomain.FriendlyName;
            ApplicationVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            RuntimeFramework = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkDisplayName;
            RuntimeFrameworkVersion = Assembly.GetEntryAssembly().GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName;
        }

        public string ApplicationName { get; }

        public string ApplicationVersion { get; }

        public string RuntimeFramework { get; }

        public string RuntimeFrameworkVersion { get; }
    }
}