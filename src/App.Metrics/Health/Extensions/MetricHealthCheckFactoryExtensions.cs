// <copyright file="MetricHealthCheckFactoryExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using App.Metrics.Apdex;
using App.Metrics.Core.Options;
using App.Metrics.Counter;
using App.Metrics.Gauge;
using App.Metrics.Health;
using App.Metrics.Health.Abstractions;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Tagging;
using App.Metrics.Timer;

// ReSharper disable CheckNamespace
namespace App.Metrics
    // ReSharper restore CheckNamespace
{
    public static class MetricHealthCheckFactoryExtensions
    {
#pragma warning disable SA1008, SA1009
        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            ApdexOptions options,
            Func<ApdexValue, (string message, bool result)> passing,
            Func<ApdexValue, (string message, bool result)> warning = null,
            Func<ApdexValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            ApdexOptions options,
            MetricTags tags,
            Func<ApdexValue, (string message, bool result)> passing,
            Func<ApdexValue, (string message, bool result)> warning = null,
            Func<ApdexValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetApdexValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetApdexValue(options.Context, options.Name, tags);
                    return PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            CounterOptions options,
            Func<CounterValue, (string message, bool result)> passing,
            Func<CounterValue, (string message, bool result)> warning = null,
            Func<CounterValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            CounterOptions options,
            MetricTags tags,
            Func<CounterValue, (string message, bool result)> passing,
            Func<CounterValue, (string message, bool result)> warning = null,
            Func<CounterValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetCounterValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetCounterValue(options.Context, options.Name, tags);
                    return PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            GaugeOptions options,
            Func<double, (string message, bool result)> passing,
            Func<double, (string message, bool result)> warning = null,
            Func<double, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            GaugeOptions options,
            MetricTags tags,
            Func<double, (string message, bool result)> passing,
            Func<double, (string message, bool result)> warning = null,
            Func<double, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetGaugeValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetGaugeValue(options.Context, options.Name, tags);
                    return PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            MeterOptions options,
            Func<MeterValue, (string message, bool result)> passing,
            Func<MeterValue, (string message, bool result)> warning = null,
            Func<MeterValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            MeterOptions options,
            MetricTags tags,
            Func<MeterValue, (string message, bool result)> passing,
            Func<MeterValue, (string message, bool result)> warning = null,
            Func<MeterValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetMeterValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetMeterValue(options.Context, options.Name, tags);
                    return PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            HistogramOptions options,
            Func<HistogramValue, (string message, bool result)> passing,
            Func<HistogramValue, (string message, bool result)> warning = null,
            Func<HistogramValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            HistogramOptions options,
            MetricTags tags,
            Func<HistogramValue, (string message, bool result)> passing,
            Func<HistogramValue, (string message, bool result)> warning = null,
            Func<HistogramValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetHistogramValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetHistogramValue(options.Context, options.Name, tags);
                    return PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            TimerOptions options,
            Func<TimerValue, (string message, bool result)> passing,
            Func<TimerValue, (string message, bool result)> warning = null,
            Func<TimerValue, (string message, bool result)> failing = null)
        {
            return factory.RegisterMetricCheck(name, options, MetricTags.Empty, passing, warning, failing);
        }

        public static IHealthCheckFactory RegisterMetricCheck(
            this IHealthCheckFactory factory,
            string name,
            TimerOptions options,
            MetricTags tags,
            Func<TimerValue, (string message, bool result)> passing,
            Func<TimerValue, (string message, bool result)> warning = null,
            Func<TimerValue, (string message, bool result)> failing = null)
        {
            factory.Register(
                name,
                () =>
                {
                    var value = tags.Count == 0
                        ? factory.Metrics.Value.Snapshot.GetTimerValue(options.Context, options.Name)
                        : factory.Metrics.Value.Snapshot.GetTimerValue(options.Context, options.Name, tags);
                    return PerformCheck(passing, warning, failing, value);
                });

            return factory;
        }

        private static Task<HealthCheckResult> PerformCheck<T>(
            Func<T, (string message, bool result)> passing,
            Func<T, (string message, bool result)> warning,
            Func<T, (string message, bool result)> failing,
            T value)
        {
            if (value == null)
            {
                return Task.FromResult(HealthCheckResult.Ignore("Metric not found"));
            }

            var passingResult = passing(value);

            if (passingResult.result)
            {
                return Task.FromResult(HealthCheckResult.Healthy(passingResult.message));
            }

            if (warning != null)
            {
                var warningResult = warning(value);

                if (warningResult.result)
                {
                    return Task.FromResult(HealthCheckResult.Degraded(warningResult.message));
                }
            }

            if (failing != null)
            {
                var failingResult = failing(value);

                if (failingResult.result)
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy(failingResult.message));
                }
            }

            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
#pragma warning restore SA1008, SA1009
}