// <copyright file="NullGauge.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Internal;
using App.Metrics.Gauge;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal struct NullGauge : IGauge
    {
        /// <inheritdoc />
        public void Reset()
        {
        }

        /// <inheritdoc />
        public void SetValue(double value)
        {
        }
    }
}