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
using Microsoft.Extensions.Hosting;

namespace MetricsSandboxMvc
{
    public class Startup
    {
        private const bool HaveAppRunSampleRequests = true;

        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseTestStuff(lifetime, HaveAppRunSampleRequests);

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
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

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'V");

            services.AddMvcCore(options =>
                    {
                        options.EnableEndpointRouting = true;
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddApiExplorer()
                    .AddMetricsCore();
        }
    }
}