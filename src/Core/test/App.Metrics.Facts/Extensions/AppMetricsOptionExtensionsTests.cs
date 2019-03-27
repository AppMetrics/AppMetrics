// <copyright file="AppMetricsOptionExtensionsTests.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System.Collections.Generic;
using App.Metrics.Infrastructure;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class AppMetricsOptionExtensionsTests
    {
        [Theory]
        [InlineData("testApp")]
        [InlineData(null)]
        public void Can_set_application_name_tag(string appName)
        {
            // Arrange
            var options = new MetricsOptions();

            // Act
            options.AddAppTag(appName);

            // Assert
            if (appName == null)
            {
                options.GlobalTags["app"].Should().NotBeNullOrEmpty();
            }
            else
            {
                options.GlobalTags["app"].Should().Be(appName);
            }
        }

        [Theory]
        [InlineData("production")]
        [InlineData(null)]
        public void Can_set_environment_name_tag(string envName)
        {
            // Arrange
            var options = new MetricsOptions();

            // Act
            options.AddEnvTag(envName);

            // Assert
            if (envName == null)
            {
                options.GlobalTags["env"].Should().NotBeNullOrEmpty();
            }
            else
            {
                options.GlobalTags["env"].Should().Be(envName);
            }
        }

        [Fact]
        public void Can_set_global_tags_on_metric_options()
        {
            // Arrange
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

            // Act
            options.WithGlobalTags(
                (globalTags, envInfo) =>
                {
                    globalTags.Add("machine_name", envInfo.MachineName);
                    globalTags.Add("app_name", envInfo.EntryAssemblyName);
                    globalTags.Add("app_version", envInfo.EntryAssemblyVersion);
                });

            // Assert
            options.GlobalTags.Should().Equal(expected);
        }

        [Theory]
        [InlineData("serverName")]
        [InlineData(null)]
        public void Can_set_server_name_tag(string serverName)
        {
            // Arrange
            var options = new MetricsOptions();

            // Act
            options.AddServerTag(serverName);

            // Assert
            if (serverName == null)
            {
                options.GlobalTags["server"].Should().NotBeNullOrEmpty();
            }
            else
            {
                options.GlobalTags["server"].Should().Be(serverName);
            }
        }
    }
}