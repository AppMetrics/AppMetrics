using System;
using App.Metrics.Extensions.Middleware.DependencyInjection.Options;
using App.Metrics.Sandbox.JustForTesting;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
    // ReSharper restore CheckNamespace
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTestStuff(this IServiceCollection services)
        {
            services.AddTransient<Func<double, RequestDurationForApdexTesting>>(
                provider => { return apdexTSeconds => new RequestDurationForApdexTesting(apdexTSeconds); });

            services.AddSingleton<RandomValuesForTesting>();

            services.AddTransient(
                provider =>
                {
                    var options = provider.GetRequiredService<AspNetMetricsOptions>();
                    return new RequestDurationForApdexTesting(options.ApdexTSeconds);
                });

            return services;
        }
    }
}