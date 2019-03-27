// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health;
using App.Metrics.Health;
using App.Metrics.Health.Formatters.Ascii;
using App.Metrics.Health.Reporting.Slack;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace HealthSandboxMvc
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            return WebHost.CreateDefaultBuilder(args)

                   #region App Metrics Health configuration options

                   // To configure ASP.NET core App Metrics hosting options: custom ports and endpoints
                   // .ConfigureAppHealthHostingConfiguration(
                   //  options =>
                   //  {
                   //      options.HealthEndpointPort = 2222;
                   //      options.HealthEndpoint = "healthchecks";
                   //  })

                   // To configure App Metrics Health core where an IHealthBuilder is provided with defaults that can be overriden
                   // .ConfigureHealthWithDefaults(
                   //    builder =>
                   //    {
                   //        builder.Configuration.Configure(
                   //            options =>
                   //            {
                   //                options.Enabled = true;
                   //            });
                   //    })

                   // To configure App Metrics core where an IMetricsBuilder is provided without any defaults set
                   // .ConfigureHealth(
                   //    builder =>
                   //    {
                   //        builder.Configuration.Configure(
                   //            options =>
                   //            {
                   //                options.Enabled = true;
                   //            });
                   //    })

                   // To configure App Metrics Health endpoints explicity
                   // .UseHealthEndpoints()

                   // To configure all App Metrics Health Asp.Net Core extensions overriding defaults
                   // .UseHealth(Configure())

                   // To confgiure all App Metrics Asp.Net core extensions using a custom startup filter providing more control over what middleware tracking is added to the request pipeline for example
                   // .UseHealth<DefaultHealthStartupFilter>()

                   #endregion

                   .ConfigureHealthWithDefaults(
                       (context, builder) =>
                       {
                           var slackOptions = new SlackHealthAlertOptions();
                           context.Configuration.GetSection(nameof(SlackHealthAlertOptions)).Bind(slackOptions);

                           builder.HealthChecks.AddProcessPrivateMemorySizeCheck("Private Memory Size", 200);
                           builder.HealthChecks.AddProcessVirtualMemorySizeCheck("Virtual Memory Size", 200);
                           builder.HealthChecks.AddProcessPhysicalMemoryCheck("Working Set", 200);
                           builder.HealthChecks.AddPingCheck("google ping", "google.com", TimeSpan.FromSeconds(10));
                           builder.HealthChecks.AddHttpGetCheck("github", new Uri("https://github.com/"), TimeSpan.FromSeconds(10));
                           builder.Report.ToSlack(slackOptions); // TODO: Edit webhook url in appsettings
                       })
                    .UseHealth()
                    .UseStartup<Startup>()
                    .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        // ReSharper disable UnusedMember.Local - .UseHealth(Configure())
        private static Action<HealthWebHostOptions> Configure()
            // ReSharper restore UnusedMember.Local
        {
            return options =>
            {
                options.EndpointOptions = endpointsOptions =>
                {
                    endpointsOptions.HealthEndpointOutputFormatter = new HealthStatusTextOutputFormatter();
                };
            };
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .WriteTo.LiterateConsole()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();
        }
    }
}