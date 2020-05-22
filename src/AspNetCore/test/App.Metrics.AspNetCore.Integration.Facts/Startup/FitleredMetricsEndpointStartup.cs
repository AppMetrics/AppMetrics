// <copyright file="FitleredMetricsEndpointStartup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Counter;
using App.Metrics.Filtering;
using App.Metrics.Gauge;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.Timer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class FitleredMetricsEndpointStartup : TestStartup
        // ReSharper restore ClassNeverInstantiated.Global
    {
        // ReSharper disable UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
            // ReSharper restore UnusedMember.Global
        {
            app.UseMetricsEndpoint();
            app.UseMetricsTextEndpoint();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);

            RecordSomeMetrics();
        }

        // ReSharper disable UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
            // ReSharper restore UnusedMember.Global
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        Enabled = true
                                    };

            var endpointOptions = new MetricEndpointsOptions();
            var trackingOptions = new MetricsWebTrackingOptions();

            SetupServices(
                services,
                appMetricsOptions,
                trackingOptions,
                endpointOptions,
                new MetricsFilter().WhereType(MetricType.Counter));
        }

        private void RecordSomeMetrics()
        {
            var counterOptions = new CounterOptions
                                 {
                                     Name = "test_counter",
                                     MeasurementUnit = Unit.Requests,
                                     Tags = new MetricTags("tag1", "value")
                                 };

            var meterOptions = new MeterOptions
                               {
                                   Name = "test_meter",
                                   MeasurementUnit = Unit.None,
                                   Tags = new MetricTags("tag2", "value")
                               };

            var timerOptions = new TimerOptions
                               {
                                   Name = "test_timer",
                                   MeasurementUnit = Unit.Requests
                               };

            var histogramOptions = new HistogramOptions
                                   {
                                       Name = "test_histogram",
                                       MeasurementUnit = Unit.Requests
                                   };

            var gaugeOptions = new GaugeOptions
                               {
                                   Name = "test_gauge"
                               };

            Metrics.Measure.Counter.Increment(counterOptions);
            Metrics.Measure.Meter.Mark(meterOptions);
            Metrics.Measure.Timer.Time(timerOptions, () => Metrics.Clock.Advance(TimeUnit.Milliseconds, 10));
            Metrics.Measure.Histogram.Update(histogramOptions, 5);
            Metrics.Measure.Gauge.SetValue(gaugeOptions, () => 8);
        }
    }
}