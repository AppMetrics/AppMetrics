// <copyright file="AppMetricsReportingAppMetricsBuilderExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.Configuration;
using App.Metrics.Core.Internal.NoOp;
using App.Metrics.Reporting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable CheckNamespace
namespace App.Metrics.Builder
    // ReSharper restore CheckNamespace
{
    public static class AppMetricsReportingAppMetricsBuilderExtensions
    {
        public static IAppMetricsBuilder AddReporting(this IAppMetricsBuilder builder, Action<IReportFactory> setupAction)
        {
            builder.Services.Replace(
                ServiceDescriptor.Singleton<IReportFactory>(
                    provider =>
                    {
                        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                        var metrics = provider.GetRequiredService<IMetrics>();
                        var options = provider.GetRequiredService<AppMetricsOptions>();

                        if (!options.ReportingEnabled || setupAction == null)
                        {
                            return new NoOpReportFactory();
                        }

                        var factory = new ReportFactory(metrics, loggerFactory);
                        setupAction.Invoke(factory);
                        return factory;
                    }));

            return builder;
        }
    }
}
