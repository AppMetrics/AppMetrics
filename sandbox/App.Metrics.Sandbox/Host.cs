// <copyright file="Host.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Reporting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace App.Metrics.Sandbox
{
    public static class Host
    {
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseMetricsDefaults()
                   .UseMetricsReporting(factory =>
                   {
                       factory.AddInfluxReporting();
                       factory.AddElasticSearchReporting();
                       factory.AddGraphiteReporting();
                   })
                   .UseStartup<Startup>()
                   .Build();

        public static void Main(string[] args) { BuildWebHost(args).Run(); }
    }
}