// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.Metrics.Reporting
{
    internal sealed class Reporter : IReporter
    {
        private readonly string _name;
        private readonly DefaultReportGenerator _reportGenerator;
        private IMetricReporter[] _reporters;

        public Reporter(ReportFactory reportFactory, string name)
        {
            _name = name;
            _reportGenerator = new DefaultReportGenerator();

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
                    await _reportGenerator.Generate(reporter, context, token);
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
                    message: "An error occurred while running reporter(s).", innerExceptions: exceptions);
            }
        }

        internal void AddProvider(IReporterProvider provider)
        {
            var reporter = provider.CreateMetricReporter(_name);
            int logIndex;
            if (_reporters == null)
            {
                logIndex = 0;
                _reporters = new IMetricReporter[1];
            }
            else
            {
                logIndex = _reporters.Length;
                Array.Resize(ref _reporters, logIndex + 1);
            }
            _reporters[logIndex] = reporter;
        }
    }
}