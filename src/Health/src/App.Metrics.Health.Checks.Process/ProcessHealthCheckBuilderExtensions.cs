// <copyright file="ProcessHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using static System.Diagnostics.Process;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class ProcessHealthCheckBuilderExtensions
    {
         /// <summary>
        ///     Registers a health check on the process confirming that the current amount of physical memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="healthCheckBuilder">The health check healthCheckBuilder where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The physical memory threshold in bytes.</param>
        /// <param name="degradedOnError">Return a degraded status instead of unhealthy on error.</param>
        /// <returns>The health check healthCheckBuilder instance</returns>
        public static IHealthBuilder AddProcessPhysicalMemoryCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var currentSize = GetCurrentProcess().WorkingSet64;
                        return new ValueTask<HealthCheckResult>(
                            currentSize <= thresholdBytes
                                ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                                : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes}", degradedOnError));
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(ex))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex));
                    }
                });

            return healthCheckBuilder.Builder;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of private memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="healthCheckBuilder">The health check healthCheckBuilder where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The private memory threshold in bytes.</param>
        /// <param name="degradedOnError">Return a degraded status instead of unhealthy on error.</param>
        /// <returns>The health check healthCheckBuilder instance</returns>
        public static IHealthBuilder AddProcessPrivateMemorySizeCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var currentSize = GetCurrentProcess().PrivateMemorySize64;
                        return new ValueTask<HealthCheckResult>(
                            currentSize <= thresholdBytes
                                ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                                : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes} bytes", degradedOnError));
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(ex))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex));
                    }
                });

            return healthCheckBuilder.Builder;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of virtual memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="healthCheckBuilder">The health check healthCheckBuilder where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The virtual memory threshold in bytes.</param>
        /// <param name="degradedOnError">Return a degraded status instead of unhealthy on error.</param>
        /// <returns>The health check healthCheckBuilder instance</returns>
        public static IHealthBuilder AddProcessVirtualMemorySizeCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
                name,
                () =>
                {
                    try
                    {
                        var currentSize = GetCurrentProcess().VirtualMemorySize64;
                        return new ValueTask<HealthCheckResult>(
                            currentSize <= thresholdBytes
                                ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                                : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes} bytes", degradedOnError));
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? new ValueTask<HealthCheckResult>(HealthCheckResult.Degraded(ex))
                            : new ValueTask<HealthCheckResult>(HealthCheckResult.Unhealthy(ex));
                    }
                });

            return healthCheckBuilder.Builder;
        }

        /// <summary>
        ///     Create a failure (degraded or unhealthy) status response.
        /// </summary>
        /// <param name="message">Status message.</param>
        /// <param name="degradedOnError">
        ///     If true, create a degraded status response.
        ///     Otherwise create an unhealthy status response. (default: false)
        /// </param>
        /// <returns>Failure status response.</returns>
        private static HealthCheckResult HealthCheckResultOnError(string message, bool degradedOnError)
        {
            return degradedOnError
                ? HealthCheckResult.Degraded(message)
                : HealthCheckResult.Unhealthy(message);
        }
    }
}
