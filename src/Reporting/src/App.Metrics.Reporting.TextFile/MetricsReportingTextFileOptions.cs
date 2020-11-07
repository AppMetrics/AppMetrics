// <copyright file="MetricsReportingTextFileOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Reporting.Base;

namespace App.Metrics.Reporting.TextFile
{
    /// <summary>
    ///     Provides programmatic configuration of Text File Reporting in the App Metrics framework.
    /// </summary>
    public class MetricsReportingTextFileOptions : MetricsReportingBaseOptions
    {
        /// <summary>
        ///     Gets or sets a value indicating whether or not to [append metrics when writing to file].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [append metrics]; otherwise, <c>false</c>.
        /// </value>
        public bool AppendMetricsToTextFile { get; set; }

        /// <summary>
        ///     Gets or sets the directory and filename where metrics are written.
        /// </summary>
        /// <remarks>If not sets writes metrics.txt to the application's running directory.</remarks>
        public string OutputPathAndFileName { get; set; }
    }
}