// <copyright file="ServiceCollectionExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Tracking;
using MetricsPrometheusSandboxMvc.JustForTesting;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestStuff(this IServiceCollection services)
        {
            services.AddTransient<Func<double, RequestDurationForApdexTesting>>(
                serviceProvider => { return apdexTSeconds => new RequestDurationForApdexTesting(apdexTSeconds); });

            services.AddSingleton<RandomValuesForTesting>();

            services.AddTransient(
                serviceProvider =>
                {
                    var optionsAccessor = serviceProvider.GetRequiredService<IOptions<MetricsWebTrackingOptions>>();
                    return new RequestDurationForApdexTesting(optionsAccessor.Value.ApdexTSeconds);
                });

            return services;
        }
    }
}