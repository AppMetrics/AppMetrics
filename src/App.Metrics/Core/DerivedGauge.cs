// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable SA1515
// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
#pragma warning disable SA1515// Original repo: https://github.com/etishor/Metrics.NET

using System;
using App.Metrics.Core.Interfaces;
using App.Metrics.Data.Interfaces;

namespace App.Metrics.Core
{
    public sealed class DerivedGauge : IGaugeMetric
    {
        private readonly IMetricValueProvider<double> _gauge;
        private readonly Func<double, double> _transformation;

        public DerivedGauge(IMetricValueProvider<double> gauge, Func<double, double> transformation)
        {
            _gauge = gauge;
            _transformation = transformation;
        }

        /// <inheritdoc />
        public double Value
        {
            get
            {
                try
                {
                    return _transformation(_gauge.Value);
                }
                catch (Exception)
                {
                    return double.NaN;
                }
            }
        }

        /// <inheritdoc />
        public double GetValue(bool resetMetric = false) { return Value; }
    }
}