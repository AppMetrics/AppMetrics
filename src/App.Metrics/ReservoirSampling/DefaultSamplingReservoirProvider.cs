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
        public DefaultSamplingReservoirProvider() { Instance = () => new DefaultForwardDecayingReservoir(); }

        public DefaultSamplingReservoirProvider(Func<IReservoir> instance) { Instance = instance; }

        public Func<IReservoir> Instance { get; }
    }
}