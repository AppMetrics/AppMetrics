// <copyright file="AppMetricsOptionExtensionsTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Infrastructure;
using App.Metrics.Tagging;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class AppMetricsOptionExtensionsTests
    {
        [Fact]
        public void Can_set_global_tags_on_metric_options()
        {
            var environmentBuilder = new EnvironmentInfoProvider();
            var environmentInfo = environmentBuilder.Build();
            var expected = new GlobalMetricTags(
                new Dictionary<string, string>
                {
                    { "machine_name", environmentInfo.MachineName },
                    { "app_name", environmentInfo.EntryAssemblyName },
                    { "app_version", environmentInfo.EntryAssemblyVersion }
                });
            var options = new MetricsOptions();

            options.WithGlobalTags(
                (globalTags, envInfo) =>
                {
                    globalTags.Add("machine_name", envInfo.MachineName);
                    globalTags.Add("app_name", envInfo.EntryAssemblyName);
                    globalTags.Add("app_version", envInfo.EntryAssemblyVersion);
                });

            options.GlobalTags.Should().Equal(expected);
        }
    }
}