// <copyright file="TestStartup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using App.Metrics.AspNetCore.Endpoints;
using App.Metrics.AspNetCore.Tracking;
using App.Metrics.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.Integration.Facts.Startup
{
    public abstract class TestStartup
    {
        protected IMetrics Metrics { get; private set; }

        protected void SetupAppBuilder(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Metrics = app.ApplicationServices.GetRequiredService<IMetrics>();

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected void SetupServices(
            IServiceCollection services,
            MetricsOptions appMetricsOptions,
            MetricsWebTrackingOptions webTrackingOptions,
            MetricEndpointsOptions endpointsOptions,
            IFilterMetrics filter = null)
        {
            services.AddLogging().AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvcCore().AddMetricsCore();

            var builder = new MetricsBuilder().Configuration.Configure(
                                                   options =>
                                                   {
                                                       options.DefaultContextLabel = appMetricsOptions.DefaultContextLabel;
                                                       options.Enabled = appMetricsOptions.Enabled;
                                                   })
                                               .OutputEnvInfo.AsPlainText()
                                               .OutputEnvInfo.AsJson()
                                               .OutputMetrics.AsJson()
                                               .OutputMetrics.AsPlainText()
                                               .SampleWith.AlgorithmR(1028)
                                               .TimeWith.Clock<TestClock>();

            if (filter != null)
            {
                builder.Filter.With(filter);
            }

            services.AddMetrics(builder);

            services.AddMetricsEndpoints(
                options =>
                {
                    options.MetricsTextEndpointEnabled = endpointsOptions.MetricsTextEndpointEnabled;
                    options.MetricsEndpointEnabled = endpointsOptions.MetricsEndpointEnabled;
                });

            services.AddMetricsTrackingMiddleware(
                options =>
                {
                    options.OAuth2TrackingEnabled = webTrackingOptions.OAuth2TrackingEnabled;
                    options.IgnoredRoutesRegexPatterns = webTrackingOptions.IgnoredRoutesRegexPatterns;
                    options.IgnoredHttpStatusCodes = webTrackingOptions.IgnoredHttpStatusCodes;
                });
        }
    }
}