using System;
using App.Metrics;
using App.Metrics.Formatters.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MetricsSandboxCoreConfigServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricsAllMiddleware();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseMetricsAllEndpoints();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var metrics = AppMetrics.CreateDefaultBuilder().OutputMetrics.AsJson().OutputMetrics.AsPlainText().Build();
            
            services.AddMetrics(metrics)
                    .AddMetricsTrackingMiddleware()
                    .AddMetricsEndpoints();
            
            services.AddControllers()
                    .AddMetrics();
        }
    }
}