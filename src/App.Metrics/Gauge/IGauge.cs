// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Abstractions;

namespace App.Metrics.Gauge
{
    public interface IGauge : IResetableMetric
    {
        void SetValue(double value);
    }
}