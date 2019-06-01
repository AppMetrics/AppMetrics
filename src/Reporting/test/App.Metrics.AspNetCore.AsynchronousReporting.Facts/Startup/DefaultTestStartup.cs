using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddMetrics(builder);
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