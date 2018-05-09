// <copyright file="MeterFields.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public enum MeterFields
    {
#pragma warning disable SA1602
        Count,
        Rate1M,
        Rate5M,
        Rate15M,
        RateMean,
        SetItem,
        SetItemPercent
#pragma warning restore SA1602
    }
}