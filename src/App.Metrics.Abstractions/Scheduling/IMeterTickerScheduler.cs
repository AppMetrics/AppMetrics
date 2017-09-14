// <copyright file="IMeterTickerScheduler.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.Meter;

namespace App.Metrics.Scheduling
{
    public interface IMeterTickerScheduler : IDisposable
    {
        void ScheduleTick(ITickingMeter meter);

        void RemoveSchedule(ITickingMeter meter);
    }
}