// <copyright file="CounterRegistry.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Counter;

namespace MetricsSandbox
{
    public static class CounterRegistry
    {
        public static CounterOptions CounterOne => new CounterOptions
                                                  {
                                                      Name = "test_one"
                                                  };
    }
}