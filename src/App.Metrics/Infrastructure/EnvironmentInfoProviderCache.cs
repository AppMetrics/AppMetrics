// <copyright file="EnvironmentInfoProviderCache.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Microsoft.DotNet.PlatformAbstractions;

namespace App.Metrics.Infrastructure
{
    internal class EnvironmentInfoProviderCache
    {
        private EnvironmentInfoProviderCache()
        {
            var process = Process.GetCurrentProcess();

            ProcessName = StringExtensions.GetSafeString(() => process.ProcessName);
            OperatingSystemVersion = RuntimeEnvironment.OperatingSystemVersion;
            Os = RuntimeEnvironment.OperatingSystem;
            ProcessorCount = Environment.ProcessorCount.ToString();
            MachineName = process.MachineName;
            HostName = StringExtensions.GetSafeString(Dns.GetHostName);

            var entryAssembly = Assembly.GetEntryAssembly();

            EntryAssemblyName = StringExtensions.GetSafeString(() => entryAssembly.GetName().Name);
            EntryAssemblyVersion = StringExtensions.GetSafeString(() => entryAssembly.GetName().Version.ToString());
        }

        public static EnvironmentInfoProviderCache Instance { get; } = new EnvironmentInfoProviderCache();

        public string EntryAssemblyName { get; }

        public string EntryAssemblyVersion { get; }

        public string HostName { get; }

        public string MachineName { get; }

        public string OperatingSystemVersion { get; }

        public string Os { get; }

        public string ProcessName { get; }

        public string ProcessorCount { get; }
    }
}