// <copyright file="AppBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Sandbox.JustForTesting;
using Microsoft.AspNetCore.Hosting;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseTestStuff(
            this IApplicationBuilder app,
            IApplicationLifetime lifetime,
            bool runSampleRequestsFromApp)
        {
            app.Use(
                (context, next) =>
                {
                    RandomClientIdForTesting.SetTheFakeClaimsPrincipal(context);
                    return next();
                });

            if (runSampleRequestsFromApp)
            {
                SampleRequests.Run(lifetime.ApplicationStopping);
            }

            return app;
        }
    }
}
