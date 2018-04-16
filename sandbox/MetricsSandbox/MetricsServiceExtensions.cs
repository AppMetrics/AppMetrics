// <copyright file="MetricsServiceExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MetricsSandbox
{
    public static class MetricsServiceExtensions
    {
        public static IServiceCollection ConfigureMetrics(this IServiceCollection services, HostBuilderContext hostContext, bool filterMetricValueTypes)
        {
            var dataKeys = new GeneratedMetricNameMapping();

            if (filterMetricValueTypes)
            {
                dataKeys.Apdex.Remove(ApdexValueDataKeys.Samples);
                dataKeys.Counter.Remove(CounterValueDataKeys.Total);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Samples);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Count);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Sum);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Min);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Max);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Mean);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.Median);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.StdDev);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.P999);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.P99);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.P98);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.P95);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.P75);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.UserLastValue);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.UserMinValue);
                dataKeys.Histogram.Remove(HistogramValueDataKeys.UserMaxValue);
                dataKeys.Meter.Remove(MeterValueDataKeys.Count);
                dataKeys.Meter.Remove(MeterValueDataKeys.Rate1M);
                dataKeys.Meter.Remove(MeterValueDataKeys.Rate15M);
                dataKeys.Meter.Remove(MeterValueDataKeys.RateMean);
                dataKeys.Meter.Remove(MeterValueDataKeys.SetItemPercent);
            }

            var metricsConfigSection = hostContext.Configuration.GetSection(nameof(MetricsOptions));
            // Metrics = AppMetrics.CreateDefaultBuilder() to use the default configuration
            var metrics = new MetricsBuilder()
                      .Configuration.Configure(options =>
                      {
                          options.AddServerTag();
                          options.AddAppTag();
                          options.AddEnvTag();
                      })
                      .Configuration.Configure(metricsConfigSection.AsEnumerable())
                      .OutputEnvInfo.AsPlainText()
                      .OutputMetrics.AsPlainText(options => { options.DataKeys = dataKeys; })
                      .SampleWith.ForwardDecaying()
                      .TimeWith.StopwatchClock()
                      .Report.Using<SimpleConsoleMetricsReporter>(TimeSpan.FromSeconds(2))
                      .Build();

            services.AddSingleton(metrics);

            return services;
        }
    }
}