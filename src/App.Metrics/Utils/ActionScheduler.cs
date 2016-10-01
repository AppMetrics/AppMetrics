using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Utils
{
    /// <summary>
    ///     Utility class to schedule an Action to be executed repeatedly according to the interval.
    /// </summary>
    /// <remarks>
    ///     The scheduling code is inspired form Daniel Crenna's metrics port
    ///     https://github.com/danielcrenna/metrics-net/blob/master/src/metrics/Reporting/ReporterBase.cs
    /// </remarks>
    public sealed class ActionScheduler : Scheduler
    {
        private CancellationTokenSource token;

        public void Dispose()
        {
            if (this.token != null)
            {
                this.token.Cancel();
                this.token.Dispose();
            }
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

            if (this.token != null)
            {
                throw new InvalidOperationException("Scheduler is already started.");
            }

            this.token = new CancellationTokenSource();

            RunScheduler(interval, action, this.token);
        }

        public void Stop()
        {
            if (this.token != null)
            {
                token.Cancel();
            }
        }

        private static void RunScheduler(TimeSpan interval, Func<CancellationToken, Task> action, CancellationTokenSource token)
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
                        catch (Exception x)
                        {
                            MetricsErrorHandler.Handle(x, "Error while executing action scheduler.");
                            token.Cancel();
                        }
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }, token.Token);
        }
    }
}