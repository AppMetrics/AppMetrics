// <copyright file="HealthReporterCollection.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace App.Metrics.Health.Internal
{
    public class HealthReporterCollection : Collection<IReportHealthStatus>
    {
        public HealthReporterCollection() { }

        public HealthReporterCollection(IList<IReportHealthStatus> list)
            : base(list)
        {
        }

        public IReportHealthStatus GetType<TReporter>()
            where TReporter : IReportHealthStatus
        {
            return GetType(typeof(TReporter));
        }

        public IReportHealthStatus GetType(Type reporterType)
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
            where TReporter : IReportHealthStatus
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

        public void TryAdd<TReporter>(IReportHealthStatus reporter)
            where TReporter : IReportHealthStatus
        {
            RemoveType<TReporter>();
            Add(reporter);
        }

        public void TryAdd(IReportHealthStatus reporter)
        {
            RemoveType(reporter.GetType());
            Add(reporter);
        }
    }
}