// <copyright file="AppMetricsEnvironment.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Runtime.Versioning;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public sealed class AppMetricsEnvironment : IAppMetricsEnvironment
    {
        public AppMetricsEnvironment()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            ApplicationName = AppDomain.CurrentDomain?.FriendlyName ?? "unknown";
            ApplicationVersion = entryAssembly?.GetName()?.Version.ToString() ?? "1.0.0.0";
            RuntimeFramework = entryAssembly?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkDisplayName ?? "unknown";
            RuntimeFrameworkVersion = entryAssembly?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName ?? "unknown";
        }

        public string ApplicationName { get; }

        public string ApplicationVersion { get; }

        public string RuntimeFramework { get; }

        public string RuntimeFrameworkVersion { get; }
    }
}