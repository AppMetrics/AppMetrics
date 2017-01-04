// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using App.Metrics.Reporting.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Extensions.Reporting.TextFile
{
    public class TextFileReporterProvider : IReporterProvider
    {
        private readonly TextFileReporterSettings _settings;

        public TextFileReporterProvider(TextFileReporterSettings settings, IMetricsFilter fitler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
            Filter = fitler;
        }

        public IMetricsFilter Filter { get; }

        public IMetricReporter CreateMetricReporter(string name, ILoggerFactory loggerFactory)
        {
            return new TextFileReporter(name, _settings.FileName, _settings.ReportInterval, loggerFactory);
        }
    }
}