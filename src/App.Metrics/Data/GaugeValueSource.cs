// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using App.Metrics.Data.Interfaces;

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
namespace App.Metrics.Data
{
    /// <summary>
    ///     Combines the value of a gauge (a double) with the defined unit for the value.
    /// </summary>
    public sealed class GaugeValueSource : MetricValueSource<double>
    {
        public GaugeValueSource(string name, IMetricValueProvider<double> value, Unit unit, MetricTags tags)
            : base(name, value, unit, tags) { }
    }
}