using System.Collections.Generic;
using App.Metrics.Configuration;
using App.Metrics.Infrastructure;
using App.Metrics.Tagging;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Extensions
{
    public class AppMetricsOptionExtensionsTests
    {
        [Fact]
        public void can_set_global_tags_on_metric_options()
        {
            var environmentBuilder = new EnvironmentInfoProvider();
            var environmentInfo = environmentBuilder.Build();
            var expected = new GlobalMetricTags(new Dictionary<string, string>
            {
                { "host", environmentInfo.HostName },
                { "machine_name", environmentInfo.MachineName },
                { "app_name", environmentInfo.EntryAssemblyName },
                { "app_version", environmentInfo.EntryAssemblyVersion }
            });
            var options = new AppMetricsOptions();

            options.WithGlobalTags((globalTags, envInfo) =>
            {
                globalTags.Add("host", envInfo.HostName);
                globalTags.Add("machine_name", envInfo.MachineName);
                globalTags.Add("app_name", envInfo.EntryAssemblyName);
                globalTags.Add("app_version", envInfo.EntryAssemblyVersion);
            });

            options.GlobalTags.Should().Equal(expected);
        }
    }
}