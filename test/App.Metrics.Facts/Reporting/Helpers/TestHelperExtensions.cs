// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public enum StringReporterSamples
    {
        EnvironmentInfo,
        HealthChecks,
        Counters,
        CountersWithGroup,
        Gauges,
        Meters,
        Histograms,
        Apdex,
        ApdexWithGroup,
        Timers
    }

    public static class TestHelperExtensions
    {
        private static readonly Dictionary<StringReporterSamples, string> StringReporterSampleMapping = new Dictionary<StringReporterSamples, string>
                                                                                                        {
                                                                                                            {
                                                                                                                StringReporterSamples.EnvironmentInfo,
                                                                                                                "environment_info"
                                                                                                            },
                                                                                                            {
                                                                                                                StringReporterSamples.HealthChecks,
                                                                                                                "health_checks"
                                                                                                            },
                                                                                                            {
                                                                                                                StringReporterSamples.Counters,
                                                                                                                "counters"
                                                                                                            },
                                                                                                            {
                                                                                                                StringReporterSamples.Gauges, "gauges"
                                                                                                            },
                                                                                                            {
                                                                                                                StringReporterSamples.Meters, "meters"
                                                                                                            },
                                                                                                            {
                                                                                                                StringReporterSamples.Histograms,
                                                                                                                "histograms"
                                                                                                            },
                                                                                                            { StringReporterSamples.Apdex, "apdex" },
                                                                                                            { StringReporterSamples.ApdexWithGroup, "apdex_with_group" },
                                                                                                            { StringReporterSamples.Timers, "timers" }
                                                                                                        };

        public static string ExtractStringReporterSampleFromResourceFile(this StringReporterSamples sample)
        {
            return ExtractTextFromEmbeddedResource(StringReporterSampleMapping[sample]);
        }

        private static string ExtractTextFromEmbeddedResource(string key)
        {
            var assemblyName = new AssemblyName("App.Metrics.Facts");
            using (
                var fileStream =
                    Assembly.Load(assemblyName).GetManifestResourceStream($"App.Metrics.Facts.Reporting.StringReports.{key}.txt"))
            {
                if (fileStream == null)
                {
                    return null;
                }
                using (var textReader = new StreamReader(fileStream))
                {
                    return textReader.ReadToEnd();
                }
            }
        }
    }
}