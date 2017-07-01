// <copyright file="TestHelperExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace App.Metrics.Reporting.Facts.TestHelpers
{
    public enum StringReporterSamples
    {
#pragma warning disable SA1602 // Enumeration items must be documented
        EnvironmentInfo,
        Counters,
        CountersWithGroup,
        Gauges,
        Meters,
        Histograms,
        Apdex,
        ApdexWithGroup,
        Timers
#pragma warning restore SA1602 // Enumeration items must be documented
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
                                                                                                            {
                                                                                                                StringReporterSamples.ApdexWithGroup,
                                                                                                                "apdex_with_group"
                                                                                                            },
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