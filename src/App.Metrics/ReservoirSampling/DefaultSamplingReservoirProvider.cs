// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Abstractions.ReservoirSampling;
using App.Metrics.ReservoirSampling.ExponentialDecay;

namespace App.Metrics.ReservoirSampling
{
    public class DefaultSamplingReservoirProvider
    {
        public DefaultSamplingReservoirProvider() { Instance = () => new Lazy<IReservoir>(() => new DefaultForwardDecayingReservoir()); }

        public DefaultSamplingReservoirProvider(Func<Lazy<IReservoir>> instance) { Instance = instance; }

        public Func<Lazy<IReservoir>> Instance { get; }
    }
}