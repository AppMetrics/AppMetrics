using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using App.Metrics;
using App.Metrics.Data;
using App.Metrics.Extensions.Reporting.TextFile;
using App.Metrics.Reporting;
using Microsoft.AspNet.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using App.Metrics.Formatters.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;
using App.Metrics.DependencyInjection;
using App.Metrics.Internal;

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
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //TODO: AH - Should all metrics options be passed in here or in the service collection extensions?
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
            reporter.RunReports(metrics, CancellationToken.None);

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
                .AddMetrics(options =>
                {
                    options.DefaultSamplingType = SamplingType.ExponentiallyDecaying;
                    options.DisableMetrics = false;
                })
                //.AddGlobalFilter(new DefaultMetricsFilter().WhereType(MetricType.Counter))
                .AddJsonSerialization()
                .AddReporting(options =>
                {
                    options.Reporters = factory =>
                    {
                        var textFileSettings = new TextFileReporterSettings
                        {
                            ReportInterval = TimeSpan.FromSeconds(30),
                            FileName = @"C:\metrics\aspnet-sample.txt"
                        };

                        factory.AddTextFile(textFileSettings);
                    };
                })
                .AddHealthChecks()
                .AddAspNetMetrics(options => { });
            //.WithAllPerformanceCounters()
        }
    }
}