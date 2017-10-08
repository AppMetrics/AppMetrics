// <copyright file="DefaultMetricsManager.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Internal.NoOp;
using App.Metrics.Logging;
using App.Metrics.Registry;

namespace App.Metrics.Internal
{
    public sealed class DefaultMetricsManager : IManageMetrics
    {
        private static readonly ILog Logger = LogProvider.For<DefaultMetricsManager>();
        private readonly IMetricsRegistry _registry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultMetricsManager" /> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        public DefaultMetricsManager(IMetricsRegistry registry)
        {
            _registry = registry ?? new NullMetricsRegistry();
        }

        /// <inheritdoc />
        public void Disable()
        {
            Logger.Debug("Disabling metrics");

            _registry.Disable();

            Logger.Debug("Metrics disabled");
        }

        /// <inheritdoc />
        public void Reset()
        {
            Logger.Debug("Clearing metrics registry");

            _registry.Clear();

            Logger.Debug("Metrics registry cleared");
        }

        /// <inheritdoc />
        public void ShutdownContext(string context)
        {
            Logger.Debug("Shutting down metrics context");

            _registry.RemoveContext(context);

            Logger.Debug("Metrics Context shutdown");
        }
    }
}