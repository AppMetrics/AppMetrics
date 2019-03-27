// <copyright file="HealthTextOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Text;

namespace App.Metrics.Health.Formatters.Ascii
{
    public class HealthTextOptions
    {
        public HealthTextOptions()
        {
            Padding = 20;
            Separator = "=";
            Encoding = Encoding.UTF8;
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
    }
}