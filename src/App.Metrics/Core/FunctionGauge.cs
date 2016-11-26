// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

//Written by Iulian Margarintescu and will retain the same license as the Java Version
//Original .NET Source by Iulian Margarintescu: https://github.com/etishor/Metrics.NET/tree/master/Src
//Ported to a .NET Standard Project by Allan Hardy as the owner Iulian Margarintescu is unreachable and the source and packages are no longer maintained


using System;
using App.Metrics.Core.Interfaces;

namespace App.Metrics.Core
{
    /// <summary>
    ///     A Gauge metric using a function to provide the instantaneous value to record
    /// </summary>
    /// <seealso cref="IGaugeMetric" />
    public class FunctionGauge : IGaugeMetric
    {
        private readonly Func<double> _valueProvider;

        public FunctionGauge(Func<double> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        /// <inheritdoc />
        public double Value
        {
            get
            {
                try
                {
                    return _valueProvider();
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