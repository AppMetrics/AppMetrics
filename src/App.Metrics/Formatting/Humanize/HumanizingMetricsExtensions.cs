// <copyright file="HumanizingMetricsExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Apdex;
using App.Metrics.Core.Abstractions;
using App.Metrics.Counter;
using App.Metrics.Health;
using App.Metrics.Histogram;
using App.Metrics.Infrastructure;
using App.Metrics.Meter;
using App.Metrics.Timer;

namespace App.Metrics.Formatting.Humanize
{
    public static class HumanizingMetricsExtensions
    {
        private static readonly IReadOnlyDictionary<Type, string> MetricTypeDisplayNameMapping = new Dictionary<Type, string>
                                                                                                 {
                                                                                                     { typeof(CounterValue), "Counters" },
                                                                                                     { typeof(double), "Gauges" },
                                                                                                     { typeof(MeterValue), "Meters" },
                                                                                                     { typeof(TimerValue), "Timers" },
                                                                                                     { typeof(ApdexValue), "ApdexScores" },
                                                                                                     { typeof(HistogramValue), "Histograms" },
                                                                                                     { typeof(HealthStatus), "Health Checks" },
                                                                                                     {
                                                                                                         typeof(EnvironmentInfo),
                                                                                                         "Environment Information"
                                                                                                     }
                                                                                                 };

        public static string HumanzeEndMetricType(this Type metricType)
        {
            var metricTypeDisplay = MetricTypeDisplayNameMapping[metricType];

            return string.Format("***** End - {0} *****" + Environment.NewLine, metricTypeDisplay);
        }

        public static string HumanzeStartMetricType(this Type metricType, string context = null)
        {
            var metricTypeDisplay = MetricTypeDisplayNameMapping[metricType];

            if (context.IsPresent())
            {
                return string.Format(
                    Environment.NewLine + "***** [{0}] {1} *****" + Environment.NewLine + Environment.NewLine,
                    context,
                    metricTypeDisplay);
            }

            return string.Format(Environment.NewLine + "***** {0} *****" + Environment.NewLine + Environment.NewLine, metricTypeDisplay);
        }

        public static string HumanzizeName<T>(this MetricValueSourceBase<T> valueSource, string context = null)
        {
            return context.IsPresent() ? $"\t[{context}] {valueSource.Name}" : $"\t{valueSource.Name}";
        }

        public static string Hummanize<T>(this MetricValueSourceBase<T> valueSource)
        {
#if NET452
            var formatProvider = new HumanizeMetricValueFormatProvider<T>();
            var formatter = formatProvider.GetFormat(valueSource.GetType()) as ICustomFormatter;
            return formatter != null
                ? formatter.Format(string.Empty, valueSource, null)
                : string.Empty;
#else
            FormattableString metricValueSourceString = $"{valueSource}";
            return metricValueSourceString.ToString(new HumanizeMetricValueFormatProvider<T>());
#endif
        }

        public static string Hummanize(this EnvironmentInfo environmentInfo)
        {
#if NET452
            var formatProvider = new HumanizeEnvironmentInfoFormatter();
            return formatProvider.Format(string.Empty, environmentInfo, null);
#else
            FormattableString environmentInfoString = $"{environmentInfo}";
            return environmentInfoString.ToString(new HumanizeMetricValueFormatProvider<EnvironmentInfo>());
#endif
        }

        public static string Hummanize(this HealthCheck.Result healthCheckResult)
        {
#if NET452
            var formatProvider = new HumanizeHealthCheckResultFormatter();
            return formatProvider.Format(string.Empty, healthCheckResult, null);
#else
            FormattableString healthCheckResultString = $"{healthCheckResult}";
            return healthCheckResultString.ToString(new HumanizeMetricValueFormatProvider<HealthCheck.Result>());
#endif
        }
    }
}