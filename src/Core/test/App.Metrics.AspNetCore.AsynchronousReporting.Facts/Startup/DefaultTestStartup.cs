using System;
using App.Metrics.Infrastructure;
using App.Metrics.Internal.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace App.Metrics.AspNetCore.AsynchronousReporting.Facts.Startup
{
    public abstract class DefaultTestStartup
    {
        private readonly Action<IMetricsOutputFormattingBuilder> _outputMetricsConfigure;

        protected DefaultTestStartup(Action<IMetricsOutputFormattingBuilder> outputMetricsConfigure)
        {
            _outputMetricsConfigure = outputMetricsConfigure ?? throw new ArgumentNullException(nameof(outputMetricsConfigure));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => { options.LowercaseUrls = true; });
            services.AddControllers();
            
            var builder = new MetricsBuilder();
            _outputMetricsConfigure(builder.OutputMetrics);

            var metrics = builder.Build();
            services.TryAddSingleton(metrics.Clock);
            services.TryAddSingleton(metrics.Filter);
            services.TryAddSingleton(metrics.DefaultOutputMetricsFormatter);
            services.TryAddSingleton(metrics.OutputMetricsFormatters);
            services.TryAddSingleton(metrics.DefaultOutputEnvFormatter);
            services.TryAddSingleton(metrics.OutputEnvFormatters);
            services.TryAddSingleton(new EnvironmentInfoProvider());
            services.TryAddSingleton((IMetrics) metrics);
            services.TryAddSingleton(metrics);
            services.TryAddSingleton(metrics.Options);
            services.TryAddSingleton(metrics.Reporters);
            services.TryAddSingleton(metrics.ReportRunner);
            services.TryAddSingleton<AppMetricsMarkerService, AppMetricsMarkerService>();
            
            services.AddMetricsEndpoints();
        }
        
        // ReSharper disable UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
            // ReSharper restore UnusedMember.Global
        {
            app.UseRouting();
            app.UseMetricsEndpoint();
            app.UseMetricsAllMiddleware();
        }
    } 
}