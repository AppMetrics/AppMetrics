// <copyright file="AppMetricsOptionExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using App.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace App.Metrics.Configuration
{
    // ReSharper restore CheckNamespace
    public static class AppMetricsOptionExtensions
    {
        public static AppMetricsOptions WithGlobalTags(
            this AppMetricsOptions options,
            Action<Dictionary<string, string>, EnvironmentInfo> setupAction)
        {
            var environmentBuilder = new EnvironmentInfoProvider();
            var environmentInfo = environmentBuilder.Build();

            setupAction(options.GlobalTags, environmentInfo);

            return options;
        }
    }
}