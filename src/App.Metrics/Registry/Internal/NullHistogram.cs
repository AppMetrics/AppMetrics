// <copyright file="NullHistogram.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.Internal;
using App.Metrics.Histogram.Abstractions;

namespace App.Metrics.Registry.Internal
{
    [AppMetricsExcludeFromCodeCoverage]
    internal struct NullHistogram : IHistogram
    {
        public void Reset() { }

        public void Update(long value, string userValue) { }

        public void Update(long value) { }
    }
}