// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics
{
    // ReSharper restore CheckNamespace
    public static class HealthCheckFactoryExtensions
    {
        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of physical memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="factory">The health check factory where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The physical memory threshold in bytes.</param>
        /// <returns>The health check factory instance</returns>
        public static IHealthCheckFactory RegisterProcessPhysicalMemoryHealthCheck(this IHealthCheckFactory factory, string name, long thresholdBytes)
        {
            factory.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().WorkingSet64;
                    return Task.FromResult(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResult.Unhealthy($"FAILED. {currentSize} > {thresholdBytes}"));
                });

            return factory;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of private memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="factory">The health check factory where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The private memory threshold in bytes.</param>
        /// <returns>The health check factory instance</returns>
        public static IHealthCheckFactory RegisterProcessPrivateMemorySizeHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            long thresholdBytes)
        {
            factory.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().PrivateMemorySize64;
                    return Task.FromResult(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResult.Unhealthy($"FAILED. {currentSize} > {thresholdBytes}"));
                });

            return factory;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of virtual memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="factory">The health check factory where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The virtual memory threshold in bytes.</param>
        /// <returns>The health check factory instance</returns>
        public static IHealthCheckFactory RegisterProcessVirtualMemorySizeHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            long thresholdBytes)
        {
            factory.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().VirtualMemorySize64;
                    return Task.FromResult(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResult.Unhealthy($"FAILED. {currentSize} > {thresholdBytes}"));
                });

            return factory;
        }
    }
}