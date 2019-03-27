// <copyright file="EnvironmentInfoProviderCache.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Reflection;
#if !NET452
using System.Runtime.InteropServices;
#endif

namespace App.Metrics.Infrastructure
{
    internal class EnvironmentInfoProviderCache
    {
        private EnvironmentInfoProviderCache()
        {
            ProcessArchitecture = StringExtensions.GetSafeString(GetProcessArchitecture);
            OperatingSystemVersion = StringExtensions.GetSafeString(GetOSVersion);
            OperatingSystemPlatform = StringExtensions.GetSafeString(GetOSPlatform);
            OperatingSystemArchitecture = StringExtensions.GetSafeString(GetOSArchitecture);
            ProcessorCount = StringExtensions.GetSafeString(() => Environment.ProcessorCount.ToString());
            MachineName = StringExtensions.GetSafeString(() => Environment.MachineName);
            FrameworkDescription = StringExtensions.GetSafeString(GetFrameworkDescription);

            var aspnetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (aspnetCoreEnv.IsPresent())
            {
                // ReSharper disable PossibleNullReferenceException checked with .IsPresent
                RunningEnvironment = aspnetCoreEnv.ToLowerInvariant();
                // ReSharper restore PossibleNullReferenceException
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

        #if NETSTANDARD2_0
        // ReSharper disable InconsistentNaming
        private static string GetOSPlatform()
            // ReSharper restore InconsistentNaming
        {
            var platform = OSPlatform.Create("Other Platform");
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            platform = isWindows ? OSPlatform.Windows : platform;
            var isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            platform = isOsx ? OSPlatform.OSX : platform;
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            platform = isLinux ? OSPlatform.Linux : platform;
            return platform.ToString();
        }
#else
        // ReSharper disable InconsistentNaming
        private static string GetOSPlatform() { return Environment.OSVersion.Platform.ToString(); }
        // ReSharper restore InconsistentNaming
#endif

#if NETSTANDARD2_0
        // ReSharper disable InconsistentNaming
        private static string GetProcessArchitecture()
            // ReSharper restore InconsistentNaming
        {
            return RuntimeInformation.ProcessArchitecture.ToString();
        }
#else
        // ReSharper disable InconsistentNaming
        private static string GetProcessArchitecture() { return Environment.Is64BitProcess ? "X64" : "X86"; }
        // ReSharper restore InconsistentNaming
#endif

#if NETSTANDARD2_0
        // ReSharper disable InconsistentNaming
        private static string GetOSArchitecture()
            // ReSharper restore InconsistentNaming
        {
            return RuntimeInformation.OSArchitecture.ToString();
        }
#else
        // ReSharper disable InconsistentNaming
        private static string GetOSArchitecture() { return Environment.Is64BitOperatingSystem ? "X64" : "X86"; }
        // ReSharper restore InconsistentNaming
#endif

#if NETSTANDARD2_0
        // ReSharper disable InconsistentNaming
        private static string GetOSVersion()
            // ReSharper restore InconsistentNaming
        {
            return RuntimeInformation.OSDescription;
        }
#else
// ReSharper disable InconsistentNaming
        private static string GetOSVersion()
        {
            // DEVNOTE: Not ideal but it's a pain to get detect the actual OS Version
            return Environment.OSVersion.VersionString;
        }

        // ReSharper restore InconsistentNaming
#endif

#if NETSTANDARD2_0
        // ReSharper disable InconsistentNaming
        private static string GetFrameworkDescription()
            // ReSharper restore InconsistentNaming
        {
            return RuntimeInformation.FrameworkDescription;
        }
#else
        // ReSharper disable InconsistentNaming
        private static string GetFrameworkDescription() { return AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName; }
        // ReSharper restore InconsistentNaming
#endif
    }
}