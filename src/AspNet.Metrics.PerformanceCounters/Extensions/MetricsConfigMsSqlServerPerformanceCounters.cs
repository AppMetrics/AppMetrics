using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

// ReSharper disable CheckNamespace

namespace Metrics
// ReSharper restore CheckNamespace
{
    //https://msdn.microsoft.com/en-us/library/ms254503(v=vs.110).aspx
    public static class MetricsConfigMsSqlServerPerformanceCounters
    {
        public static PerformanceCounter[] PerfCounters = new PerformanceCounter[10];

        private enum ADONetPerformanceCounters
        {
            NumberOfActiveConnectionPools,
            NumberOfReclaimedConnections,
            HardConnectsPerSecond,
            HardDisconnectsPerSecond,
            NumberOfActiveConnectionPoolGroups,
            NumberOfInactiveConnectionPoolGroups,
            NumberOfInactiveConnectionPools,
            NumberOfNonPooledConnections,
            NumberOfPooledConnections,
            NumberOfStasisConnections
            // The following performance counters are more expensive to track.
            // Enable ConnectionPoolPerformanceCounterDetail in your config file.
            //     SoftConnectsPerSecond
            //     SoftDisconnectsPerSecond
            //     NumberOfActiveConnections
            //     NumberOfFreeConnections
        }

        public static MetricsConfig WithSqlServerPerformanceCounters(this MetricsConfig config,
            string contextName = "MsSqlServer.Application")
        {
            config.WithConfigExtension((context, func) =>
            {
                SetUpPerformanceCounters();

                foreach (var perfCounter in PerfCounters)
                {
                    context.Context(contextName)
                        .PerformanceCounter(perfCounter.CounterName, perfCounter.CategoryName, perfCounter.CounterName, perfCounter.InstanceName,
                            Unit.Items, "database");
                }
            });
            return config;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int GetCurrentProcessId();

        private static string GetInstanceNameInWebContext()
        {
            var instanceName = AppDomain.CurrentDomain.FriendlyName.Replace('(', '[')
                .Replace(')', ']').Replace('#', '_').Replace('/', '_').Replace('\\', '_');

            var pid = GetCurrentProcessId().ToString();
            instanceName = instanceName + "[" + pid + "]";

            return instanceName;
        }

        private static void SetUpPerformanceCounters()
        {
            PerfCounters = new PerformanceCounter[10];
            var instanceName = GetInstanceNameInWebContext();
            var apc = typeof(ADONetPerformanceCounters);
            var i = 0;
            foreach (var s in Enum.GetNames(apc))
            {
                PerfCounters[i] = new PerformanceCounter
                {
                    CategoryName = ".NET Data Provider for SqlServer",
                    CounterName = s,
                    InstanceName = instanceName
                };
                i++;
            }
        }
    }
}