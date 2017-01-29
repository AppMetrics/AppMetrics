// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Text;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Health
{
    /// <summary>
    ///     Result of a health check
    /// </summary>
    public struct HealthCheckResult
    {
        /// <summary>
        /// Gets the status message of the check. A status can be provided for both healthy and unhealthy states.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }

        /// <summary>
        ///     True if the check was healthy, degraded or unhealthy.
        /// </summary>
        public readonly HealthCheckStatus Status;

        private HealthCheckResult(HealthCheckStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        ///     Create a Degraded status response. This is useful for when a health check may
        ///     fail but the application itself is still functioning correct. E.g. There could be a
        ///     health check checking the number of messages in a queue, if that number reaches a
        ///     specificied threshold, a degraded status could be returned rather than raising a critical alert.
        /// </summary>
        /// <param name="message">Status message.</param>
        /// <param name="values">Values to format the status message with.</param>
        /// <returns>A degraded health check result</returns>
        public static HealthCheckResult Degraded(string message, params object[] values)
        {
            var status = string.Format(message, values);
            return new HealthCheckResult(
                HealthCheckStatus.Degraded,
                string.IsNullOrWhiteSpace(status) ? "DEGRADED" : status);
        }

        /// <summary>
        ///     Create a Degraded status response. This is useful for when a health check may
        ///     fail but the application itself is still functioning correct. E.g. There could be a
        ///     health check checking the number of messages in a queue, if that number reaches a
        ///     specificied threshold, a degraded status could be returned rather than raising a critical alert.
        /// </summary>
        /// <param name="exception">Exception to use for reason.</param>
        /// <returns>Degraded status response.</returns>
        public static HealthCheckResult Degraded(Exception exception)
        {
            var status = $"EXCEPTION: {exception.GetType().Name} - {exception.Message}";
            return new HealthCheckResult(
                HealthCheckStatus.Degraded,
                status + Environment.NewLine + FormatStackTrace(exception));
        }

        /// <summary>
        ///     Create a Degraded status response. This is useful for when a health check may
        ///     fail but the application itself is still functioning correct. E.g. There could be a
        ///     health check checking the number of messages in a queue, if that number reaches a
        ///     specificied threshold, a degraded status could be returned rather than raising a critical alert.
        /// </summary>
        /// <returns>Degraded status response.</returns>
        public static HealthCheckResult Degraded() { return Degraded("DEGRADED"); }

        /// <summary>
        ///     Create a healthy status response.
        /// </summary>
        /// <returns>Healthy status response.</returns>
        public static HealthCheckResult Healthy() { return Healthy("OK"); }

        /// <summary>
        ///     Create a healthy status response.
        /// </summary>
        /// <param name="message">Status message.</param>
        /// <param name="values">Values to format the status message with.</param>
        /// <returns>Healthy status response.</returns>
        public static HealthCheckResult Healthy(string message, params object[] values)
        {
            var status = string.Format(message, values);
            return new HealthCheckResult(HealthCheckStatus.Healthy, string.IsNullOrWhiteSpace(status) ? "OK" : status);
        }

        /// <summary>
        ///     Ignores this health check.
        /// </summary>
        /// <returns>An ignored health check restul</returns>
        public static HealthCheckResult Ignore() { return new HealthCheckResult(HealthCheckStatus.Ignored, "ignored check"); }

        /// <summary>
        ///     Create a unhealthy status response.
        /// </summary>
        /// <returns>Unhealthy status response.</returns>
        public static HealthCheckResult Unhealthy() { return Unhealthy("FAILED"); }

        /// <summary>
        ///     Create a unhealthy status response.
        /// </summary>
        /// <param name="message">Status message.</param>
        /// <param name="values">Values to format the status message with.</param>
        /// <returns>Unhealthy status response.</returns>
        public static HealthCheckResult Unhealthy(string message, params object[] values)
        {
            var status = string.Format(message, values);
            return new HealthCheckResult(
                HealthCheckStatus.Unhealthy,
                string.IsNullOrWhiteSpace(status) ? "FAILED" : status);
        }

        /// <summary>
        ///     Create a unhealthy status response.
        /// </summary>
        /// <param name="exception">Exception to use for reason.</param>
        /// <returns>Unhealthy status response.</returns>
        public static HealthCheckResult Unhealthy(Exception exception)
        {
            var status = $"EXCEPTION: {exception.GetType().Name} - {exception.Message}";
            return new HealthCheckResult(
                HealthCheckStatus.Unhealthy,
                status + Environment.NewLine + FormatStackTrace(exception));
        }

        private static string FormatStackTrace(Exception exception, int indent = 2)
        {
            var builder = new StringBuilder();

            var aggregate = exception as AggregateException;
            var pad = new string(' ', indent * 2);
            if (aggregate != null)
            {
                builder.AppendFormat("{0}{1}: {2}" + Environment.NewLine, pad, exception.GetType().Name, exception.Message);

                foreach (var inner in aggregate.InnerExceptions)
                {
                    builder.AppendLine(FormatStackTrace(inner, indent + 2));
                }
            }
            else
            {
                builder.AppendFormat("{0}{1}: {2}" + Environment.NewLine, pad, exception.GetType().Name, exception.Message);

                if (exception.StackTrace != null)
                {
                    var stackLines = exception.StackTrace.Split('\n')
                                              .Where(l => !string.IsNullOrWhiteSpace(l))
                                              .Select(l => string.Concat(pad, l.Trim()));

                    builder.AppendLine(string.Join(Environment.NewLine, stackLines));
                }
                else
                {
                    builder.AppendLine(string.Concat(pad, "[No Stacktrace]"));
                }

                if (exception.InnerException != null)
                {
                    builder.AppendLine(FormatStackTrace(exception.InnerException, indent + 2));
                }
            }

            return builder.ToString();
        }
    }
}