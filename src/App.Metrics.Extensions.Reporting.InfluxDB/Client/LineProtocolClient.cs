// Copyright (c) Allan hardy. All rights reserved.
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
    public class LineProtocolClient
    {
        private readonly HttpClient _httpClient;
        private readonly InfluxDBSettings _influxDbSettings;
        private readonly ILogger<LineProtocolClient> _logger;
        private readonly Policy _policy;

        public LineProtocolClient(ILoggerFactory loggerFactory, InfluxDBSettings influxDbSettings)
            : this(
                loggerFactory, influxDbSettings,
                new HttpPolicy { FailuresBeforeBackoff = 3, BackoffPeriod = TimeSpan.FromSeconds(30), Timeout = TimeSpan.FromSeconds(3) })
        {
        }

        public LineProtocolClient(ILoggerFactory loggerFactory, InfluxDBSettings influxDbSettings, HttpPolicy httpPolicy)
        {
            if (influxDbSettings == null)
            {
                throw new ArgumentNullException(nameof(influxDbSettings));
            }

            if (httpPolicy == null)
            {
                throw new ArgumentNullException(nameof(httpPolicy));
            }

            if (influxDbSettings.Database.IsMissing())
            {
                throw new ArgumentException("A database must be specified");
            }

            _httpClient = CreateHttpClient(influxDbSettings, httpPolicy);
            _influxDbSettings = influxDbSettings;
            _policy = httpPolicy.AsPolicy();
            _logger = loggerFactory.CreateLogger<LineProtocolClient>();
        }

        public async Task<LineProtocolWriteResult> WriteAsync(LineProtocolPayload payload,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _policy.ExecuteAndCaptureAsync(async () =>
            {
                var payloadText = new StringWriter();
                payload.Format(payloadText);

                var content = new StringContent(payloadText.ToString(), Encoding.UTF8);
                var response = await _httpClient.PostAsync(_influxDbSettings.Endpoint, content, cancellationToken).ConfigureAwait(false);

                return response.IsSuccessStatusCode
                    ? new LineProtocolWriteResult(true, null)
                    : new LineProtocolWriteResult(false, $"{response.StatusCode} {response.ReasonPhrase}");
            }, cancellationToken);

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(LoggingEvents.InfluxDbWriteError, result.FinalException, "Failed to write to InfluxDB {error}",
                    result.Result.ErrorMessage);
            }
            else
            {
                _logger.LogDebug("Successful write to InfluxDB");
            }

            return result.Result;
        }

        private static HttpClient CreateHttpClient(InfluxDBSettings influxDbSettings, HttpPolicy httpPolicy)
        {
            if (influxDbSettings.UserName.IsPresent() && influxDbSettings.Password.IsPresent())
            {
                var byteArray = Encoding.ASCII.GetBytes($"{influxDbSettings.UserName}:{influxDbSettings.Password}");

                return new HttpClient
                {
                    BaseAddress = influxDbSettings.BaseAddress,
                    Timeout = httpPolicy.Timeout,
                    DefaultRequestHeaders =
                    {
                        Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))
                    }
                };
            }

            return new HttpClient
            {
                Timeout = httpPolicy.Timeout,
                BaseAddress = influxDbSettings.BaseAddress
            };
        }
    }
}