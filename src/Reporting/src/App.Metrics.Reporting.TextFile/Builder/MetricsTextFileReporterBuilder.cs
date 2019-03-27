// <copyright file="MetricsTextFileReporterBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.Builder;
using App.Metrics.Reporting.TextFile;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Builder for configuring metrics text file reporting using an
    ///     <see cref="IMetricsReportingBuilder" />.
    /// </summary>
    public static class MetricsTextFileReporterBuilder
    {
        /// <summary>
        ///     Add the <see cref="TextFileMetricsReporter" /> allowing metrics to be reported to text file. Default output
        ///     ./metrics.txt
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="options">The text file reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToTextFile(
            this IMetricsReportingBuilder reportingBuilder,
            MetricsReportingTextFileOptions options)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var reporter = new TextFileMetricsReporter(options);

            return reportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="TextFileMetricsReporter" /> allowing metrics to be reported to text file. Default output
        ///     ./metrics.txt
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="setupAction">The text file reporting options to use.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToTextFile(
            this IMetricsReportingBuilder reportingBuilder,
            Action<MetricsReportingTextFileOptions> setupAction)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingTextFileOptions();

            setupAction?.Invoke(options);

            var reporter = new TextFileMetricsReporter(options);

            return reportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="TextFileMetricsReporter" /> allowing metrics to be reported to text file. Default output
        ///     ./metrics.txt
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToTextFile(this IMetricsReportingBuilder reportingBuilder)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            return reportingBuilder.Using<TextFileMetricsReporter>();
        }

        /// <summary>
        ///     Add the <see cref="TextFileMetricsReporter" /> allowing metrics to be reported to text file. Default output
        ///     ./metrics.txt
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="output">The absolute directory and file name of the file where metric values are written.</param>
        /// <param name="flushInterval">
        ///     The <see cref="T:System.TimeSpan" /> interval used if intended to schedule metrics
        ///     reporting.
        /// </param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToTextFile(
            this IMetricsReportingBuilder reportingBuilder,
            string output,
            TimeSpan flushInterval)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingTextFileOptions
                          {
                              OutputPathAndFileName = output,
                              FlushInterval = flushInterval
                          };

            var reporter = new TextFileMetricsReporter(options);

            return reportingBuilder.Using(reporter);
        }

        /// <summary>
        ///     Add the <see cref="TextFileMetricsReporter" /> allowing metrics to be reported to text file. Default output
        ///     ./metrics.txt
        /// </summary>
        /// <param name="reportingBuilder">
        ///     The <see cref="IMetricsReportingBuilder" /> used to configure metrics reporters.
        /// </param>
        /// <param name="output">The absolute directory and file name of the file where metric values are written.</param>
        /// <returns>
        ///     An <see cref="IMetricsBuilder" /> that can be used to further configure App Metrics.
        /// </returns>
        public static IMetricsBuilder ToTextFile(
            this IMetricsReportingBuilder reportingBuilder,
            string output)
        {
            if (reportingBuilder == null)
            {
                throw new ArgumentNullException(nameof(reportingBuilder));
            }

            var options = new MetricsReportingTextFileOptions
                          {
                              OutputPathAndFileName = output
                          };

            var reporter = new TextFileMetricsReporter(options);

            return reportingBuilder.Using(reporter);
        }
    }
}