// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Metrics.Internal.Managers
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
            _logger.LogDebug("Disabling metrics");

            if (_registry is NullMetricsRegistry)
            {
                return;
            }

            _registry.Disable();

            _logger.LogDebug("Metrics disabled");
        }

        /// <inheritdoc />
        public void Reset()
        {
            _logger.LogDebug("Clearing metrics registry");

            _registry.Clear();

            _logger.LogDebug("Metrics registry cleared");
        }

        /// <inheritdoc />
        public void ShutdownContext(string context)
        {
            _logger.LogDebug("Shutting down metrics context");

            _registry.RemoveContext(context);

            _logger.LogDebug("Metrics Context shutdown");
        }
    }
}