// <copyright file="DefaultLineProtocolClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;
using App.Metrics.Reporting.InfluxDB.Client;

namespace App.Metrics.Reporting.InfluxDB2.Client
{
    public class InfluxDb2ProtocolClient : ILineProtocolClient
    {
        private static readonly ILog Logger = LogProvider.For<InfluxDb2ProtocolClient>();

        private static long _backOffTicks;
        private static long _failureAttempts;
        private static long _failuresBeforeBackoff;
        private static TimeSpan _backOffPeriod;

        private readonly HttpClient _httpClient;
        private readonly InfluxDb2Options _influxDbOptions;

        public InfluxDb2ProtocolClient(
            InfluxDb2Options influxDbOptions,
            HttpPolicy httpPolicy,
            HttpClient httpClient)
        {
            _influxDbOptions = influxDbOptions ?? throw new ArgumentNullException(nameof(influxDbOptions));
            _httpClient = httpClient;
            _backOffPeriod = httpPolicy?.BackoffPeriod ?? throw new ArgumentNullException(nameof(httpPolicy));
            _failuresBeforeBackoff = httpPolicy.FailuresBeforeBackoff;
            _failureAttempts = 0;
        }

        public async Task<LineProtocolWriteResult> WriteAsync(
            Stream payload,
            CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                return new LineProtocolWriteResult(true);
            }

            if (NeedToBackoff())
            {
                return new LineProtocolWriteResult(false, "Too many failures in writing to InfluxDB, Circuit Opened");
            }

            try
            {
                var content = new StreamContent(payload);

                var response = await _httpClient.PostAsync(_influxDbOptions.Endpoint, content, cancellationToken);


                if (!response.IsSuccessStatusCode)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage = $"Failed to write to InfluxDB - StatusCode: {response.StatusCode} Reason: {response.ReasonPhrase}";
                    Logger.Error(errorMessage);

                    return new LineProtocolWriteResult(false, errorMessage);
                }

                Interlocked.Exchange(ref _failureAttempts, 0);

                Logger.Trace("Successful write to InfluxDB");

                return new LineProtocolWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, "Failed to write to InfluxDB");
                return new LineProtocolWriteResult(false, ex.ToString());
            }
        }


        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _failuresBeforeBackoff)
            {
                return false;
            }

            Logger.Error($"InfluxDB write backoff for {_backOffPeriod.Seconds} secs");

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
