// <copyright file="ApdexValueDataKeys.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
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