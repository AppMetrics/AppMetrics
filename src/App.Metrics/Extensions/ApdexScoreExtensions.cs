using System.Collections.Generic;
using System.Linq;
using App.Metrics.Data;

namespace App.Metrics.Extensions
{
    public static class ApdexScoreExtensions
    {
        public static IEnumerable<ApdexScore> ToMetric(this IEnumerable<ApdexValueSource> source)
        {
            return source.Select(x => ToMetric((ApdexValueSource)x));
        }

        public static ApdexScore ToMetric(this ApdexValueSource source)
        {
            return new ApdexScore
            {
                Name = source.Name,
                Score = source.Value.Score,
                SampleSize = source.Value.SampleSize,
                Satisfied = source.Value.Satisfied,
                Tolerating = source.Value.Tolerating,
                Frustrating = source.Value.Frustrating,
                Tags = source.Tags
            };
        }

        public static ApdexValueSource ToMetricValueSource(this ApdexScore source)
        {
            var counterValue = new ApdexValue(source.Score, source.Satisfied, source.Tolerating, source.Frustrating, source.SampleSize);
            return new ApdexValueSource(source.Name, ConstantValue.Provider(counterValue), source.Tags);
        }

        public static IEnumerable<ApdexValueSource> ToMetricValueSource(this IEnumerable<ApdexScore> source)
        {
            return source.Select(x => x.ToMetricValueSource());
        }
    }
}