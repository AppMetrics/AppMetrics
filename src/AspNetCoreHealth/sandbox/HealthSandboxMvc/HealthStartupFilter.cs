// <copyright file="HealthStartupFilter.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace HealthSandboxMvc
{
    public class HealthStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return AddHealth;

            void AddHealth(IApplicationBuilder builder)
            {
                builder.UsePingEndpoint();
                builder.UseHealthEndpoint();

                next(builder);
            }
        }
    }
}