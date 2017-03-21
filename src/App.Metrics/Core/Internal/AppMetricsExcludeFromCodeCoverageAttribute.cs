// <copyright file="AppMetricsExcludeFromCodeCoverageAttribute.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Core.Internal
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    internal sealed class AppMetricsExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}