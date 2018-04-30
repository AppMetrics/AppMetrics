// <copyright file="ApdexValueDataKeys.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics
{
    public enum ApdexValueDataKeys
    {
#pragma warning disable SA1602
        Samples,
        Score,
        Satisfied,
        Tolerating,
        Frustrating
#pragma warning restore SA1602
    }
}