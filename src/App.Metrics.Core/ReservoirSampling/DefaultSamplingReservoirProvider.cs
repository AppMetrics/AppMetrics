// <copyright file="DefaultSamplingReservoirProvider.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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