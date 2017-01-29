// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Infrastructure
{
    public struct EnvironmentInfo
    {
        public static readonly EnvironmentInfo Empty = new EnvironmentInfo(new Dictionary<string, string>());
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

        public static bool operator ==(EnvironmentInfo left, EnvironmentInfo right) { return left.Equals(right); }

        public static bool operator !=(EnvironmentInfo left, EnvironmentInfo right) { return !left.Equals(right); }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is EnvironmentInfo && Equals((EnvironmentInfo)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EntryAssemblyName?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (EntryAssemblyVersion?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (HostName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (LocalTimeString?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (MachineName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (OperatingSystem?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (OperatingSystemVersion?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ProcessName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (ProcessorCount?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public bool Equals(EnvironmentInfo other)
        {
            return string.Equals(EntryAssemblyName, other.EntryAssemblyName) && string.Equals(EntryAssemblyVersion, other.EntryAssemblyVersion) &&
                   string.Equals(HostName, other.HostName) && string.Equals(LocalTimeString, other.LocalTimeString) &&
                   string.Equals(MachineName, other.MachineName) && string.Equals(OperatingSystem, other.OperatingSystem) &&
                   string.Equals(OperatingSystemVersion, other.OperatingSystemVersion) && string.Equals(ProcessName, other.ProcessName) &&
                   string.Equals(ProcessorCount, other.ProcessorCount);
        }
    }
}