using System;
using App.Metrics.Core;

namespace App.Metrics.Internal
{
    internal static class MetricValueOptionsExtensions
    {
        public static MetricValueOptions EnsureGroupName(this MetricValueOptions options, string defaultGroupName)
        {
            if (options.GroupName.IsMissing())
            {
                options.GroupName = defaultGroupName;
            }

            return options;
        }

        public static MetricValueWithSamplingOption EnsureSamplingType(this MetricValueWithSamplingOption options, SamplingType defaultSamplingType)
        {
            if (options.SamplingType == SamplingType.Default)
            {
                options.SamplingType = defaultSamplingType;
            }

            return options;
        }
    }
}