using System;
using App.Metrics.MetricData;
using App.Metrics.Sampling;

namespace App.Metrics.Core
{
    public interface HistogramImplementation : Histogram, MetricValueProvider<HistogramValue>
    {
    }

    public sealed class HistogramMetric : HistogramImplementation
    {
        private readonly Reservoir _reservoir;
        private UserValueWrapper _last;

        public HistogramMetric()
            : this(SamplingType.Default)
        {
        }

        public HistogramMetric(SamplingType samplingType)
            : this(SamplingTypeToReservoir(samplingType))
        {
        }

        public HistogramMetric(Reservoir reservoir)
        {
            _reservoir = reservoir;
        }

        public HistogramValue Value
        {
            get { return GetValue(); }
        }

        public HistogramValue GetValue(bool resetMetric = false)
        {
            var value = new HistogramValue(_last.Value, _last.UserValue, _reservoir.GetSnapshot(resetMetric));
            if (resetMetric)
            {
                _last = UserValueWrapper.Empty;
            }
            return value;
        }

        public void Reset()
        {
            _last = UserValueWrapper.Empty;
            _reservoir.Reset();
        }

        public void Update(long value, string userValue = null)
        {
            _last = new UserValueWrapper(value, userValue);
            _reservoir.Update(value, userValue);
        }

        private static Reservoir SamplingTypeToReservoir(SamplingType samplingType)
        {
            while (true)
            {
                switch (samplingType)
                {
                    case SamplingType.Default:
                        samplingType = Metric.Config.DefaultSamplingType;
                        continue;
                    case SamplingType.HighDynamicRange:
                        return new HdrHistogramReservoir();
                    case SamplingType.ExponentiallyDecaying:
                        return new ExponentiallyDecayingReservoir();
                    case SamplingType.LongTerm:
                        return new UniformReservoir();
                    case SamplingType.SlidingWindow:
                        return new SlidingWindowReservoir();
                }
                throw new InvalidOperationException("Sampling type not implemented " + samplingType);
            }
        }
    }
}