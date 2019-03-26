// <copyright file="PingHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class PingHealthCheckBuilderExtensions
    {
        public static IHealthBuilder AddPingCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string host,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCheck(
                name,
                async () => await ExecutePingCheckAsync(host, timeout, degradedOnError));

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddPingCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string host,
            TimeSpan timeout,
            TimeSpan cacheDuration,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddCachedCheck(
                name,
                async () => await ExecutePingCheckAsync(host, timeout, degradedOnError),
                cacheDuration);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddPingCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            string host,
            TimeSpan timeout,
            HealthCheck.QuiteTime quiteTime,
            bool degradedOnError = false)
        {
            healthCheckBuilder.AddQuiteTimeCheck(
                name,
                async () => await ExecutePingCheckAsync(host, timeout, degradedOnError),
                quiteTime);

            return healthCheckBuilder.Builder;
        }

        private static async Task<HealthCheckResult> ExecutePingCheckAsync(string host, TimeSpan timeout, bool degradedOnError)
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
