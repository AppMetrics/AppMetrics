// <copyright file="DefaultGraphiteUdpClient.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Reporting.Graphite.Client
{
    public class DefaultGraphiteUdpClient : IGraphiteClient
    {
        private static readonly ILog Logger = LogProvider.For<DefaultGraphiteUdpClient>();

        private static TimeSpan _backOffPeriod;
        private static long _backOffTicks;
        private static long _failureAttempts;
        private static long _failuresBeforeBackoff;
        private readonly int _sendTimeout;
        private readonly GraphiteOptions _options;

        public DefaultGraphiteUdpClient(
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
                return new GraphiteWriteResult(false, "Too many failures in writing to Graphite - UDP, Circuit Opened");
            }

            try
            {
                using (var client = new UdpClient { Client = { SendTimeout = _sendTimeout } })
                {
                    Logger.Trace("Opening UDP Connection for Graphite");
                    await client.Client.ConnectAsync(_options.BaseUri.Host, _options.BaseUri.Port).ConfigureAwait(false);

                    var datagram = Encoding.UTF8.GetBytes(payload);

                    await client.Client.SendAsync(new ArraySegment<byte>(datagram), SocketFlags.None).ConfigureAwait(false);

                    Logger.Trace("Successful write to Graphite - UDP");

                    return new GraphiteWriteResult(true);
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _failureAttempts);
                Logger.Error(ex, "Failed to write to Graphite - UDP");
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
