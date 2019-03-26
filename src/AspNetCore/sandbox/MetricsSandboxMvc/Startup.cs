// <copyright file="Startup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetricsSandboxMvc
{
    public class Startup
    {
        private const bool HaveAppRunSampleRequests = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            app.UseTestStuff(lifetime, HaveAppRunSampleRequests);

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddTestStuff();

            services.AddApiVersioning(
                options =>
                {
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                    options.ReportApiVersions = true;
                });

            services.AddMvcCore(options =>
                    {
                        options.EnableEndpointRouting = true;
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddApiExplorer()
                    .AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V")
                    .AddMetricsCore();
        }
    }
}