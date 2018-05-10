// <copyright file="MetricsTextOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Text;
using App.Metrics.Formatters.Ascii.Internal;

namespace App.Metrics.Formatters.Ascii
{
    /// <summary>
    ///     Provides programmatic configuration for ASCII formatting the App Metrics framework.
    /// </summary>
    public class MetricsTextOptions
    {
        public MetricsTextOptions()
        {
            Padding = MetricsTextFormatterConstants.OutputFormatting.Padding;
            Separator = MetricsTextFormatterConstants.OutputFormatting.Separator;
            Encoding = Encoding.ASCII;
        }

        /// <summary>
        ///     Gets or sets the padding to apply on health check labels and messages
        /// </summary>
        /// <value>
        ///     The padding to apply on health check labels and messages
        /// </value>
        public int Padding { get; set; }

        /// <summary>
        ///     Gets or sets the separator to use between on health check labels and messages/status
        /// </summary>
        /// <value>
        ///     The separator to apply between on health check labels and messages/status
        /// </value>
        public string Separator { get; set; }

        public Encoding Encoding { get; set; }

        public Func<string, string, string> MetricNameFormatter { get; set; }
    }
}
