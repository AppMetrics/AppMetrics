// <copyright file="EnvironmentInfo.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

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
            RunningEnvironment = entries.FirstOrDefault(e => string.Equals(e.Key, "RunningEnvironment", StringComparison.OrdinalIgnoreCase)).
                                           Value;
            FrameworkDescription = entries.FirstOrDefault(e => string.Equals(e.Key, "FrameworkDescription", StringComparison.OrdinalIgnoreCase)).
                                           Value;
            MachineName = entries.FirstOrDefault(e => string.Equals(e.Key, "MachineName", StringComparison.OrdinalIgnoreCase)).Value;
            ProcessArchitecture = entries.FirstOrDefault(e => string.Equals(e.Key, "ProcessArchitecture", StringComparison.OrdinalIgnoreCase)).Value;
            OperatingSystemPlatform =
                entries.FirstOrDefault(e => string.Equals(e.Key, "OperatingSystemPlatform", StringComparison.OrdinalIgnoreCase)).Value;
            OperatingSystemVersion = entries.FirstOrDefault(e => string.Equals(e.Key, "OperatingSystemVersion", StringComparison.OrdinalIgnoreCase)).
                                             Value;
            OperatingSystemArchitecture =
                entries.FirstOrDefault(e => string.Equals(e.Key, "OperatingSystemArchitecture", StringComparison.OrdinalIgnoreCase)).Value;
            ProcessorCount = entries.FirstOrDefault(e => string.Equals(e.Key, "CPUCount", StringComparison.OrdinalIgnoreCase)).Value;
            LocalTimeString = entries.FirstOrDefault(e => string.Equals(e.Key, "LocalTime", StringComparison.OrdinalIgnoreCase)).Value;
            EntryAssemblyName = entries.FirstOrDefault(e => string.Equals(e.Key, "EntryAssemblyName", StringComparison.OrdinalIgnoreCase)).Value;
            EntryAssemblyVersion = entries.FirstOrDefault(e => string.Equals(e.Key, "EntryAssemblyVersion", StringComparison.OrdinalIgnoreCase)).
                                           Value;

            _entries = new[]
                       {
                           new EnvironmentInfoEntry("RunningEnvironment", RunningEnvironment),
                           new EnvironmentInfoEntry("FrameworkDescription", FrameworkDescription),
                           new EnvironmentInfoEntry("MachineName", MachineName),
                           new EnvironmentInfoEntry("OperatingSystemPlatform", OperatingSystemPlatform),
                           new EnvironmentInfoEntry("OperatingSystemVersion", OperatingSystemVersion),
                           new EnvironmentInfoEntry("OperatingSystemArchitecture", OperatingSystemArchitecture),
                           new EnvironmentInfoEntry("CPUCount", ProcessorCount),
                           new EnvironmentInfoEntry("LocalTime", LocalTimeString),
                           new EnvironmentInfoEntry("EntryAssemblyName", EntryAssemblyName),
                           new EnvironmentInfoEntry("EntryAssemblyVersion", EntryAssemblyVersion)
                       };
        }

        public EnvironmentInfo(
            string runningEnvironment,
            string frameworkDescription,
            string entryAssemblyName,
            string entryAssemblyVersion,
            string localTimeString,
            string machineName,
            string operatingSystemPlatform,
            string operatingSystemVersion,
            string operatingSystemArchitecture,
            string processArchitecture,
            string processorCount)
        {
            RunningEnvironment = runningEnvironment;
            FrameworkDescription = frameworkDescription;
            EntryAssemblyName = entryAssemblyName;
            EntryAssemblyVersion = entryAssemblyVersion;
            LocalTimeString = localTimeString;
            MachineName = machineName;
            OperatingSystemArchitecture = operatingSystemArchitecture;
            OperatingSystemPlatform = operatingSystemPlatform;
            OperatingSystemVersion = operatingSystemVersion;
            ProcessArchitecture = processArchitecture;
            ProcessorCount = processorCount;

            _entries = new[]
                       {
                           new EnvironmentInfoEntry("RunningEnvironment", RunningEnvironment),
                           new EnvironmentInfoEntry("FrameworkDescription", FrameworkDescription),
                           new EnvironmentInfoEntry("MachineName", MachineName),
                           new EnvironmentInfoEntry("ProcessArchitecture", ProcessArchitecture),
                           new EnvironmentInfoEntry("OperatingSystemArchitecture", OperatingSystemArchitecture),
                           new EnvironmentInfoEntry("OperatingSystemPlatform", OperatingSystemPlatform),
                           new EnvironmentInfoEntry("OperatingSystemVersion", OperatingSystemVersion),
                           new EnvironmentInfoEntry("CPUCount", ProcessorCount),
                           new EnvironmentInfoEntry("LocalTime", LocalTimeString),
                           new EnvironmentInfoEntry("EntryAssemblyName", EntryAssemblyName),
                           new EnvironmentInfoEntry("EntryAssemblyVersion", EntryAssemblyVersion)
                       };
        }

        public IEnumerable<EnvironmentInfoEntry> Entries => _entries ?? Enumerable.Empty<EnvironmentInfoEntry>();

        public string RunningEnvironment { get; }

        public string EntryAssemblyName { get; }

        public string EntryAssemblyVersion { get; }

        public string FrameworkDescription { get; }

        public string LocalTimeString { get; }

        public string MachineName { get; }

        public string OperatingSystemArchitecture { get; }

        public string OperatingSystemPlatform { get; }

        public string OperatingSystemVersion { get; }

        public string ProcessArchitecture { get; }

        public string ProcessorCount { get; }

        public static bool operator ==(EnvironmentInfo left, EnvironmentInfo right) { return left.Equals(right); }

        public static bool operator !=(EnvironmentInfo left, EnvironmentInfo right) { return !left.Equals(right); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is EnvironmentInfo && Equals((EnvironmentInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FrameworkDescription != null ? FrameworkDescription.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (RunningEnvironment != null ? RunningEnvironment.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (EntryAssemblyName != null ? EntryAssemblyName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (EntryAssemblyVersion != null ? EntryAssemblyVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LocalTimeString != null ? LocalTimeString.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MachineName != null ? MachineName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OperatingSystemPlatform != null ? OperatingSystemPlatform.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OperatingSystemArchitecture != null ? OperatingSystemArchitecture.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OperatingSystemVersion != null ? OperatingSystemVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ProcessArchitecture != null ? ProcessArchitecture.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ProcessorCount != null ? ProcessorCount.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(EnvironmentInfo other)
        {
            return string.Equals(FrameworkDescription, other.FrameworkDescription) && string.Equals(EntryAssemblyName, other.EntryAssemblyName) &&
                   string.Equals(RunningEnvironment, other.RunningEnvironment) &&
                   string.Equals(EntryAssemblyVersion, other.EntryAssemblyVersion) &&
                   string.Equals(LocalTimeString, other.LocalTimeString) && string.Equals(MachineName, other.MachineName) &&
                   string.Equals(OperatingSystemPlatform, other.OperatingSystemPlatform) &&
                   string.Equals(OperatingSystemArchitecture, other.OperatingSystemArchitecture) &&
                   string.Equals(OperatingSystemVersion, other.OperatingSystemVersion) &&
                   string.Equals(ProcessArchitecture, other.ProcessArchitecture) &&
                   string.Equals(ProcessorCount, other.ProcessorCount);
        }
    }
}