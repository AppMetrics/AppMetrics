// <copyright file="IgnoredStatusCodeTestStartup.cs" company="App Metrics Contributors">
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
    // ReSharper disable ClassNeverInstantiated.Global
    public class IgnoredStatusCodeTestStartup : TestStartup
        // ReSharper restore ClassNeverInstantiated.Global
    {
        // ReSharper disable UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
            // ReSharper restore UnusedMember.Global
        {
            app.UseMetricsEndpoint();
            app.UseMetricsAllMiddleware();

            SetupAppBuilder(app, env, loggerFactory);
        }

        // ReSharper disable UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
            // ReSharper restore UnusedMember.Global
        {
            var appMetricsOptions = new MetricsOptions
                                    {
                                        DefaultContextLabel = "testing",
                                        Enabled = true
                                    };

            var endpointsOptions = new MetricEndpointsOptions
                                   {
                                        MetricsTextEndpointEnabled = true,
                                       MetricsEndpointEnabled = true
            };

            var trackingOptions = new MetricsWebTrackingOptions();
            trackingOptions.IgnoredHttpStatusCodes.Add(401);

            SetupServices(services, appMetricsOptions, trackingOptions, endpointsOptions);
        }
    }
}