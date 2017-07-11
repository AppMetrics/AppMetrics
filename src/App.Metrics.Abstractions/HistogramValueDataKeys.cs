// <copyright file="HistogramValueDataKeys.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public enum HistogramValueDataKeys
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
