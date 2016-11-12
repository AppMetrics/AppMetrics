// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Scheduling;

namespace App.Metrics.Reporting
{
    public class ReportFactory : IReportFactory
    {
        private readonly Dictionary<Type, IReporterProvider> _providers = new Dictionary<Type, IReporterProvider>();
        private readonly object _syncLock = new object();
        private volatile bool _disposed;

        public void AddProvider(IReporterProvider provider)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(ReportFactory));
            }

            lock (_syncLock)
            {
                _providers.Add(provider.GetType(), provider);
            }
        }

        public IReporter CreateReporter(IScheduler scheduler)
        {
            //TODO: AH - doing nothing with scheduler?
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(ReportFactory));
            }

            return new Reporter(this);
        }

        public IReporter CreateReporter()
        {
            return CreateReporter(new DefaultTaskScheduler());
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            foreach (var provider in _providers)
            {
                try
                {
                    provider.Value.Dispose();
                }
                catch
                {
                    // Swallow exceptions on dispose
                }
            }
        }

        internal Dictionary<Type, IReporterProvider> GetProviders()
        {
            return _providers;
        }

        protected virtual bool CheckDisposed() => _disposed;
    }
}