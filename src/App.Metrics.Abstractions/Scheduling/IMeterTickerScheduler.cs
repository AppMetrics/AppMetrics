// <copyright file="IMeterTickerScheduler.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
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