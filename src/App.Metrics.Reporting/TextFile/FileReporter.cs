// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics.Internal;

namespace App.Metrics.Reporting.TextFile
{
    public class FileReporter : IReporter
    {

        public FileReporter(string name, TimeSpan interval,
            string fileReportingFolder,
            bool isEnabled, IMetricsFilter filter)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (fileReportingFolder == null)
            {
                throw new ArgumentNullException(nameof(fileReportingFolder));
            }

            Name = name;
            Filter = filter ?? new NoOpFilter();
            Interval = interval;
            IsEnabled = isEnabled;
            FileReportingFolder = fileReportingFolder;
        }

        public TimeSpan Interval { get; }

        public IMetricsFilter Filter { get; }

        public bool IsEnabled { get; }

        public string FileReportingFolder { get; }

        public string Name { get; }

        public async Task RunReports(IMetricsContext context, CancellationToken token)
        {
           System.Console.Write("file reporter");
        }
    }
}