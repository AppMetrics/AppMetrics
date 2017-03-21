// <copyright file="HealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health;
using App.Metrics.Health.Abstractions;

// ReSharper disable CheckNamespace
namespace App.Metrics
{
    // ReSharper restore CheckNamespace

    public static class HealthCheckFactoryExtensions
    {
        private static readonly HttpClient HttpClient = new HttpClient { DefaultRequestHeaders = { { "cache-control", "no-cache" } } };

        public static IHealthCheckFactory RegisterHttpGetHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            Uri uri,
            TimeSpan timeout,
            CancellationToken token = default(CancellationToken))
        {
            factory.Register(
                name,
                async () =>
                {
                    using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(token))
                    {
                        tokenWithTimeout.CancelAfter(timeout);

                        var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token).ConfigureAwait(false);

                        return response.IsSuccessStatusCode
                            ? HealthCheckResult.Healthy($"OK. {uri}")
                            : HealthCheckResult.Unhealthy($"FAILED. {uri} status code was {response.StatusCode}");
                    }
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterPingHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            string host,
            TimeSpan timeout)
        {
            factory.Register(
                name,
                async () =>
                {
                    var ping = new Ping();
                    var result = await ping.SendPingAsync(host, (int)timeout.TotalMilliseconds).ConfigureAwait(false);

                    return result.Status == IPStatus.Success
                        ? HealthCheckResult.Healthy($"OK. {host}")
                        : HealthCheckResult.Unhealthy($"FAILED. {host} ping result was {result.Status}");
                });

            return factory;
        }

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