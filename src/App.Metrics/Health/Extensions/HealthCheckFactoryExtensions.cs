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
            CancellationToken token = default(CancellationToken),
            bool degradedOnError = false)
        {
            factory.Register(
                name,
                async () =>
                {
                    try
                    {
                        using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(token))
                        {
                            tokenWithTimeout.CancelAfter(timeout);

                            var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token).ConfigureAwait(false);

                            return response.IsSuccessStatusCode
                                ? HealthCheckResult.Healthy($"OK. {uri}")
                                : HealthCheckResultOnError($"FAILED. {uri} status code was {response.StatusCode}", degradedOnError);
                        }
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? HealthCheckResult.Degraded(ex)
                            : HealthCheckResult.Unhealthy(ex);
                    }
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterPingHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            string host,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            factory.Register(
                name,
                async () =>
                {
                    try
                    {
                        var ping = new Ping();
                        var result = await ping.SendPingAsync(host, (int)timeout.TotalMilliseconds).ConfigureAwait(false);

                        return result.Status == IPStatus.Success
                            ? HealthCheckResult.Healthy($"OK. {host}")
                            : HealthCheckResultOnError($"FAILED. {host} ping result was {result.Status}", degradedOnError);
                    }
                    catch (Exception ex)
                    {
                        return degradedOnError
                            ? HealthCheckResult.Degraded(ex)
                            : HealthCheckResult.Unhealthy(ex);
                    }
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
        /// <param name="degradedOnError">
        ///     WHen the check fails, if true, create a degraded status response.
        ///     Otherwise create an unhealthy status response. (default: false)
        /// </param>
        /// <returns>The health check factory instance</returns>
        public static IHealthCheckFactory RegisterProcessPhysicalMemoryHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            factory.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().WorkingSet64;
                    return Task.FromResult(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes}", degradedOnError));
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
        /// <param name="degradedOnError">
        ///     WHen the check fails, if true, create a degraded status response.
        ///     Otherwise create an unhealthy status response. (default: false)
        /// </param>
        /// <returns>The health check factory instance</returns>
        public static IHealthCheckFactory RegisterProcessPrivateMemorySizeHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            factory.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().PrivateMemorySize64;
                    return Task.FromResult(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes} bytes", degradedOnError));
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
        /// <param name="degradedOnError">
        ///     WHen the check fails, if true, create a degraded status response.
        ///     Otherwise create an unhealthy status response. (default: false)
        /// </param>
        /// <returns>The health check factory instance</returns>
        public static IHealthCheckFactory RegisterProcessVirtualMemorySizeHealthCheck(
            this IHealthCheckFactory factory,
            string name,
            long thresholdBytes,
            bool degradedOnError = false)
        {
            factory.Register(
                name,
                () =>
                {
                    var currentSize = Process.GetCurrentProcess().VirtualMemorySize64;
                    return Task.FromResult(
                        currentSize <= thresholdBytes
                            ? HealthCheckResult.Healthy($"OK. {thresholdBytes} bytes")
                            : HealthCheckResultOnError($"FAILED. {currentSize} > {thresholdBytes} bytes", degradedOnError));
                });

            return factory;
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