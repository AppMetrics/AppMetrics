// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Polly;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class HttpPolicy
    {
        public int FailuresBeforeBackoff { get; set; }

        public TimeSpan BackoffPeriod { get; set; }

        public TimeSpan Timeout { get; set; }

        public Policy AsPolicy()
        {
            return Policy.Handle<Exception>().CircuitBreakerAsync(FailuresBeforeBackoff, BackoffPeriod);
        }
    }
}