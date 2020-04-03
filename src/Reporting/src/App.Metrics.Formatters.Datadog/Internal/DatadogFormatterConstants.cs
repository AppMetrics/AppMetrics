// <copyright file="DatadogFormatterConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

using System;

namespace App.Metrics.Formatters.Datadog.Internal
{
    public static class DatadogFormatterConstants
    {
        public static class GraphiteDefaults
        {
            public static readonly Func<IDatadogMetricJsonWriter> MetricPointTextWriter = () => new DefaultDatadogMetricJsonWriter();
        }
    }
}