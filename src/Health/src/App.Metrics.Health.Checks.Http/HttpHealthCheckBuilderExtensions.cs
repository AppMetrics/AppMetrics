// <copyright file="HttpHealthCheckBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Health.Logging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Health
    // ReSharper restore CheckNamespace
{
    public static class HttpHealthCheckBuilderExtensions
    {
        private static readonly HttpClient HttpClient = new HttpClient { DefaultRequestHeaders = { { "cache-control", "no-cache" } } };
        private static readonly ILog Logger = LogProvider.For<IRunHealthChecks>();

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            TimeSpan timeout,
            TimeSpan cacheDuration,
            bool degradedOnError = false)
        {
            EnsureValidTimeout(timeout);

            healthCheckBuilder.AddCachedCheck(
                name,
                async cancellationToken => await ExecuteHttpCheckNoRetriesAsync(uri, timeout, degradedOnError, cancellationToken),
                cacheDuration);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            TimeSpan timeout,
            HealthCheck.QuiteTime quiteTime,
            bool degradedOnError = false)
        {
            EnsureValidTimeout(timeout);

            healthCheckBuilder.AddQuiteTimeCheck(
                name,
                async cancellationToken => await ExecuteHttpCheckNoRetriesAsync(uri, timeout, degradedOnError, cancellationToken),
                quiteTime);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            int retries,
            TimeSpan delayBetweenRetries,
            TimeSpan timeoutPerRequest,
            TimeSpan cacheDuration,
            bool degradedOnError = false)
        {
            EnsureValidRetries(retries);
            EnsureValidDelayBetweenRequests(delayBetweenRetries);
            EnsureValidTimeoutPerRequest(timeoutPerRequest);

            healthCheckBuilder.AddCachedCheck(
                name,
                async cancellationToken => await ExecuteHealthCheckWithRetriesAsync(
                    uri,
                    retries,
                    delayBetweenRetries,
                    timeoutPerRequest,
                    degradedOnError,
                    cancellationToken),
                cacheDuration);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            int retries,
            TimeSpan delayBetweenRetries,
            TimeSpan timeoutPerRequest,
            HealthCheck.QuiteTime quiteTime,
            bool degradedOnError = false)
        {
            EnsureValidRetries(retries);
            EnsureValidDelayBetweenRequests(delayBetweenRetries);
            EnsureValidTimeoutPerRequest(timeoutPerRequest);

            healthCheckBuilder.AddQuiteTimeCheck(
                name,
                async cancellationToken => await ExecuteHealthCheckWithRetriesAsync(
                    uri,
                    retries,
                    delayBetweenRetries,
                    timeoutPerRequest,
                    degradedOnError,
                    cancellationToken),
                quiteTime);

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            TimeSpan timeout,
            bool degradedOnError = false)
        {
            EnsureValidTimeout(timeout);

            healthCheckBuilder.AddCheck(
                name,
                async cancellationToken => await ExecuteHttpCheckNoRetriesAsync(uri, timeout, degradedOnError, cancellationToken));

            return healthCheckBuilder.Builder;
        }

        public static IHealthBuilder AddHttpGetCheck(
            this IHealthCheckBuilder healthCheckBuilder,
            string name,
            Uri uri,
            int retries,
            TimeSpan delayBetweenRetries,
            TimeSpan timeoutPerRequest,
            bool degradedOnError = false)
        {
            EnsureValidRetries(retries);
            EnsureValidDelayBetweenRequests(delayBetweenRetries);
            EnsureValidTimeoutPerRequest(timeoutPerRequest);

            healthCheckBuilder.AddCheck(
                name,
                async cancellationToken => await ExecuteHealthCheckWithRetriesAsync(
                    uri,
                    retries,
                    delayBetweenRetries,
                    timeoutPerRequest,
                    degradedOnError,
                    cancellationToken));

            return healthCheckBuilder.Builder;
        }

        private static void EnsureValidDelayBetweenRequests(TimeSpan delayBetweenRetries)
        {
            if (delayBetweenRetries <= TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(delayBetweenRetries)} must be greater than 0");
            }
        }

        private static void EnsureValidRetries(int retries)
        {
            if (retries <= 0)
            {
                throw new InvalidOperationException($"{nameof(retries)} must be greater than 0");
            }
        }

        private static void EnsureValidTimeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(timeout)} must be greater than 0");
            }
        }

        private static void EnsureValidTimeoutPerRequest(TimeSpan timeoutPerRequest)
        {
            if (timeoutPerRequest <= TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(timeoutPerRequest)} must be greater than 0");
            }
        }

        private static async Task<HealthCheckResult> ExecuteHealthCheckWithRetriesAsync(
            Uri uri,
            int retries,
            TimeSpan delayBetweenRetries,
            TimeSpan timeoutPerRequest,
            bool degradedOnError,
            CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            var attempts = 0;
            try
            {
                sw.Start();

                do
                {
                    try
                    {
                        attempts++;

                        using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                        {
                            tokenWithTimeout.CancelAfter(timeoutPerRequest);

                            var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token);

                            if (attempts == retries || response.IsSuccessStatusCode)
                            {
                                return response.IsSuccessStatusCode
                                    ? HealthCheckResult.Healthy(
                                        $"OK. '{uri}' success. Total Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.")
                                    : HealthCheckResultOnError(
                                        $"FAILED. '{uri}' status code was {response.StatusCode}. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                                        degradedOnError);
                            }

                            if (response.StatusCode == HttpStatusCode.GatewayTimeout ||
                                response.StatusCode == HttpStatusCode.ServiceUnavailable)
                            {
                                Logger.Error(
                                    $"HTTP Health Check '{uri}' failed with status code {response.StatusCode}. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.");

                                Logger.Info(
                                    $"Retrying HTTP Health Check '{uri}' in {delayBetweenRetries.TotalMilliseconds}ms. {attempts} / {retries} retries.");

                                await Task.Delay(delayBetweenRetries, cancellationToken);
                            }
                        }
                    }
                    catch (Exception ex) when (ex is TaskCanceledException)
                    {
                        Logger.ErrorException(
                            $"HTTP Health Check '{uri}' did not respond within '{timeoutPerRequest.TotalMilliseconds}'ms. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                            ex);

                        if (attempts == retries)
                        {
                            return HealthCheckResultOnError(
                                $"FAILED. '{uri}' did not respond within {timeoutPerRequest.TotalMilliseconds}ms. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                                degradedOnError);
                        }

                        await Retry(uri, retries, delayBetweenRetries, attempts, ex, cancellationToken);
                    }
                    catch (Exception ex) when (ex is TimeoutException)
                    {
                        Logger.ErrorException(
                            $"HTTP Health Check '{uri}' timed out. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                            ex);

                        if (attempts == retries)
                        {
                            return HealthCheckResultOnError(
                                $"FAILED. '{uri}' timed out. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                                degradedOnError);
                        }

                        await Retry(uri, retries, delayBetweenRetries, attempts, ex, cancellationToken);
                    }
                    catch (Exception ex) when (ex is HttpRequestException)
                    {
                        Logger.ErrorException(
                            $"HTTP Health Check '{uri}' failed. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                            ex);

                        if (attempts == retries)
                        {
                            return HealthCheckResultOnError(
                                $"FAILED. '{uri}' request failed with an unexpected error. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.",
                                degradedOnError);
                        }

                        await Retry(uri, retries, delayBetweenRetries, attempts, ex, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        var message =
                            $"HTTP Health Check failed to request '{uri}'. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.";

                        if (attempts == retries)
                        {
                            Logger.ErrorException(message, ex);

                            return HealthCheckResultOnError(
                                $"FAILED. {message}.",
                                degradedOnError);
                        }

                        await Retry(uri, retries, delayBetweenRetries, attempts, ex, cancellationToken);
                    }
                }
                while (true);
            }
            catch (Exception ex) when (ex is TaskCanceledException)
            {
                Logger.ErrorException(
                    $"HTTP Health Check '{uri}' did not respond within '{timeoutPerRequest.TotalMilliseconds}'ms. Attempts: {attempts}.",
                    ex);

                return HealthCheckResultOnError(
                    $"FAILED. '{uri}' did not respond within {timeoutPerRequest.TotalMilliseconds}ms. Attempts: {attempts}.",
                    degradedOnError);
            }
            catch (Exception ex)
            {
                var message = $"HTTP Health Check failed to request '{uri}'. Time taken: {sw.ElapsedMilliseconds}ms. Attempts: {attempts}.";

                Logger.ErrorException(message, ex);

                return HealthCheckResultOnError(
                    $"FAILED. {message}",
                    degradedOnError);
            }
            finally
            {
                sw.Stop();
            }
        }

        private static async Task<HealthCheckResult> ExecuteHttpCheckNoRetriesAsync(
            Uri uri,
            TimeSpan timeout,
            bool degradedOnError,
            CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();

            try
            {
                using (var tokenWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    tokenWithTimeout.CancelAfter(timeout);

                    sw.Start();

                    var response = await HttpClient.GetAsync(uri, tokenWithTimeout.Token).ConfigureAwait(false);

                    return response.IsSuccessStatusCode
                        ? HealthCheckResult.Healthy($"OK. '{uri}' success. Time taken: {sw.ElapsedMilliseconds}ms.")
                        : HealthCheckResultOnError(
                            $"FAILED. '{uri}' status code was {response.StatusCode}. Time taken: {sw.ElapsedMilliseconds}ms.",
                            degradedOnError);
                }
            }
            catch (Exception ex) when (ex is TaskCanceledException)
            {
                Logger.ErrorException($"HTTP Health Check '{uri}' did not respond within '{timeout.TotalMilliseconds}'ms.", ex);

                return HealthCheckResultOnError($"FAILED. '{uri}' did not respond within {timeout.TotalMilliseconds}ms", degradedOnError);
            }
            catch (Exception ex) when (ex is TimeoutException)
            {
                Logger.ErrorException($"HTTP Health Check '{uri}' timed out. Time taken: {sw.ElapsedMilliseconds}ms.", ex);

                return HealthCheckResultOnError($"FAILED. '{uri}' timed out. Time taken: {sw.ElapsedMilliseconds}ms.", degradedOnError);
            }
            catch (Exception ex) when (ex is HttpRequestException)
            {
                Logger.ErrorException($"HTTP Health Check '{uri}' failed. Time taken: {sw.ElapsedMilliseconds}ms.", ex);

                return HealthCheckResultOnError(
                    $"FAILED. '{uri}' request failed with an unexpected error. Time taken: {sw.ElapsedMilliseconds}ms.",
                    degradedOnError);
            }
            catch (Exception ex)
            {
                var message = $"HTTP Health Check failed to request '{uri}'. Time taken: {sw.ElapsedMilliseconds}ms.";

                Logger.ErrorException(message, ex);

                return HealthCheckResultOnError($"FAILED. {message}", degradedOnError);
            }
            finally
            {
                sw.Stop();
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

        private static async Task Retry(
            Uri uri,
            int retries,
            TimeSpan delayBetweenRetries,
            int attempts,
            Exception ex,
            CancellationToken cancellationToken)
        {
            Logger.InfoException(
                $"Retrying HTTP Health Check '{uri}' in {delayBetweenRetries.TotalMilliseconds}ms. {attempts + 1} / {retries} retries.",
                ex);

            await Task.Delay(delayBetweenRetries, cancellationToken);
        }
    }
}