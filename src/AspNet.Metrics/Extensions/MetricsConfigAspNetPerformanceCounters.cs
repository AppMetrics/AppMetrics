using System.Diagnostics;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace

namespace Metrics
// ReSharper restore CheckNamespace
{
    //https://msdn.microsoft.com/en-us/library/fxk122b4.aspx
    public static class MetricsConfigAspNetPerformanceCounters
    {
        public static PerformanceCounter[] PerfCounters = new PerformanceCounter[10];

        public static MetricsConfig WithAspNetPerformanceCounters(this MetricsConfig config,
            string contextName = "ASPNET.Application")
        {
            config.WithConfigExtension((context, func) =>
            {
                SetUpPerformanceCounters();

                foreach (var perfCounter in PerfCounters)
                {
                    context.Context(contextName)
                        .PerformanceCounter(perfCounter.CounterName, perfCounter.CategoryName, perfCounter.CounterName, perfCounter.InstanceName,
                            Unit.Items, "aspnet");
                }
            });
            return config;
        }

        private static void SetUpPerformanceCounters()
        {
            var aspNetApplicationFields = typeof(AspNetApplicationPerformanceCounters)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral);

            var aspNetFields = typeof(AspNetPerformanceCounters)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral);

            var i = 0;
            var aspNetApplicationPerfCounters = new PerformanceCounter[aspNetApplicationFields.Count()];
            foreach (var field in aspNetApplicationFields)
            {
                aspNetApplicationPerfCounters[i] = new PerformanceCounter
                {
                    CategoryName = "ASP.NET Applications",
                    CounterName = field.GetValue(null) as string,
                    InstanceName = "__Total__"
                };
                i++;
            }

            i = 0;
            var aspNetPerCounters = new PerformanceCounter[aspNetFields.Count()];
            foreach (var field in aspNetFields)
            {
                aspNetPerCounters[i] = new PerformanceCounter
                {
                    CategoryName = "ASP.NET",
                    CounterName = field.GetValue(null) as string
                };
                i++;
            }

            PerfCounters = aspNetApplicationPerfCounters.Concat(aspNetPerCounters).Where(c => c != null).ToArray();
        }

        internal class AspNetApplicationPerformanceCounters
        {
            public const string AnonymousRequests = "Anonymous Requests";
            public const string AnonymousRequestsSec = "Anonymous Requests/Sec";
            public const string RequestsExecuting = "Requests Executing";
            public const string RequestsFailed = "Requests Failed";
            public const string RequestsNotAuthorized = "Requests Not Authorized";
            public const string RequestsNotFound = "Requests Not Found";
            public const string RequestsSec = "Requests/Sec";
            public const string RequestsSucceeded = "Requests Succeeded";
            public const string RequestsTimedOut = "Requests Timed Out";
        }

        internal class AspNetPerformanceCounters
        {
            public const string ApplicationRestarts = "Application Restarts";
            public const string RequestsDisconnected = "Requests Disconnected";
            public const string RequestsQueued = "Requests Queued";
            public const string RequestsRejected = "Requests Rejected";
            public const string WorkerProcessRestarts = "Worker Process Restarts";
        }
    }
}