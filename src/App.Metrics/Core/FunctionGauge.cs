// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable SA1515
// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy
#pragma warning disable SA1515// Original repo: https://github.com/etishor/Metrics.NET

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

        public FunctionGauge(Func<double> valueProvider) { _valueProvider = valueProvider; }

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
        public double GetValue(bool resetMetric = false) { return Value; }
    }
}