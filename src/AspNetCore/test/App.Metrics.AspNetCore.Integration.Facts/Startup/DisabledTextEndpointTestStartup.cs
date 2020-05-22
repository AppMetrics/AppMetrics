// <copyright file="DisabledTextEndpointTestStartup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public class DisabledTextEndpointTestStartup : TestStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMetricsEndpoint();
            app.UseMetricsTextEndpoint();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appMetricsOptions = new MetricsOptions();

            var endpointsOptions = new MetricEndpointsOptions
                                   {
                                       MetricsTextEndpointEnabled = false,
                                       MetricsEndpointEnabled = true
                                   };

            var trackingOptions = new MetricsWebTrackingOptions();

            SetupServices(services, appMetricsOptions, trackingOptions, endpointsOptions);
        }
    }
}