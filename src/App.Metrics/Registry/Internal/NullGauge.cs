// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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