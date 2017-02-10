// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;
using App.Metrics.Registry.Abstractions;
using App.Metrics.Registry.Internal;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Core.Internal
{
    internal sealed class DefaultMetricsManager : IManageMetrics
    {
        private readonly ILogger<DefaultMetricsManager> _logger;
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricsManager" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <param name="logger">The logger.</param>
        public DefaultMetricsManager(IMetricsRegistry registry, ILogger<DefaultMetricsManager> logger)
        {
            _registry = registry ?? new NullMetricsRegistry();
            _logger = logger;
        }

        /// <inheritdoc />
        public void Disable()
        {
            _logger.LogTrace("Disabling metrics");

            _registry.Disable();

            _logger.LogTrace("Metrics disabled");
        }

        /// <inheritdoc />
        public void Reset()
        {
            _logger.LogTrace("Clearing metrics registry");

            _registry.Clear();

            _logger.LogTrace("Metrics registry cleared");
        }

        /// <inheritdoc />
        public void ShutdownContext(string context)
        {
            _logger.LogTrace("Shutting down metrics context");

            _registry.RemoveContext(context);

            _logger.LogTrace("Metrics Context shutdown");
        }
    }
}