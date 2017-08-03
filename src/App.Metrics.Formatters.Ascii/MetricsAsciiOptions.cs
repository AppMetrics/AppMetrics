// <copyright file="MetricsAsciiOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters.Ascii
{
    public class MetricsAsciiOptions
    {
        public MetricsAsciiOptions()
        {
            Padding = 20;
            Separator = "=";
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
    }
}
