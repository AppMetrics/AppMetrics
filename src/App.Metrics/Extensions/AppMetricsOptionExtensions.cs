// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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