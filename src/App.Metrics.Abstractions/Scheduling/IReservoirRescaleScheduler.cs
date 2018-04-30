// <copyright file="IReservoirRescaleScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;
using App.Metrics.ReservoirSampling;

namespace App.Metrics.Scheduling
{
    public interface IReservoirRescaleScheduler : IDisposable
    {
        void RemoveSchedule(IRescalingReservoir reservoir);

        void ScheduleReScaling(IRescalingReservoir reservoir);
    }
}