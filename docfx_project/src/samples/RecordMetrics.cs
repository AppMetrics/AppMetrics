_metrics.Increment(AppMetricsRegistery.SampleCounter);
_metrics.Decrement(AppMetricsRegistery.SampleCounter);

_metrics.Gauge(AppMetricsRegistery.Errors, () => 1);

_metrics.Update(AppMetricsRegistery.SampleHistogram, 1);

_metrics.Mark(AppMetricsRegistery.SampleMeter, 1);

using(_metrics.Time(AppMetricsRegistery.SampleTimer))
{
    // Do something
}