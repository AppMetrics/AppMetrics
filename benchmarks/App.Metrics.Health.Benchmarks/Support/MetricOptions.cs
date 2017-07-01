// <copyright file="MetricOptions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Counter;

namespace App.Metrics.Health.Benchmarks.Support
{
    public static class MetricOptions
    {
        public static class Counter
        {
            public static readonly CounterOptions Options = new CounterOptions
                                                            {
                                                                Name = "test_counter"
                                                            };
        }
    }
}