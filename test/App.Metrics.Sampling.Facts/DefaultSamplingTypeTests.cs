using System.Reflection;
using App.Metrics.Core;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts
{
    public class HistogramMetricSamplingTypeTests
    {
        private static readonly FieldInfo ReservoirField = typeof(HistogramMetric).GetField("_reservoir",
            BindingFlags.Instance | BindingFlags.NonPublic);

        [Fact]
        public void maps_correct_sampling_type_to_reservoir()
        {
            GetReservoir(new HistogramMetric(SamplingType.ExponentiallyDecaying)).Should().BeOfType<ExponentiallyDecayingReservoir>();
            GetReservoir(new HistogramMetric(SamplingType.LongTerm)).Should().BeOfType<UniformReservoir>();
            GetReservoir(new HistogramMetric(SamplingType.HighDynamicRange)).Should().BeOfType<HdrHistogramReservoir>();
            GetReservoir(new HistogramMetric(SamplingType.SlidingWindow)).Should().BeOfType<SlidingWindowReservoir>();
        }

        private static IReservoir GetReservoir(HistogramMetric histogram)
        {
            return ReservoirField.GetValue(histogram) as IReservoir;
        }
    }
}