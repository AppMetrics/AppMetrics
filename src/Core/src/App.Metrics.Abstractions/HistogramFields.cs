// <copyright file="HistogramFields.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public enum HistogramFields
    {
#pragma warning disable SA1602
        Samples,
        LastValue,
        Count,
        Sum,
        Min,
        Max,
        Mean,
        Median,
        StdDev,
        P999,
        P99,
        P98,
        P95,
        P75,
        UserLastValue,
        UserMinValue,
        UserMaxValue
#pragma warning restore SA1602
    }
}
