// <copyright file="NewLineFormat.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters.Prometheus
{
    /// <summary>
    /// Line endings format
    /// </summary>
    public enum NewLineFormat
    {
        /// <summary>
        /// Use Unix style new line character by default
        /// </summary>
        Default,

        /// <summary>
        /// Use Environement.NewLine as new line character
        /// </summary>
        Auto,

        /// <summary>
        /// Use '\r\n' as new line character
        /// </summary>
        Windows,

        /// <summary>
        /// Use '\r' as new line character
        /// </summary>
        Unix
    }
}