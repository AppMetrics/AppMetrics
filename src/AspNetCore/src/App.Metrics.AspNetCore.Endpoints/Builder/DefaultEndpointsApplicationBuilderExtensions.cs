// <copyright file="DefaultEndpointsApplicationBuilderExtensions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Extension methods for <see cref="IApplicationBuilder" /> to add App Metrics Hosting to the request execution pipeline.
    /// </summary>
    public static class DefaultEndpointsApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds all available App Metrics endpoint middleware to the <see cref="IApplicationBuilder" /> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseMetricsAllEndpoints(this IApplicationBuilder app)
        {
            app.UseMetricsEndpoint();
            app.UseMetricsTextEndpoint();
            app.UseEnvInfoEndpoint();

            return app;
        }
    }
}
