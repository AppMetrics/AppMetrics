// <copyright file="ReportFactory.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Core.Scheduling;
using App.Metrics.Reporting.Internal;
using App.Metrics.Scheduling;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Reporting
{
    public sealed class ReportFactory : IReportFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMetrics _metrics;
        private readonly Dictionary<Type, IReporterProvider> _providers = new Dictionary<Type, IReporterProvider>();
        private readonly object _syncLock = new object();

        public ReportFactory(IMetrics metrics, ILoggerFactory loggerFactory)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public void AddProvider(IReporterProvider provider)
        {
            lock (_syncLock)
            {
                _providers.Add(provider.GetType(), provider);
            }
        }

        public IReporter CreateReporter(IScheduler scheduler) { return new DefaultReporter(this, _metrics, scheduler, _loggerFactory); }

        public IReporter CreateReporter() { return CreateReporter(new DefaultTaskScheduler()); }

        internal Dictionary<Type, IReporterProvider> GetProviders() { return _providers; }
    }
}