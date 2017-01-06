// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


//Written by Iulian Margarintescu and will retain the same license as the Java Version
//Original .NET Source by Iulian Margarintescu: https://github.com/etishor/Metrics.NET/tree/master/Src
//Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained


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
        public double GetValue(bool resetMetric = false)
        {
            return Value;
        }
    }
}