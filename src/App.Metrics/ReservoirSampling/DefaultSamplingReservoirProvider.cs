// <copyright file="DefaultSamplingReservoirProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

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