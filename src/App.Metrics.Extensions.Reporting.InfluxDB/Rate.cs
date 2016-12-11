// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using Polly;

namespace App.Metrics.Extensions.Reporting.InfluxDB
{
    internal class Rate
    {
        internal Rate(int events, TimeSpan period)
        {
            Events = events;
            Period = period;
        }

        /// <summary>
        ///     Rate specification in the form of: <code>events / timeframe</code>
        /// </summary>
        internal Rate(string specification)
        {
            var parts = specification.Split('/');
            Events = int.Parse(parts[0].Trim());
            Period = TimeSpan.Parse(parts[1].Trim());
        }

        internal int Events { get; }

        internal TimeSpan Period { get; }

        internal Policy AsPolicy()
        {
            return Policy.Handle<Exception>().CircuitBreakerAsync(Events, Period);
        }
    }
}