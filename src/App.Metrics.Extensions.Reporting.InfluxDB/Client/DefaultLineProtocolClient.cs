// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    public class DefaultLineProtocolClient : ILineProtocolClient
    {
        private readonly HttpClient _httpClient;
        private readonly InfluxDBSettings _influxDbSettings;
        private readonly ILogger<DefaultLineProtocolClient> _logger;
        private readonly Policy _policy;

        public DefaultLineProtocolClient(ILoggerFactory loggerFactory, InfluxDBSettings influxDbSettings)
            : this(
                loggerFactory,
                influxDbSettings,
#pragma warning disable SA1118
                new HttpPolicy
                {
                    FailuresBeforeBackoff = Constants.DefaultFailuresBeforeBackoff,
                    BackoffPeriod = Constants.DefaultBackoffPeriod,
                    Timeout = Constants.DefaultTimeout
                }) { }
#pragma warning disable SA1118

        public DefaultLineProtocolClient(
            ILoggerFactory loggerFactory,
            InfluxDBSettings influxDbSettings,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            if (influxDbSettings == null)
            {
                throw new ArgumentNullException(nameof(influxDbSettings));
            }

            if (httpPolicy == null)
            {
                throw new ArgumentNullException(nameof(httpPolicy));
            }

            _httpClient = CreateHttpClient(influxDbSettings, httpPolicy, httpMessageHandler);
            _influxDbSettings = influxDbSettings;
            _policy = httpPolicy.AsPolicy();
            _logger = loggerFactory.CreateLogger<DefaultLineProtocolClient>();
        }

        public async Task<LineProtocolWriteResult> WriteAsync(
            LineProtocolPayload payload,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _policy.ExecuteAndCaptureAsync(
                async () =>
                {
                    var payloadText = new StringWriter();
                    payload.Format(payloadText);

                    var content = new StringContent(payloadText.ToString(), Encoding.UTF8);
                    var response = await _httpClient.PostAsync(_influxDbSettings.Endpoint, content, cancellationToken).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    return new LineProtocolWriteResult(true, null);
                },
                cancellationToken);

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(LoggingEvents.InfluxDbWriteError, result.FinalException, "Failed to write to InfluxDB");

                return new LineProtocolWriteResult(false, result.FinalException.ToString());
            }

            _logger.LogDebug("Successful write to InfluxDB");

            return result.Result;
        }

        private static HttpClient CreateHttpClient(
            InfluxDBSettings influxDbSettings,
            HttpPolicy httpPolicy,
            HttpMessageHandler httpMessageHandler = null)
        {
            var client = httpMessageHandler == null
                ? new HttpClient()
                : new HttpClient(httpMessageHandler);

            client.BaseAddress = influxDbSettings.BaseAddress;
            client.Timeout = httpPolicy.Timeout;

            if (influxDbSettings.UserName.IsMissing() || influxDbSettings.Password.IsMissing())
            {
                return client;
            }

            var byteArray = Encoding.ASCII.GetBytes($"{influxDbSettings.UserName}:{influxDbSettings.Password}");
            client.BaseAddress = influxDbSettings.BaseAddress;
            client.Timeout = httpPolicy.Timeout;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            return client;
        }
    }
}