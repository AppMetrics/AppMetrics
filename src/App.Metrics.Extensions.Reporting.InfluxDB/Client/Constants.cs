// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Client
{
    internal static class Constants
    {
        public static readonly TimeSpan DefaultBackoffPeriod = TimeSpan.FromSeconds(30);
        public static readonly int DefaultFailuresBeforeBackoff = 3;
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
    }
}