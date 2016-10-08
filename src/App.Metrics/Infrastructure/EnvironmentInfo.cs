using System.Collections.Generic;
using System.Linq;
using App.Metrics.MetricData;

namespace App.Metrics.Infrastructure
{
    public struct EnvironmentInfo
    {
        private readonly IEnumerable<EnvironmentInfoEntry> _entries;

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