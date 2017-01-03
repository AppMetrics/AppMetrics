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
        private Task _task;
        private CancellationTokenSource _token;

        public DefaultTaskScheduler()
        {
            _token = new CancellationTokenSource();
        }

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

        // <inheritdoc />
        public Task Interval(TimeSpan pollInterval, TaskCreationOptions taskCreationOptions, Action action)
        {
            ThrowIfInvalid(pollInterval);

            if (HasStarted())
            {
                return _task;
            }

            _task = Task.Factory.StartNew(() =>
            {
                for (;;)
                {
                    if (_token.Token.WaitCancellationRequested(pollInterval))
                    {
                        break;
                    }

                    try
                    {
                        action();
                    }
                    catch (Exception)
                    {
                        _token.Cancel();
                    }
                }
            }, _token.Token, taskCreationOptions, TaskScheduler.Default);

            return _task;
        }

        private bool HasStarted()
        {
            return _task != null && (_task.IsCompleted == false ||
                                     _task.Status == TaskStatus.Running ||
                                     _task.Status == TaskStatus.WaitingToRun ||
                                     _task.Status == TaskStatus.WaitingForActivation);
        }

        // <inheritdoc />
        public Task Interval(TimeSpan pollInterval, TaskCreationOptions taskCreationOptions, Action action, CancellationToken token)
        {
            ThrowIfInvalid(pollInterval);

            if (HasStarted())
            {
                return _task;
            }

            _token = CancellationTokenSource.CreateLinkedTokenSource(token);

            _task = Task.Factory.StartNew(() =>
            {
                for (;;)
                {
                    if (token.WaitCancellationRequested(pollInterval))
                    {
                        break;
                    }

                    try
                    {
                        action();
                    }
                    catch (Exception)
                    {
                        _token.Cancel();
                    }
                }
            }, token, taskCreationOptions, TaskScheduler.Default);

            return _task;
        }

        // <inheritdoc />
        public void Stop()
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(DefaultTaskScheduler));
            }

            _token?.Cancel();
        }

        private bool CheckDisposed() => _disposed;

        private void ThrowIfInvalid(TimeSpan pollInterval)
        {
            if (pollInterval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval));
            }            
        }
    }
}