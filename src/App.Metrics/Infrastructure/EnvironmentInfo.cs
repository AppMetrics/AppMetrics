// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Infrastructure
{
    public struct EnvironmentInfo
    {
        public static EnvironmentInfo Empty;
        private readonly IEnumerable<EnvironmentInfoEntry> _entries;

        public EnvironmentInfo(IDictionary<string, string> entries)
        {
            MachineName = entries.FirstOrDefault(e => string.Equals(e.Key, "MachineName", StringComparison.OrdinalIgnoreCase)).Value;
            ProcessName = entries.FirstOrDefault(e => string.Equals(e.Key, "ProcessName", StringComparison.OrdinalIgnoreCase)).Value;
            OperatingSystem = entries.FirstOrDefault(e => string.Equals(e.Key, "OS", StringComparison.OrdinalIgnoreCase)).Value;
            OperatingSystemVersion = entries.FirstOrDefault(e => string.Equals(e.Key, "OSVersion", StringComparison.OrdinalIgnoreCase)).Value;
            ProcessorCount = entries.FirstOrDefault(e => string.Equals(e.Key, "CPUCount", StringComparison.OrdinalIgnoreCase)).Value;
            HostName = entries.FirstOrDefault(e => string.Equals(e.Key, "HostName", StringComparison.OrdinalIgnoreCase)).Value;
            LocalTimeString = entries.FirstOrDefault(e => string.Equals(e.Key, "LocalTime", StringComparison.OrdinalIgnoreCase)).Value;
            EntryAssemblyName = entries.FirstOrDefault(e => string.Equals(e.Key, "EntryAssemblyName", StringComparison.OrdinalIgnoreCase)).Value;
            EntryAssemblyVersion = entries.FirstOrDefault(e => string.Equals(e.Key, "EntryAssemblyVersion", StringComparison.OrdinalIgnoreCase)).Value;

            _entries = new[]
                       {
                           new EnvironmentInfoEntry("MachineName", MachineName),
                           new EnvironmentInfoEntry("ProcessName", ProcessName),
                           new EnvironmentInfoEntry("OS", OperatingSystem),
                           new EnvironmentInfoEntry("OSVersion", OperatingSystemVersion),
                           new EnvironmentInfoEntry("CPUCount", ProcessorCount),
                           new EnvironmentInfoEntry("HostName", HostName),
                           new EnvironmentInfoEntry("LocalTime", LocalTimeString),
                           new EnvironmentInfoEntry("EntryAssemblyName", EntryAssemblyName),
                           new EnvironmentInfoEntry("EntryAssemblyVersion", EntryAssemblyVersion)
                       };
        }

        public EnvironmentInfo(
            string entryAssemblyName,
            string entryAssemblyVersion,
            string hostName,
            string localTimeString,
            string machineName,
            string operatingSystem,
            string operatingSystemVersion,
            string processName,
            string processorCount)
        {
            EntryAssemblyName = entryAssemblyName;
            EntryAssemblyVersion = entryAssemblyVersion;
            HostName = hostName;
            LocalTimeString = localTimeString;
            MachineName = machineName;
            OperatingSystem = operatingSystem;
            OperatingSystemVersion = operatingSystemVersion;
            ProcessName = processName;
            ProcessorCount = processorCount;

            _entries = new[]
                       {
                           new EnvironmentInfoEntry("MachineName", MachineName),
                           new EnvironmentInfoEntry("ProcessName", ProcessName),
                           new EnvironmentInfoEntry("OS", OperatingSystem),
                           new EnvironmentInfoEntry("OSVersion", OperatingSystemVersion),
                           new EnvironmentInfoEntry("CPUCount", ProcessorCount),
                           new EnvironmentInfoEntry("HostName", HostName),
                           new EnvironmentInfoEntry("LocalTime", LocalTimeString),
                           new EnvironmentInfoEntry("EntryAssemblyName", EntryAssemblyName),
                           new EnvironmentInfoEntry("EntryAssemblyVersion", EntryAssemblyVersion)
                       };
        }

        public IEnumerable<EnvironmentInfoEntry> Entries => _entries ?? Enumerable.Empty<EnvironmentInfoEntry>();

        public string EntryAssemblyName { get; }

        public string EntryAssemblyVersion { get; }

        public string HostName { get; }

        public string LocalTimeString { get; }

        public string MachineName { get; }

        public string OperatingSystem { get; }

        public string OperatingSystemVersion { get; }

        public string ProcessName { get; }

        public string ProcessorCount { get; }
    }
}