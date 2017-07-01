// <copyright file="HealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class HealthCheckFactoryExtensions
    {
        private static readonly HttpClient HttpClient = new HttpClient { DefaultRequestHeaders = { { "cache-control", "no-cache" } } };

        public static IHealthCheckRegistry AddHttpGetCheck(
            this IHealthCheckRegistry registry,
            string name,
            Uri uri,
            TimeSpan timeout)
        {
            registry.Register(
                name,
                async cancellationToken =>
                {
                    using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                    {
                        tokenWithTimeout.CancelAfter(timeout);

                        var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token).ConfigureAwait(false);

                        return response.IsSuccessStatusCode
                            ? HealthCheckResult.Healthy($"OK. {uri}")
                            : HealthCheckResult.Unhealthy($"FAILED. {uri} status code was {response.StatusCode}");
                    }
                });

            return registry;
        }

        public static IHealthCheckRegistry AddPingCheck(
            this IHealthCheckRegistry registry,
            string name,
            string host,
            TimeSpan timeout)
        {
            registry.Register(
                name,
                async () =>
                {
                    var ping = new Ping();
                    var result = await ping.SendPingAsync(host, (int)timeout.TotalMilliseconds).ConfigureAwait(false);

                    return result.Status == IPStatus.Success
                        ? HealthCheckResult.Healthy($"OK. {host}")
                        : HealthCheckResult.Unhealthy($"FAILED. {host} ping result was {result.Status}");
                });

            return registry;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of physical memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="registry">The health check registry where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The physical memory threshold in bytes.</param>
        /// <returns>The health check registry instance</returns>
        public static IHealthCheckRegistry AddProcessPhysicalMemoryCheck(this IHealthCheckRegistry registry, string name, long thresholdBytes)
        {
            registry.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().WorkingSet64;
                    return new ValueTask<HealthCheckResult>(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResult.Unhealthy($"FAILED. {currentSize} > {thresholdBytes}"));
                });

            return registry;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of private memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="registry">The health check registry where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The private memory threshold in bytes.</param>
        /// <returns>The health check registry instance</returns>
        public static IHealthCheckRegistry AddProcessPrivateMemorySizeCheck(
            this IHealthCheckRegistry registry,
            string name,
            long thresholdBytes)
        {
            registry.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().PrivateMemorySize64;
                    return new ValueTask<HealthCheckResult>(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResult.Unhealthy($"FAILED. {currentSize} > {thresholdBytes} bytes"));
                });

            return registry;
        }

        /// <summary>
        ///     Registers a health check on the process confirming that the current amount of virtual memory is below the
        ///     threshold.
        /// </summary>
        /// <param name="registry">The health check registry where the health check is registered.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="thresholdBytes">The virtual memory threshold in bytes.</param>
        /// <returns>The health check registry instance</returns>
        public static IHealthCheckRegistry AddProcessVirtualMemorySizeCheck(
            this IHealthCheckRegistry registry,
            string name,
            long thresholdBytes)
        {
            registry.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().VirtualMemorySize64;
                    return new ValueTask<HealthCheckResult>(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResult.Unhealthy($"FAILED. {currentSize} > {thresholdBytes} bytes"));
                });

            return registry;
        }
    }
}