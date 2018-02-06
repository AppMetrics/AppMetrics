// <copyright file="EnvironmentInfoProviderCache.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace App.Metrics.Infrastructure
{
    internal class EnvironmentInfoProviderCache
    {
        private EnvironmentInfoProviderCache()
        {
            var platform = GetOSPlatform();

            ProcessArchitecture = StringExtensions.GetSafeString(() => RuntimeInformation.ProcessArchitecture.ToString());
            OperatingSystemVersion = StringExtensions.GetSafeString(() => RuntimeInformation.OSDescription);
            OperatingSystemPlatform = StringExtensions.GetSafeString(() => platform.ToString());
            OperatingSystemArchitecture = StringExtensions.GetSafeString(() => RuntimeInformation.OSArchitecture.ToString());
            ProcessorCount = StringExtensions.GetSafeString(() => Environment.ProcessorCount.ToString());
            MachineName = StringExtensions.GetSafeString(() => Environment.MachineName);
            FrameworkDescription = StringExtensions.GetSafeString(() => RuntimeInformation.FrameworkDescription);

            var aspnetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (aspnetCoreEnv.IsPresent())
            {
                RunningEnvironment = aspnetCoreEnv.ToLowerInvariant();
            }
            else
            {
#if DEBUG
                RunningEnvironment = "debug";
#else
                RunningEnvironment = "release";
#endif
            }

            var entryAssembly = Assembly.GetEntryAssembly();
            EntryAssemblyName = StringExtensions.GetSafeString(() => entryAssembly?.GetName().Name ?? "unknown");
            EntryAssemblyVersion = StringExtensions.GetSafeString(() => entryAssembly?.GetName().Version.ToString() ?? "unknown");
        }

        public static EnvironmentInfoProviderCache Instance { get; } = new EnvironmentInfoProviderCache();

        public string EntryAssemblyName { get; }

        public string EntryAssemblyVersion { get; }

        public string FrameworkDescription { get; }

        public string MachineName { get; }

        public string OperatingSystemArchitecture { get; }

        public string OperatingSystemPlatform { get; }

        public string OperatingSystemVersion { get; }

        public string ProcessArchitecture { get; }

        public string ProcessorCount { get; }

        public string RunningEnvironment { get; }

        private static OSPlatform GetOSPlatform()
        {
            var platform = OSPlatform.Create("Other Platform");
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            platform = isWindows ? OSPlatform.Windows : platform;
            var isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            platform = isOsx ? OSPlatform.OSX : platform;
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            platform = isLinux ? OSPlatform.Linux : platform;
            return platform;
        }
    }
}