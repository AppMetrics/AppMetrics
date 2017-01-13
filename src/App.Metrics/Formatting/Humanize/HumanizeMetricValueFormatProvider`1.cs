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
        private readonly IDictionary<Type, Func<ICustomFormatter>> _customFormaterTypesFactor = new Dictionary<Type, Func<ICustomFormatter>>
                                                                                                {
                                                                                                    {
                                                                                                        typeof(CounterValue),
                                                                                                        () => new HumanizeCounterMetricFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(double),
                                                                                                        () => new HumanizeGaugeMetricFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(HistogramValue),
                                                                                                        () => new HumanizeHistogramMetricFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(MeterValue),
                                                                                                        () => new HumanizeMeterMetricFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(TimerValue),
                                                                                                        () => new HumanizeTimerMetricFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(ApdexValue),
                                                                                                        () => new HumanizeApdexScoreMetricFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(HealthCheck.Result),
                                                                                                        () => new HumanizeHealthCheckResultFormatter()
                                                                                                    },
                                                                                                    {
                                                                                                        typeof(EnvironmentInfo),
                                                                                                        () => new HumanizeEnvironmentInfoFormatter()
                                                                                                    }
                                                                                                };

        public object GetFormat(Type formatType)
        {
#if NET452
            var metricType = typeof(T);
            if (_customFormaterTypesFactor.ContainsKey(metricType))
            {
                return _customFormaterTypesFactor[metricType]();
            }

#else
            var metricType = typeof(T);
            if (formatType == typeof(ICustomFormatter) && _customFormaterTypesFactor.ContainsKey(metricType))
            {
                return _customFormaterTypesFactor[metricType]();
            }

#endif
            return null;
        }
    }
}