// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.Metrics;
using App.Metrics.Core.Options;

namespace App.Metrics.Apdex.Abstractions
{
    public interface IProvideApdexMetrics
    {
        IApdex Instance(ApdexOptions options);

        IApdex Instance<T>(ApdexOptions options, Func<T> builder)
            where T : IApdexMetric;
    }
}