// <copyright file="Startup.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthSandboxMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app) { app.UseMvc(); }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // To configure App Metrics Health Host options in Startup
            // services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<HealthEndpointsHostingOptions>, ConfigureHealthHostingOptions>());
        }

        // To configure App Metrics Health Host options using IConfigureOptions in Startup
        // public class ConfigureHealthHostingOptions : IConfigureOptions<HealthEndpointsHostingOptions>
        // {
        //     public void Configure(HealthEndpointsHostingOptions options) { options.HealthEndpoint = "/ucstom_health_check_url"; }
        // }
    }
}