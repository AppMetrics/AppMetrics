// <copyright file="Startup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ReportingSandbox
{
    // ReSharper disable ClassNeverInstantiated.Global
    public class Startup
        // ReSharper restore ClassNeverInstantiated.Global
    {
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
                        req.EnableRewind();

                        using (var sr = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                        {
#if NET461
                            File.WriteAllText(fileName, sr.ReadToEnd());
#else
                            await File.WriteAllTextAsync(fileName, sr.ReadToEnd());
#endif
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
            var file = new FileInfo(fileName);
            file.Directory?.Create();
            return fileName;
        }
    }
}