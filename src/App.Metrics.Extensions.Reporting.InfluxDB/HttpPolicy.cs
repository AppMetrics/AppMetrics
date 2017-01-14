// Copyright (c) Allan Hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using App.Metrics.Extensions.Reporting.InfluxDB.Client;
using Polly;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    public class HttpPolicy
    {
        public HttpPolicy()
        {
            FailuresBeforeBackoff = Constants.DefaultFailuresBeforeBackoff;
            BackoffPeriod = Constants.DefaultBackoffPeriod;
            Timeout = Constants.DefaultTimeout;
        }

        public TimeSpan BackoffPeriod { get; set; }

        public int FailuresBeforeBackoff { get; set; }

        public TimeSpan Timeout { get; set; }

        public Policy AsPolicy() { return Policy.Handle<Exception>().CircuitBreakerAsync(FailuresBeforeBackoff, BackoffPeriod); }
    }
}