// <copyright file="DefaultTaskScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Scheduling.Abstractions;

namespace App.Metrics.Scheduling
{
    public sealed class DefaultTaskScheduler : IScheduler
    {
        private readonly bool _allowMulitpleTasks;
        private bool _disposed;
        private Task _task;
        private CancellationTokenSource _token;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultTaskScheduler" /> class.
        /// </summary>
        /// <param name="allowMulitpleTasks">
        ///     if set to <c>true</c> allows more than one task to be created at a time, otherwise
        ///     ensures the task hasn't yet started.
        /// </param>
        public DefaultTaskScheduler(bool allowMulitpleTasks = true)
        {
            _allowMulitpleTasks = allowMulitpleTasks;
            _token = new CancellationTokenSource();
        }

        public void Dispose() { Dispose(true); }

        // ReSharper disable MemberCanBePrivate.Global
        public void Dispose(bool disposing)
            // ReSharper restore MemberCanBePrivate.Global
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
        public Task Interval(
            TimeSpan pollInterval,
            TaskCreationOptions taskCreationOptions,
            Action action)
        {
            ThrowIfInvalid(pollInterval);

            if (HasStarted() && !_allowMulitpleTasks)
            {
                return _task;
            }

            _task = Task.Factory.StartNew(
                () =>
                {
                    do
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
                    while (true);
                },
                _token.Token,
                taskCreationOptions,
                TaskScheduler.Default);

            return _task;
        }

        // <inheritdoc />
        public Task Interval(
            TimeSpan pollInterval,
            TaskCreationOptions taskCreationOptions,
            Action action,
            CancellationToken token)
        {
            ThrowIfInvalid(pollInterval);

            if (HasStarted() && !_allowMulitpleTasks)
            {
                return _task;
            }

            _token = CancellationTokenSource.CreateLinkedTokenSource(token);

            _task = Task.Factory.StartNew(
                () =>
                {
                    do
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
                    while (true);
                },
                token,
                taskCreationOptions,
                TaskScheduler.Default);

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

        private static void ThrowIfInvalid(TimeSpan pollInterval)
        {
            if (pollInterval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(pollInterval));
            }
        }

        private bool CheckDisposed() => _disposed;

        private bool HasStarted()
        {
            return _task != null && (_task.IsCompleted == false ||
                                     _task.Status == TaskStatus.Running ||
                                     _task.Status == TaskStatus.WaitingToRun ||
                                     _task.Status == TaskStatus.WaitingForActivation);
        }
    }
}