// <copyright file="MetricFieldsBuilder.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public class MetricFieldsBuilder : IMetricFieldsBuilder
    {
        private readonly MetricFields _metricFields;
        private readonly Action<MetricFields> _configureAction;

        public MetricFieldsBuilder(
            IMetricsBuilder metricsBuilder,
            MetricFields metricFields,
            Action<MetricFields> configureAction)
        {
            Builder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            _configureAction = configureAction ?? throw new ArgumentNullException(nameof(configureAction));
            _metricFields = metricFields ?? new MetricFields();
        }

        /// <inheritdoc />
        public IMetricsBuilder Builder { get; }

        /// <inheritdoc />
        public IMetricsBuilder Configure(Action<MetricFields> configureAction)
        {
            if (configureAction == null)
            {
                throw new ArgumentNullException(nameof(configureAction));
            }

            configureAction(_metricFields);

            _configureAction(_metricFields);

            return Builder;
        }
    }
}