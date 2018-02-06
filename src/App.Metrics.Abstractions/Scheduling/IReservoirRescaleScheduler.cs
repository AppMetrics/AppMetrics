// <copyright file="IReservoirRescaleScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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