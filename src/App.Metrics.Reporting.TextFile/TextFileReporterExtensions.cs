// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace App.Metrics.Reporting
{
    public static class TextFileReporterExtensions
    {
        public static IReportFactory AddTextFile(this IReportFactory factory,
            ITextFileReporterSettings settings)
        {
            factory.AddProvider(new TextFileReporterProvider(settings));
            return factory;
        }

        public static IReportFactory AddTextFile(this IReportFactory factory)
        {
            var settings = new TextFileReporterSettings();
            factory.AddTextFile(settings);
            return factory;
        }
    }
}