// <copyright file="AppMetricsTaskScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Internal;
using App.Metrics.Logging;

namespace App.Metrics.Scheduling
{
    public class AppMetricsTaskScheduler : IDisposable
    {
        private static readonly ILog Logger = LogProvider.For<AppMetricsTaskScheduler>();
        private readonly TimeSpan _interval;
        private readonly Func<Task> _task;
        private readonly object _syncLock = new object();
        private readonly IMetricsTaskSchedular _scheduler;
        private volatile bool _disposing;

        public AppMetricsTaskScheduler(TimeSpan interval, Func<Task> task)
        {
            _interval = interval;
            _task = task;
            _scheduler = new DefaultMetricsTaskSchedular(c => Tick());
        }

        public void Dispose()
        {
            lock (_syncLock)
            {
                if (_disposing)
                {
                    return;
                }

                _disposing = true;
            }

            _scheduler.Dispose();

            Tick().GetAwaiter().GetResult();
        }

        public void Start()
        {
            _scheduler.Start(_interval);
        }

        private async Task Tick()
        {
            try
            {
                await _task();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while running sample requests {0}: {1}", this, ex);
            }
            finally
            {
                lock (_syncLock)
                {
                    if (!_disposing)
                    {
                        Start();
                    }
                }
            }
        }
    }
}