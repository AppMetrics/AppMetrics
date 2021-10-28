// <copyright file="MetricsReporterCollection.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using App.Metrics.Reporting;

namespace App.Metrics.Formatters
{
    public class MetricsReporterCollection : Collection<IReportMetrics>
    {
        public MetricsReporterCollection() { }

        public MetricsReporterCollection(IList<IReportMetrics> list)
            : base(list)
        {
        }

        public IEnumerable<IReportMetrics> GetType<TReporter>()
            where TReporter : IReportMetrics
        {
            return GetType(typeof(TReporter));
        }

        public IEnumerable<IReportMetrics> GetType(Type reporterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var reporter = this[i];
                if (reporter.GetType() == reporterType)
                {
                    yield return reporter;
                }
            }
        }

        public void RemoveType<TReporter>()
            where TReporter : IReportMetrics
        {
            RemoveType(typeof(TReporter));
        }

        public void RemoveType(Type reporterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var reporter = this[i];
                if (reporter.GetType() == reporterType)
                {
                    RemoveAt(i);
                }
            }
        }

        public void TryAdd<TReporter>(IReportMetrics reporter)
            where TReporter : IReportMetrics
        {
            Add(reporter);
        }

        public void TryAdd(IReportMetrics reporter)
        {
            Add(reporter);
        }
    }
}