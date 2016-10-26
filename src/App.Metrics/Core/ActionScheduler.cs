// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy


using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Core
{
    /// <summary>
    ///     Utility class to schedule an Action to be executed repeatedly according to the interval.
    /// </summary>
    /// <remarks>
    ///     The scheduling code is inspired form Daniel Crenna's metrics port
    ///     https://github.com/danielcrenna/metrics-net/blob/master/src/metrics/Reporting/ReporterBase.cs
    /// </remarks>
    public sealed class ActionScheduler : IScheduler
    {
        private CancellationTokenSource _token;

        public void Dispose()
        {
            if (_token == null) return;

            _token.Cancel();
            _token.Dispose();
        }

        public void Start(TimeSpan interval, Action action)
        {
            Start(interval, t =>
            {
                if (!t.IsCancellationRequested)
                {
                    action();
                }
            });
        }

        public void Start(TimeSpan interval, Action<CancellationToken> action)
        {
            Start(interval, t =>
            {
                action(t);
                return Task.FromResult(true);
            });
        }

        public void Start(TimeSpan interval, Func<Task> action)
        {
            Start(interval, t => t.IsCancellationRequested ? action() : Task.FromResult(true));
        }

        public void Start(TimeSpan interval, Func<CancellationToken, Task> action)
        {
            if (interval.TotalSeconds == 0)
            {
                throw new ArgumentException("interval must be > 0 seconds", nameof(interval));
            }

            if (_token != null)
            {
                throw new InvalidOperationException("Scheduler is already started.");
            }

            _token = new CancellationTokenSource();

            RunScheduler(interval, action, _token);
        }

        public void Stop()
        {
            _token?.Cancel();
        }

        private void RunScheduler(TimeSpan interval, Func<CancellationToken, Task> action, CancellationTokenSource token)
        {
            Task.Factory.StartNew(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(interval, token.Token).ConfigureAwait(false);
                        try
                        {
                            await action(token.Token).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            //TODO: Review enableing internal metrics
                            //MetricsErrorHandler.Handle(x, "Error while executing action scheduler.");
                            token.Cancel();
                        }
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }, token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }
    }
}