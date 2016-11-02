using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Internal;

namespace App.Metrics.Scheduling
{
    public class DefaultTaskScheduler : IScheduler
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

        protected virtual bool CheckDisposed() => _disposed;
    }
}