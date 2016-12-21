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
        private const string DefaultBreakerRate = "3 / 00:00:30";
        private static readonly TimeSpan DefaultHttpTimeout = TimeSpan.FromSeconds(3);
        private readonly string _consistenency;
        private readonly string _database;
        private readonly HttpClient _httpClient;
        private readonly Policy _policy;
        private readonly string _retentionPolicy;
        private readonly ILogger<LineProtocolClient> _logger;

        public LineProtocolClient(Uri serverBaseAddress, string database, ILoggerFactory loggerFactory, string username = null, string password = null,
            string retentionPolicy = null, string consistenency = null, string breakerRate = DefaultBreakerRate)
        {
            if (serverBaseAddress == null)
            {
                throw new ArgumentNullException(nameof(serverBaseAddress));
            }

            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentException("A database must be specified");
            }

            if (username.IsPresent() && password.IsPresent())
            {
                var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");

                _httpClient = new HttpClient
                {
                    BaseAddress = serverBaseAddress,
                    Timeout = DefaultHttpTimeout,
                    DefaultRequestHeaders =
                    {
                        Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray))
                    }
                };
            }
            else
            {
                _httpClient = new HttpClient
                {
                    Timeout = DefaultHttpTimeout,
                    BaseAddress = serverBaseAddress
                };
            }


            _policy = new Rate(breakerRate).AsPolicy();
            _database = database;
            _retentionPolicy = retentionPolicy;
            _consistenency = consistenency;
            _logger = loggerFactory.CreateLogger<LineProtocolClient>();
        }

        public async Task<LineProtocolWriteResult> WriteAsync(LineProtocolPayload payload,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var endpoint = $"write?db={Uri.EscapeDataString(_database)}";            

            if (_retentionPolicy.IsPresent())
            {
                endpoint += $"&rp={Uri.EscapeDataString(_retentionPolicy)}";
            }

            if (_consistenency.IsPresent())
            {
                endpoint += $"&consistency={Uri.EscapeDataString(_consistenency)}";
            }            

            var result = await _policy.ExecuteAndCaptureAsync(async () =>
            {
                var payloadText = new StringWriter();
                payload.Format(payloadText);

                var content = new StringContent(payloadText.ToString(), Encoding.UTF8);
                var response = await _httpClient.PostAsync(endpoint, content, cancellationToken).ConfigureAwait(false);

                return response.IsSuccessStatusCode
                    ? new LineProtocolWriteResult(true, null)
                    : new LineProtocolWriteResult(false, $"{response.StatusCode} {response.ReasonPhrase}");
            }, cancellationToken);

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(LoggingEvents.InfluxDbWriteError, result.FinalException, "Failed to write to InfluxDB {error}", result.Result.ErrorMessage);
            }
            else
            {
                _logger.LogDebug("Successful write to InfluxDB");
            }

            return result.Result;
        }
    }
}