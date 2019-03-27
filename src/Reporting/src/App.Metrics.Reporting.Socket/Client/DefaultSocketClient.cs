// <copyright file="DefaultSocketClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Socket.Client
{
    public class DefaultSocketClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultSocketClient>();

        private static long _backoffTicks;
        private static long _failureAttempts;
        private readonly SocketClient _socketClient;
        private readonly SocketPolicy _socketPolicy;

        public string Endpoint
        {
            get
            {
                return _socketClient.Endpoint;
            }
        }

        public DefaultSocketClient(MetricsReportingSocketOptions options)
        {
            _socketClient = CreateSocketClient(options.SocketSettings);
            _socketPolicy = options.SocketPolicy;
            _failureAttempts = 0;
        }

        public async Task<SocketWriteResult> WriteAsync(
            string payload,
            CancellationToken cancellationToken = default)
        {
            if (NeedToBackoff())
            {
                return new SocketWriteResult(false, $"Too many failures when attempting to write to {Endpoint}, circuit is open");
            }

            if (!_socketClient.IsConnected())
            {
                Logger.Debug($"Trying to connect to {Endpoint}");
                await _socketClient.Reconnect();
            }

            try
            {
                var response = await _socketClient.WriteAsync(payload, cancellationToken);

                if (!response.Success)
                {
                    Interlocked.Increment(ref _failureAttempts);

                    var errorMessage =
                        $"Failed to write {payload.Length} bytes to {Endpoint} ({response.ErrorMessage})";
                    Logger.Error(errorMessage);

                    return new SocketWriteResult(false, errorMessage);
                }

                Logger.Trace($"Successful write to {Endpoint}");

                return new SocketWriteResult(true);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, $"Failed to write to {Endpoint}");
                return new SocketWriteResult(false, ex.ToString());
            }
        }

        private static SocketClient CreateSocketClient(
            SocketSettings socketSettings)
        {
            var client = new SocketClient(socketSettings);
            return client;
        }

        private bool NeedToBackoff()
        {
            if (Interlocked.Read(ref _failureAttempts) < _socketPolicy.FailuresBeforeBackoff)
            {
                return false;
            }

            Logger.Error($"{Endpoint} write backoff for {_socketPolicy.BackoffPeriod.Seconds} seconds");

            if (Interlocked.Read(ref _backoffTicks) == 0)
            {
                Interlocked.Exchange(ref _backoffTicks, DateTime.UtcNow.Add(_socketPolicy.BackoffPeriod).Ticks);
            }

            if (DateTime.UtcNow.Ticks <= Interlocked.Read(ref _backoffTicks))
            {
                return true;
            }

            Interlocked.Exchange(ref _failureAttempts, 0);
            Interlocked.Exchange(ref _backoffTicks, 0);

            return false;
        }
    }
}
