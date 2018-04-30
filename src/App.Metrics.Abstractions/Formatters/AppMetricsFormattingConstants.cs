// <copyright file="AppMetricsFormattingConstants.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Formatters
{
    public static class AppMetricsFormattingConstants
    {
        public static class MetricName
        {
            public static readonly string DimensionSeparator = "|";
        }

        public static class MetricSetItem
        {
            public static readonly string FallbackKey = "item";
            public static readonly char KeyValueSeparator = ':';
            public static readonly char ItemSeparator = ',';
        }

        public static class MetricTag
        {
            public static readonly string KeyValueSeparator = ":";
            public static readonly string TagSeparator = ",";
        }
    }
}