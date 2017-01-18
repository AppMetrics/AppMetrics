// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core;
using App.Metrics.Core.Interfaces;
using App.Metrics.Interfaces;

namespace App.Metrics.Internal.Builders
{
    public class DefaultCounterBuilder : IBuildCounterMetrics
    {
        /// <inheritdoc />
        public ICounterMetric Instance() { return new CounterMetric(); }
    }
}