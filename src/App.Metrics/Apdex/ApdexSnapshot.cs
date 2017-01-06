// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Linq;

namespace App.Metrics.Apdex
{
    public class ApdexSnapshot
    {
        private static readonly double ApdexTimeUnitFactor = TimeUnit.Seconds.ScalingFactorFor(TimeUnit.Nanoseconds);
        private readonly double _apdexTNanoseconds;
        private readonly IEnumerable<long> _samples;

        public ApdexSnapshot(IEnumerable<long> samples, double apdexTSeconds)
        {
            _samples = samples;
            _apdexTNanoseconds = apdexTSeconds * ApdexTimeUnitFactor;
        }

        public int FrustratingSize
        {
            get { return _samples.Count(t => t > 4.0 * _apdexTNanoseconds); }
        }

        public int SatisfiedSize
        {
            get { return _samples.Count(t => t <= _apdexTNanoseconds); }
        }

        public int ToleratingSize
        {
            get { return _samples.Count(t => t > _apdexTNanoseconds && t <= 4.0 * _apdexTNanoseconds); }
        }
    }
}