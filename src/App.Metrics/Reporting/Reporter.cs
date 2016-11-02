// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Infrastructure;
using App.Metrics.Scheduling;

namespace App.Metrics.Reporting
{   
    internal sealed class Reporter : IReporter
    {
        private readonly string _name;
        private readonly DefaultReportGenerator _reportGenerator;
        private readonly Scheduling.IScheduler _scheduler;
        private IMetricReporter[] _reporters;

        public Reporter(ReportFactory reportFactory, string name)
        {
            _name = name;
            _reportGenerator = new DefaultReportGenerator();
            _scheduler = new DefaultTaskScheduler();

            var providers = reportFactory.GetProviders();

            if (providers.Length <= 0) return;

            _reporters = new IMetricReporter[providers.Length];

            for (var index = 0; index < providers.Length; index++)
            {
                _reporters[index] = providers[index].CreateMetricReporter(name);
            }
        }

        public async Task RunReports(IMetricsContext context, CancellationToken token)
        {
            if (_reporters == null)
            {
                return;
            }

            List<Exception> exceptions = null;

            foreach (var reporter in _reporters)
            {
                try
                {
                    var task = _scheduler.Interval(reporter.ReportInterval, async () =>
                            await _reportGenerator.Generate(reporter, context, token), token);
                }
                catch (Exception ex)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }

                    exceptions.Add(ex);
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException(
                    message: "An error occurred while running reporter(s).",
                    innerExceptions: exceptions);
            }
            await AppMetricsTaskCache.EmptyTask;
        }

        internal void AddProvider(IReporterProvider provider)
        {
            var reporter = provider.CreateMetricReporter(_name);
            int reporterIndex;
            if (_reporters == null)
            {
                reporterIndex = 0;
                _reporters = new IMetricReporter[1];
            }
            else
            {
                reporterIndex = _reporters.Length;
                Array.Resize(ref _reporters, reporterIndex + 1);
            }
            _reporters[reporterIndex] = reporter;
        }
    }
}