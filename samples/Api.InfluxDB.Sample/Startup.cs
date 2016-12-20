using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using Api.InfluxDB.Sample.ForTesting;
using App.Metrics;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Extensions.Reporting.InfluxDB;
using App.Metrics.Reporting.Interfaces;
using App.Metrics.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Api.InfluxDB.Sample
{
    public class Startup
    {
        static readonly Random Random = new Random();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine($@"C:\logs\{env.ApplicationName}", "log-{Date}.txt"))
                .CreateLogger();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog(Log.Logger);

            app.UseMetrics();           

            var reportFactory = app.ApplicationServices.GetRequiredService<IReportFactory>();
            var metrics = app.ApplicationServices.GetRequiredService<IMetrics>();
            var reporter = reportFactory.CreateReporter();
            reporter.RunReportsAsync(metrics, CancellationToken.None);

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services
                .AddMetrics(Configuration.GetSection("AppMetrics"), options => options.GlobalTags.Add("app", "sample app"))
                .AddClockType<TestClock>()
                .AddJsonSerialization()
                .AddReporting(factory =>
                {
                    var influxFlushFilter = new DefaultMetricsFilter()
                        .WhereMetricNameStartsWith("GET api/apdex")                        
                        .WithEnvironmentInfo(false)
                        .WithHealthChecks(false);

                    factory.AddInfluxDb(new InfluxDbReporterSettings
                    {
                        BaseAddress = "http://127.0.0.1:8086",
                        Database = "appmetricsinfluxsample",
                        ReportInterval = TimeSpan.FromSeconds(5)
                    }, filter:influxFlushFilter);
                })
                .AddHealthChecks()
                .AddMetricsMiddleware(Configuration.GetSection("AspNetMetrics"));

            services.AddTransient<Func<double, RequestDurationForApdexTesting>>(
                provider => { return apdexTSeconds => new RequestDurationForApdexTesting(apdexTSeconds); });

            services.AddTransient(provider =>
            {
                var options = provider.GetRequiredService<AspNetMetricsOptions>();
                return new RequestDurationForApdexTesting(options.ApdexTSeconds);
            });
        }
    }
}