// <copyright file="Startup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ReportingSandbox
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class Startup
        // ReSharper restore ClassNeverInstantiated.Global
    {
        private static readonly ILogger Logger = Log.ForContext<Startup>();
        
        // ReSharper disable UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
            // ReSharper restore UnusedMember.Global
        {
            services.AddMetricsReportingHostedService();
            services.AddMetrics(Program.Metrics);
        }

        // ReSharper disable UnusedMember.Global
        public void Configure(IApplicationBuilder app)
            // ReSharper restore UnusedMember.Global
        {
            var fileName = EnsureMetricsDumpFile();

            app.Run(
                async context =>
                {
                    if (context.Request.Method == "POST" && context.Request.Path == "/metrics-receive")
                    {
                        var req = context.Request;
                        req.EnableBuffering();

                        try
                        {
                            using (var sr = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                            {
                                await File.WriteAllTextAsync(fileName, await sr.ReadToEndAsync());
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, "metrics write failed");
                        }

                        req.Body.Position = 0;

                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync("dumped metrics");
                    }
                });
        }

        private static string EnsureMetricsDumpFile()
        {
            const string fileName = @"C:\metrics\http_received.txt";
            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            if (!fileInfo.Exists)
            {
                File.Create(fileName);
            }
            
            return fileName;
        }
    }
}