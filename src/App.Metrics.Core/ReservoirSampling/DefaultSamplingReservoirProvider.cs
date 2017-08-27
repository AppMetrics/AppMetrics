// <copyright file="DefaultSamplingReservoirProvider.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.ReservoirSampling
{
    public class DefaultSamplingReservoirProvider
    {
        public DefaultSamplingReservoirProvider(Func<IReservoir> instance) { Instance = instance; }

        public Func<IReservoir> Instance { get; }
    }
}