// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Internal.Extensions;
using App.Metrics.Scheduling.Interfaces;

namespace App.Metrics.Scheduling
{
    public sealed class DefaultTaskScheduler : IScheduler
    {
        private bool _disposed;
        private CancellationTokenSource _token;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.

                    if (_token != null)
                    {
                        _token.Cancel();
                        _token.Dispose();
                    }
                }
            }

            _disposed = true;
        }

        public Task Interval(TimeSpan pollInterval, Action action)
        {
            _token = new CancellationTokenSource();

            return Task.Factory.StartNew(() =>
            {
                for (;;)
                {
                    if (_token.Token.WaitCancellationRequested(pollInterval))
                        break;

                    action();
                }
            }, _token.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public Task Interval(TimeSpan pollInterval, Action action, CancellationToken token)
        {
            _token = CancellationTokenSource.CreateLinkedTokenSource(token);

            return Task.Factory.StartNew(() =>
            {
                for (;;)
                {
                    if (token.WaitCancellationRequested(pollInterval))
                        break;

                    action();
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Stop()
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(DefaultTaskScheduler));
            }

            _token?.Cancel();
        }

        private bool CheckDisposed() => _disposed;
    }
}