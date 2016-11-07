// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Infrastructure
{
    public struct EnvironmentInfo
    {
        private readonly IEnumerable<EnvironmentInfoEntry> _entries;

        public EnvironmentInfo(IDictionary<string, string> entries)
        {
            MachineName = entries.FirstOrDefault(e => string.Equals(e.Key, "MachineName", StringComparison.OrdinalIgnoreCase)).Value;
            ProcessName = entries.FirstOrDefault(e => string.Equals(e.Key, "ProcessName", StringComparison.OrdinalIgnoreCase)).Value;
            OperatingSystem = entries.FirstOrDefault(e => string.Equals(e.Key, "OS", StringComparison.OrdinalIgnoreCase)).Value;
            OperatingSystemVersion = entries.FirstOrDefault(e => string.Equals(e.Key, "OSVersion", StringComparison.OrdinalIgnoreCase)).Value;
            ProcessorCount = entries.FirstOrDefault(e => string.Equals(e.Key, "CPUCount", StringComparison.OrdinalIgnoreCase)).Value;
            HostName = entries.FirstOrDefault(e => string.Equals(e.Key, "HostName", StringComparison.OrdinalIgnoreCase)).Value;
            IpAddress = entries.FirstOrDefault(e => string.Equals(e.Key, "IPAddress", StringComparison.OrdinalIgnoreCase)).Value;
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
                new EnvironmentInfoEntry("IPAddress", IpAddress),
                new EnvironmentInfoEntry("LocalTime", LocalTimeString),
                new EnvironmentInfoEntry("EntryAssemblyName", EntryAssemblyName),
                new EnvironmentInfoEntry("EntryAssemblyVersion", EntryAssemblyVersion)
            };
        }

        public EnvironmentInfo(string entryAssemblyName,
            string entryAssemblyVersion,
            string hostName,
            string ipAddress,
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
            IpAddress = ipAddress;
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
                new EnvironmentInfoEntry("CPUCount", ProcessorCount.ToString()),
                new EnvironmentInfoEntry("HostName", HostName),
                new EnvironmentInfoEntry("IPAddress", IpAddress),
                new EnvironmentInfoEntry("LocalTime", LocalTimeString),
                new EnvironmentInfoEntry("EntryAssemblyName", EntryAssemblyName),
                new EnvironmentInfoEntry("EntryAssemblyVersion", EntryAssemblyVersion)
            };
        }

        public static EnvironmentInfo Empty { get; } = new EnvironmentInfo();

        public IEnumerable<EnvironmentInfoEntry> Entries => _entries ?? Enumerable.Empty<EnvironmentInfoEntry>();

        public string EntryAssemblyName { get; }

        public string EntryAssemblyVersion { get; }

        public string HostName { get; }

        public string IpAddress { get; }

        public string LocalTimeString { get; }

        public string MachineName { get; }

        public string OperatingSystem { get; }

        public string OperatingSystemVersion { get; }

        public string ProcessName { get; }

        public string ProcessorCount { get; }
    }
}