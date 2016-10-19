using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using App.Metrics.Health;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
// ReSharper restore CheckNamespace
{
    internal static class MetricsEventIds
    {
        public static class HealthChecks
        {
            public const int GetStatusExecuted = HealthCheckStart + 2;
            public const int GetStatusExecuting = HealthCheckStart + 1;
            private const int HealthCheckStart = 999;
        }
    }

    internal static class AppMetricsCoreLoggerExtensions
    {
        private static readonly Action<ILogger, double, int, Exception> HealthGetStatusExecuted;
        private static readonly Action<ILogger, double, int, int, IEnumerable<string>, Exception> HealthGetStatusExecutedFailed;
        private static readonly Action<ILogger, Exception> HealthGetStatusExecutedNoResults;
        private static readonly double TimestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        static AppMetricsCoreLoggerExtensions()
        {
            HealthGetStatusExecuted = LoggerMessage.Define<double, int>(
                LogLevel.Information,
                eventId: MetricsEventIds.HealthChecks.GetStatusExecuted,
                formatString: $"Executed {nameof(HealthStatus)}, in {{elapsedMilliseconds}}ms, IsHealthy: True. {{checksPassed}} health check results passed.");

            HealthGetStatusExecutedFailed = LoggerMessage.Define<double, int, int, IEnumerable<string>>(
                LogLevel.Information,
                eventId: MetricsEventIds.HealthChecks.GetStatusExecuted,
                formatString:
                $"Executed {nameof(HealthStatus)}, in {{elapsedMilliseconds}}ms, IsHealthy: False. {{checksPassed}} health check results passed. {{checksFailed}} health check results failed. Failed Checks: {{failedChecks}}");

            HealthGetStatusExecutedNoResults = LoggerMessage.Define(
                LogLevel.Information,
                eventId: MetricsEventIds.HealthChecks.GetStatusExecuted,
                formatString: $"Executed {nameof(HealthStatus)}, 0 health check results.");
        }

        public static void HealthCheckGetStatusExecuting(this ILogger logger)
        {
            logger.LogDebug(MetricsEventIds.HealthChecks.GetStatusExecuting, "Executing HealthCheck Get Status");
        }

        public static void HealthCheckGetStatusExecuted(this ILogger logger, HealthStatus healthStatus, 
            long startTimestamp)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                if (startTimestamp != 0)
                {
                    var currentTimestamp = Stopwatch.GetTimestamp();
                    var elapsed = new TimeSpan((long)(TimestampToTicks * (currentTimestamp - startTimestamp)));

                    if (healthStatus.HasRegisteredChecks)
                    {
                        if (healthStatus.IsHealthy)
                        {
                            HealthGetStatusExecuted(logger, elapsed.TotalMilliseconds, healthStatus.Results.Length, null);
                            return;
                        }

                        var checksFailed = healthStatus.Results.Count(x => !x.Check.IsHealthy);
                        var checksPassed = healthStatus.Results.Count(x => x.Check.IsHealthy);
                        var failedChecks = healthStatus.Results.Where(h => h.Check.IsHealthy).Select(h => h.Name);

                        HealthGetStatusExecutedFailed(logger, elapsed.TotalMilliseconds, checksFailed, checksPassed, failedChecks, null);
                    }

                    HealthGetStatusExecutedNoResults(logger, null);
                }
            }
        }
    }
}