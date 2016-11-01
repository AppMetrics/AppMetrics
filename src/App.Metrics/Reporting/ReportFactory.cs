// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Reporting
{
    public class ReportFactory : IReportFactory
    {
        private readonly Dictionary<string, Reporter> _reporters = new Dictionary<string, Reporter>(StringComparer.Ordinal);
        private IReporterProvider[] _providers = new IReporterProvider[0];
        private volatile bool _disposed;
        private readonly object _syncLock = new object();

        //TODO: AH - don't need a name?
        public IReporter CreateReporter(string name)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(ReportFactory));
            }

            Reporter reporter;
            lock (_syncLock)
            {
                if (!_reporters.TryGetValue(name, out reporter))
                {
                    reporter = new Reporter(this, name);
                    _reporters[name] = reporter;
                }
            }
            return reporter;
        }

        internal IReporterProvider[] GetProviders()
        {
            return _providers;
        }

        public void AddProvider(IReporterProvider provider)
        {
            if (CheckDisposed())
            {
                throw new ObjectDisposedException(nameof(ReportFactory));
            }

            lock (_syncLock)
            {
                _providers = _providers.Concat(new[] { provider }).ToArray();
                foreach (var reporter in _reporters)
                {
                    reporter.Value.AddProvider(provider);
                }
            }
        }

        protected virtual bool CheckDisposed() => _disposed;

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            foreach (var provider in _providers)
            {
                try
                {
                    provider.Dispose();
                }
                catch
                {
                    // Swallow exceptions on dispose
                }
            }
        }
    }
}
