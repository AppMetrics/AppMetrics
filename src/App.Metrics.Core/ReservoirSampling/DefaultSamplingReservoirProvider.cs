// <copyright file="DefaultSamplingReservoirProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Core.ReservoirSampling.ExponentialDecay;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Core.ReservoirSampling
{
    public class DefaultSamplingReservoirProvider
    {
        public DefaultSamplingReservoirProvider() { Instance = () => new DefaultForwardDecayingReservoir(); }

        public DefaultSamplingReservoirProvider(Func<IReservoir> instance) { Instance = instance; }

        public Func<IReservoir> Instance { get; }
    }
}