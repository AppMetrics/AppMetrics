// <copyright file="Constants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Reporting.InfluxDB.Client
{
    internal static class Constants
    {
        public static readonly TimeSpan DefaultBackoffPeriod = TimeSpan.FromSeconds(30);
        public static readonly int DefaultFailuresBeforeBackoff = 3;
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    }
}