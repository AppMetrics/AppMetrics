// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System;
using App.Metrics.MetricData;
using App.Metrics.Sampling;
using App.Metrics.Utils;

namespace App.Metrics.Core
{
    public sealed class DefaultMetricsBuilder : IMetricsBuilder
    {
        private readonly IClock _systemClock;
        private bool _disposed = false;

        public DefaultMetricsBuilder(IClock systemClock)
        {
            if (systemClock == null)
            {
                throw new ArgumentNullException(nameof(systemClock));
            }

            _systemClock = systemClock;
        }

        ~DefaultMetricsBuilder()
        {
            Dispose(false);
        }

        public ICounterMetric BuildCounter(string name, Unit unit)
        {
            return new CounterMetric();
        }

        public IMetricValueProvider<double> BuildGauge(string name, Unit unit, Func<double> valueProvider)
        {
            return new FunctionGauge(valueProvider);
        }

        public IHistogramMetric BuildHistogram(string name, Unit unit, SamplingType samplingType)
        {
            return new HistogramMetric(samplingType);
        }

        public IHistogramMetric BuildHistogram(string name, Unit unit, IReservoir reservoir)
        {
            return new HistogramMetric(reservoir);
        }

        public IMeterMetric BuildMeter(string name, Unit unit, TimeUnit rateUnit)
        {
            return new MeterMetric(_systemClock);
        }

        public ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, SamplingType samplingType)
        {
            return new TimerMetric(samplingType, _systemClock);
        }

        public ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IHistogramMetric histogram)
        {
            return new TimerMetric(histogram, _systemClock);
        }

        public ITimerMetric BuildTimer(string name, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, IReservoir reservoir)
        {
            return new TimerMetric(reservoir, _systemClock);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
            }

            _disposed = true;
        }
    }
}