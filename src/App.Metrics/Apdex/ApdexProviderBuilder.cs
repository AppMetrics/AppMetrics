using App.Metrics.Apdex.Interfaces;
using App.Metrics.Sampling;

namespace App.Metrics.Apdex
{
    public static class ApdexProviderBuilder
    {
        public static IApdexProvider Build(SamplingType samplingType, int sampleSize, double alpha, double apdexTSeconds)
        {
            return new ApdexProvider(ReservoirBuilder.Build(samplingType, sampleSize, alpha), apdexTSeconds);
        }
    }
}