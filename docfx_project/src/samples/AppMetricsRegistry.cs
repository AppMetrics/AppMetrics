public static class AppMetricsRegistery
{
    public static GaugeOptions Errors { get; } = new GaugeOptions
    {
        Name = "Errors"            
    };

    public static CounterOptions SampleCounter { get; } = new CounterOptions
    {
        Name = "Sample Counter",
        MeasurementUnit = Unit.Calls,
    }

    public static HistogramOptions SampleHistogram { get; } = new HistogramOptions
    {
        Name = "Sample Histogram",
        SamplingType = SamplingType.HighDynamicRange,
        MeasurementUnit = Unit.MegaBytes            
    };

    public static MeterOptions SampleMeter { get; } = new MeterOptions
    {
        Name = "Sample Meter",
        MeasurementUnit = Unit.Calls            
    };

    public static TimerOptions SampleTimer { get; } = new TimerOptions
    {
        Name = "Sample Timer",
        MeasurementUnit = Unit.Items,
        DurationUnit = TimeUnit.Milliseconds,
        RateUnit = TimeUnit.Milliseconds,            
        SamplingType = SamplingType.ExponentiallyDecaying,
        ExponentialDecayFactor = 0.015
    };
}