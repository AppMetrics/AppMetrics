// <copyright file="DefaultDatadogHttpClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Datadog.Client
{
    public class DefaultDatadogHttpClient : IDatadogClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultDatadogHttpClient>();

        private static TimeSpan _backOffPeriod;
        private static long _backOffTicks;
        private static long _failureAttempts;
        private static long _failuresBeforeBackoff;
        private readonly HttpClient _client;
        private readonly DatadogOptions _options;

        public DefaultDatadogHttpClient(
            HttpClient client,
            DatadogOptions options,
            HttpPolicy httpPolicy)
        {
            if (httpPolicy == null)
            {
                throw new ArgumentNullException(nameof(httpPolicy));
            }

            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _backOffPeriod = httpPolicy.BackoffPeriod;
            _failuresBeforeBackoff = httpPolicy.FailuresBeforeBackoff;
            _failureAttempts = 0;
        }

        /// <inheritdoc />
        public async Task<DatadogWriteResult> WriteAsync(string payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                return DatadogWriteResult.SuccessResult;
            }

            if (NeedToBackoff())
            {
                return new DatadogWriteResult(false, "Too many failures in writing to Datadog, Circuit Opened - HTTP");
            }

            try
            {
                var content = new StringContent(payload);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var response = await _client.PostAsync($"/api/v1/series?api_key={_options.ApiKey}", content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage = $"Failed to write to Datadog - StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                    Logger.Error(errorMessage);

                    return new DatadogWriteResult(false, errorMessage);
                }

                Logger.Trace("Successful write to Datadog");

                return new DatadogWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, "Failed to write to Datadog");
                return new DatadogWriteResult(false, ex.ToString());
            }
        }

        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _failuresBeforeBackoff)
            {
                return false;
            }

            if (Interlocked.Read(ref _backOffTicks) == 0)
            {
                Interlocked.Exchange(ref _backOffTicks, DateTime.UtcNow.Add(_backOffPeriod).Ticks);
            }

            if (DateTime.UtcNow.Ticks <= Interlocked.Read(ref _backOffTicks))
            {
                return true;
            }

            Interlocked.Exchange(ref _failureAttempts, 0);
            Interlocked.Exchange(ref _backOffTicks, 0);

            return false;
        }
    }
}
