// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Infrastructure
{
    public struct EnvironmentInfo
    {
        private readonly IEnumerable<EnvironmentInfoEntry> _entries;

        public EnvironmentInfo(IDictionary<string, string> entries)
        {
            MachineName = entries.FirstOrDefault(e => e.Key == "MachineName").Value;
            ProcessName = entries.FirstOrDefault(e => e.Key == "ProcessName").Value;
            OperatingSystem = entries.FirstOrDefault(e => e.Key == "OS").Value;
            OperatingSystemVersion = entries.FirstOrDefault(e => e.Key == "OSVersion").Value;
            ProcessorCount = int.Parse(entries.FirstOrDefault(e => e.Key == "CPUCount").Value);
            HostName = entries.FirstOrDefault(e => e.Key == "HostName").Value;
            IpAddress = entries.FirstOrDefault(e => e.Key == "IPAddress").Value;
            LocalTimeString = entries.FirstOrDefault(e => e.Key == "LocalTime").Value;
            EntryAssemblyName = entries.FirstOrDefault(e => e.Key == "EntryAssemblyName").Value;
            EntryAssemblyVersion = entries.FirstOrDefault(e => e.Key == "EntryAssemblyVersion").Value;

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

        public EnvironmentInfo(string entryAssemblyName,
            string entryAssemblyVersion,
            string hostName,
            string ipAddress,
            string localTimeString,
            string machineName,
            string operatingSystem,
            string operatingSystemVersion,
            string processName,
            int processorCount)
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

        public int ProcessorCount { get; }
    }
}