// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Apdex.Interfaces
{
    public interface IBuildApdexMetrics
    {
        IApdexMetric Build(double apdexTSeconds, bool allowWarmup, IClock clock);

        IApdexMetric Build(Lazy<IReservoir> reservoir, double apdexTSeconds, bool allowWarmup, IClock clock);
    }
}