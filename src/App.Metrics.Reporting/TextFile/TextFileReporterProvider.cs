// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;
using System.IO;
using App.Metrics.Internal;
using Serilog;

namespace App.Metrics.Reporting.TextFile
{
    public class TextFileReporterProvider : IReporterProvider
    {
        private readonly ITextFileReporterSettings _settings;
        private readonly IMetricsFilter _filter;
        private readonly Func<IMetricsContext> _metricContext;
        private readonly ConcurrentDictionary<string, FileReporter> _reporters = new ConcurrentDictionary<string, FileReporter>();

        public TextFileReporterProvider(ITextFileReporterSettings settings, IMetricsFilter filter)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (filter == null)
            {
                throw new ArgumentException(nameof(filter));
            }

            _settings = settings;
            _filter = filter;
        }

        public TextFileReporterProvider(ITextFileReporterSettings settings)
            : this(settings, new NoOpFilter())
        {
        }

        public IReporter CreateReporter(string name)
        {
            return _reporters.GetOrAdd(name, CreateReporterImplementation);
        }

        public void Dispose()
        {
        }

        private FileReporter CreateReporterImplementation(string name)
        {
            return new FileReporter(name, _settings.Interval, _settings.FileReportingFolder, _settings.Disabled, _filter);
        }
    }
}