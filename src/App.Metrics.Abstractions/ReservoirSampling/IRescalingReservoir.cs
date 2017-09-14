// <copyright file="IRescalingReservoir.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.ReservoirSampling
{
    public interface IRescalingReservoir : IReservoir
    {
        void Rescale();
    }
}
