// <copyright file="MeterValueDataKeys.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public enum MeterValueDataKeys
    {
#pragma warning disable SA1602
        Count,
        Rate1M,
        Rate5M,
        Rate15M,
        RateMean,
        SetItemPercent,
        MetricSetItemSuffix
#pragma warning restore SA1602
    }
}