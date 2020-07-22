// <copyright file="Host.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using MetricsMicrosoftExtensionsSandbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CollectorsSandbox
{
    public class Host
    {
        public static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureMetrics(
                    (context, builder) =>
                    {
                        builder.Report.Using<SimpleConsoleMetricsReporter>(TimeSpan.FromSeconds(5));
                    })
                .ConfigureServices(services =>
                {
                    services.AddAppMetricsCollectors(options =>
                    {
                        options.CollectIntervalMilliseconds = 10000;
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}