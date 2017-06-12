// <copyright file="FormattingConstants.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

namespace App.Metrics.Internal
{
    public static class FormattingConstants
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