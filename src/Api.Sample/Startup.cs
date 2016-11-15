using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using App.Metrics;
using App.Metrics.Extensions.Reporting.TextFile;
using App.Metrics.Reporting;
using App.Metrics.Reporting.Interfaces;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Api.Sample
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
            loggerFactory.AddSerilog();

            app.UseMetrics();

            // DEVNOTE: Fake a client being authorized to test oauth2 client request rate middleare
            app.Use((context, func) =>
            {
                context.User =
                    new ClaimsPrincipal(new List<ClaimsIdentity>
                    {
                        new ClaimsIdentity(new[]
                        {
                            new Claim("client_id", "client" + Random.Next(1, 10))
                        })
                    });
                return func();
            });

            var reportFactory = app.ApplicationServices.GetRequiredService<IReportFactory>();
            var metrics = app.ApplicationServices.GetRequiredService<IMetrics>();
            var reporter = reportFactory.CreateReporter();
            reporter.RunReportsAsync(metrics, CancellationToken.None);

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            services
                .AddLogging()
                .AddRouting(options => { options.LowercaseUrls = true; });

            services.AddMvc(options => options.AddMetricsResourceFilter());

            services
                .AddMetrics(Configuration.GetSection("AppMetrics"))
                //.AddGlobalFilter(new DefaultMetricsFilter().WhereType(MetricType.Counter))
                .AddJsonSerialization()
                .AddReporting(factory =>
                {
                    var textFileSettings = new TextFileReporterSettings
                    {
                        ReportInterval = TimeSpan.FromSeconds(5),
                        FileName = @"C:\metrics\aspnet-sample.txt"
                    };

                    factory.AddTextFile(textFileSettings);
                })
                .AddHealthChecks()
                .AddAspNetMetrics(Configuration.GetSection("AspNetMetrics"));
            //.WithAllPerformanceCounters()
        }
    }
}