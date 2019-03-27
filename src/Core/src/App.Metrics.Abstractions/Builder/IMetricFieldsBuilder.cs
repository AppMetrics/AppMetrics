// <copyright file="IMetricFieldsBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public interface IMetricFieldsBuilder
    {
        /// <summary>
        /// Gets the <see cref="IMetricsBuilder"/> where App Metrics is configured.
        /// </summary>
        IMetricsBuilder Builder { get; }

        IMetricsBuilder Configure(Action<MetricFields> setupFields);
    }
}