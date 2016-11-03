// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Concurrent;

namespace App.Metrics.Reporting
{
    public class TextFileReporterProvider : IReporterProvider
    {
        private readonly ConcurrentDictionary<string, TextFileReporter> _reporters = new ConcurrentDictionary<string, TextFileReporter>();
        private readonly ITextFileReporterSettings _settings;

        public TextFileReporterProvider(ITextFileReporterSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;
        }

        public IReporterSettings Settings => _settings;

        public IMetricReporter CreateMetricReporter(string name)
        {
            return _reporters.GetOrAdd(name, CreateReporterImplementation);
        }

        public void Dispose()
        {
        }

        private TextFileReporter CreateReporterImplementation(string name)
        {
            return new TextFileReporter(name, _settings.FileName, _settings.ReportInterval);
        }
    }
}