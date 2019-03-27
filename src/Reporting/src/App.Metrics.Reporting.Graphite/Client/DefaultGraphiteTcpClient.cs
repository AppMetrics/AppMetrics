// <copyright file="DefaultGraphiteTcpClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Graphite.Client
{
    public class DefaultGraphiteTcpClient : IGraphiteClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultGraphiteTcpClient>();

        private static TimeSpan _backOffPeriod;
        private static long _backOffTicks;
        private static long _failureAttempts;
        private static long _failuresBeforeBackoff;
        private readonly int _sendTimeout;
        private readonly GraphiteOptions _options;

        public DefaultGraphiteTcpClient(
            GraphiteOptions options,
            ClientPolicy clientPolicy)
        {
            _options = options;
            _backOffPeriod = clientPolicy.BackoffPeriod;
            _failuresBeforeBackoff = clientPolicy.FailuresBeforeBackoff;
            _sendTimeout = clientPolicy.Timeout.Milliseconds;
            _failureAttempts = 0;
        }

        /// <inheritdoc />
        public async Task<GraphiteWriteResult> WriteAsync(string payload, CancellationToken cancellationToken = default)
        {
            if (payload == null)
            {
                return GraphiteWriteResult.SuccessResult;
            }

            if (NeedToBackoff())
            {
                return new GraphiteWriteResult(false, "Too many failures in writing to Graphite, Circuit Opened - TCP");
            }

            try
            {
                using (var client = new TcpClient { SendTimeout = _sendTimeout })
                {
                    Logger.Trace("Opening TCP Connection for Graphite");
                    await client.ConnectAsync(_options.BaseUri.Host, _options.BaseUri.Port).ConfigureAwait(false);

                    using (var stream = client.GetStream())
                    {
                        using (var writer = new StreamWriter(stream) { NewLine = "\n" })
                        {
                            await writer.WriteLineAsync(payload).ConfigureAwait(false);
                            await writer.FlushAsync().ConfigureAwait(false);
                        }

                        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);

                        Logger.Trace("Successful write to Graphite - TCP");

                        return new GraphiteWriteResult(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, "Failed to write to Graphite - TCP");
                return new GraphiteWriteResult(false, ex.ToString());
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
