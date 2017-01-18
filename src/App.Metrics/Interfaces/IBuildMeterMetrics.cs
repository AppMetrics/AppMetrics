// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Core.Interfaces;
using App.Metrics.Utils;

namespace App.Metrics.Interfaces
{
    public interface IBuildMeterMetrics
    {
        IMeterMetric Build(IClock clock);
    }
}