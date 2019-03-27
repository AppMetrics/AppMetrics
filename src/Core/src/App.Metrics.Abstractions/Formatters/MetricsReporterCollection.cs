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

        public IReportMetrics GetType<TReporter>()
            where TReporter : IReportMetrics
        {
            return GetType(typeof(TReporter));
        }

        public IReportMetrics GetType(Type reporterType)
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                var reporter = this[i];
                if (reporter.GetType() == reporterType)
                {
                    return reporter;
                }
            }

            return default;
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
            RemoveType<TReporter>();
            Add(reporter);
        }

        public void TryAdd(IReportMetrics reporter)
        {
            RemoveType(reporter.GetType());
            Add(reporter);
        }
    }
}