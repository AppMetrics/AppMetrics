// <copyright file="DefaultApdexManager.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Apdex.Abstractions;
using App.Metrics.Core.Options;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Tagging;

namespace App.Metrics.Apdex
{
    internal sealed class DefaultApdexManager : IMeasureApdexMetrics
    {
        private readonly IBuildApdexMetrics _apdexBuilder;
        private readonly IClock _clock;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultApdexManager" /> class.
        /// </summary>
        /// <param name="apdexBuilder">The apdex builder.</param>
        /// <param name="registry">The registry storing all metric data.</param>
        /// <param name="clock">The clock.</param>
        public DefaultApdexManager(IBuildApdexMetrics apdexBuilder, IMetricsRegistry registry, IClock clock)
        {
            _registry = registry;
            _clock = clock;
            _apdexBuilder = apdexBuilder;
        }

        /// <inheritdoc />
        public void Track(ApdexOptions options, Action action)
        {
            var apdex = _registry.Apdex(
                options,
                () => _apdexBuilder.Build(options.Reservoir, options.ApdexTSeconds, options.AllowWarmup, _clock));

            using (apdex.NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public void Track(ApdexOptions options, MetricTags tags, Action action)
        {
            var apdex = _registry.Apdex(
                options,
                tags,
                () => _apdexBuilder.Build(options.Reservoir, options.ApdexTSeconds, options.AllowWarmup, _clock));

            using (apdex.NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public ApdexContext Track(ApdexOptions options, MetricTags tags)
        {
            var apdex = _registry.Apdex(
                options,
                tags,
                () => _apdexBuilder.Build(options.Reservoir, options.ApdexTSeconds, options.AllowWarmup, _clock));

            return apdex.NewContext();
        }

        /// <inheritdoc />
        public ApdexContext Track(ApdexOptions options)
        {
            var apdex = _registry.Apdex(
                options,
                () => _apdexBuilder.Build(options.Reservoir, options.ApdexTSeconds, options.AllowWarmup, _clock));

            return apdex.NewContext();
        }
    }
}