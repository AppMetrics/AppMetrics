// <copyright file="MetricsCoreMetricsOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Infrastructure;
using Microsoft.Extensions.Options;

namespace App.Metrics.Internal
{
    /// <summary>
    ///     Sets up default options for <see cref="T:App.Metrics.MetricsOptions" />.
    /// </summary>
    public class MetricsCoreMetricsOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly EnvironmentInfo _environmentInfo;

        public MetricsCoreMetricsOptionsSetup(EnvironmentInfoProvider environmentInfoProvider)
        {
            _environmentInfo = environmentInfoProvider.Build();
        }

        /// <inheritdoc />
        public void Configure(MetricsOptions options)
        {
            if (options.AddDefaultGlobalTags)
            {
                AddDefaultTags(options);
            }
        }

        private void AddDefaultTags(MetricsOptions options)
        {
            if (!options.GlobalTags.ContainsKey("app"))
            {
                options.GlobalTags.Add("app", _environmentInfo.EntryAssemblyName);
            }

            if (!options.GlobalTags.ContainsKey("server"))
            {
                options.GlobalTags.Add("server", _environmentInfo.MachineName);
            }

            if (!options.GlobalTags.ContainsKey("env"))
            {
#if DEBUG
                options.GlobalTags.Add("env", "debug");
#else
                options.GlobalTags.Add("env", "release");
#endif
            }
        }
    }
}