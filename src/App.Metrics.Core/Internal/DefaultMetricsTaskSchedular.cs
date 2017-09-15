// <copyright file="DefaultMetricsTaskSchedular.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Logging;

namespace App.Metrics.Internal
{
    internal class DefaultMetricsTaskSchedular : IMetricsTaskSchedular
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsTaskSchedular>();
        private readonly object _syncLock = new object();
        private readonly Func<CancellationToken, Task> _onTick;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

#if THREADING_TIMER
        private readonly System.Threading.Timer _timer;
#endif

        private bool _running;
        private bool _disposed;

        public DefaultMetricsTaskSchedular(Func<CancellationToken, Task> onTick)
        {
            _onTick = onTick ?? throw new ArgumentNullException(nameof(onTick));

#if THREADING_TIMER
            _timer = new System.Threading.Timer(_ => OnTick(), null, Timeout.Infinite, Timeout.Infinite);
#endif
        }

        public void Start(TimeSpan interval)
        {
            if (interval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval));
            }

            lock (_syncLock)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(DefaultMetricsTaskSchedular));
                }

#if THREADING_TIMER
                _timer.Change(interval, Timeout.InfiniteTimeSpan);
#else
                Task.Delay(interval, _cancellationTokenSource.Token)
                    .ContinueWith(
                    _ => OnTick(),
                    CancellationToken.None,
                    TaskContinuationOptions.DenyChildAttach,
                    TaskScheduler.Default);
#endif
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();

            lock (_syncLock)
            {
                if (_disposed)
                {
                    return;
                }

                while (_running)
                {
                    Monitor.Wait(_syncLock);
                }

#if THREADING_TIMER
                _timer.Dispose();
#endif

                _disposed = true;
            }
        }

        private async void OnTick()
        {
            try
            {
                lock (_syncLock)
                {
                    if (_disposed)
                    {
                        return;
                    }

                    if (_running)
                    {
                        Monitor.Wait(_syncLock);

                        if (_disposed)
                        {
                            return;
                        }
                    }

                    _running = true;
                }

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    await _onTick(_cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException tcx)
            {
                Logger.Error("The timer was canceled during invocation: {0}", tcx);
            }
            finally
            {
                lock (_syncLock)
                {
                    _running = false;
                    Monitor.PulseAll(_syncLock);
                }
            }
        }
    }
}