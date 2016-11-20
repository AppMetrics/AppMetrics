// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.Data;

namespace App.Metrics.Formatting.Humanize
{
    public sealed class HumanizeMetricValueFormatProvider<T> : IFormatProvider
    {
        private static readonly IDictionary<Type, Func<ICustomFormatter>> CustomFormaterTypesFactor = new Dictionary<Type, Func<ICustomFormatter>>
        {
            { typeof(CounterValue), () => new HumanizeCounterMetricFormatter() },
            { typeof(double), () => new HumanizeGaugeMetricFormatter() },
            { typeof(HistogramValue), () => new HumanizeHistogramMetricFormatter() },
            { typeof(MeterValue), () => new HumanizeMeterMetricFormatter() },
            { typeof(TimerValue), () => new HumanizeTimerMetricFormatter() },
            { typeof(HealthCheck.Result), () => new HumanizeHealthCheckResultFormatter() },
            { typeof(EnvironmentInfo), () => new HumanizeEnvironmentInfoFormatter() }
        };

        public object GetFormat(Type formatType)
        {
            var metricType = typeof(T);
            if (formatType == typeof(ICustomFormatter) && CustomFormaterTypesFactor.ContainsKey(metricType))
            {
                return CustomFormaterTypesFactor[metricType]();
            }

            return null;
        }
    }
}