// <copyright file="IRescalingReservoir.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.ReservoirSampling
{
    public interface IRescalingReservoir : IReservoir
    {
        void Rescale();
    }
}
