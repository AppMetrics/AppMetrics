// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Core.Options;
using App.Metrics.Interfaces;
using App.Metrics.Internal.Interfaces;

namespace App.Metrics.Internal.Managers
{
    internal class DefaultApdexManager : IMeasureApdexMetrics
    {
        private readonly IAdvancedMetrics _advanced;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultApdexManager" /> class.
        /// </summary>
        /// <param name="advanced">The advanced metrics manager.</param>
        /// <param name="registry">The registry storing all metric data.</param>
        public DefaultApdexManager(IAdvancedMetrics advanced, IMetricsRegistry registry)
        {
            _registry = registry;
            _advanced = advanced;
        }

        /// <inheritdoc />
        public void Track(ApdexOptions options, Action action)
        {
            using (_registry.Apdex(options, () => _advanced.BuildApdex(options)).NewContext())
            {
                action();
            }
        }

        /// <inheritdoc />
        public ApdexContext Track(ApdexOptions options) { return _registry.Apdex(options, () => _advanced.BuildApdex(options)).NewContext(); }
    }
}