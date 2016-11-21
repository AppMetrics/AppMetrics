public class MyService
{
    private readonly IMetrics _metrics;

    public MyService(IMetrics metrics)
    {
        _metrics = metrics;
    }

    public Task<MetricsDataValueSource> DoSomething()
    {
        var filter = new DefaultMetricsFitler().WhereMetricNameStartsWith("test_");
        return _metrics.Advanced.Data.ReadDataAsync(filter);
    }
}