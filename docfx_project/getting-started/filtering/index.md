# Filtering Metrics

## Report Filtering

App Metrics allows you to filter Metrics per reporter.

Below is an example reporting configuration which provides a filter to a console reporter filtering Metrics containing a tag key of "filter-tag1" and are Counters

[!code-csharp[Main](../../src/samples/ConfigureServicesWithReportingAndFilter.cs?highlight=11,12,13,20)]

## Globally Filtering

Metrics can also be set at a global level which would apply to all Metric data queries.

Below is an example of how to apply a global filter in a Web Host.

[!code-csharp[Main](../../src/samples/ConfigureServicesWithGlobalFilter.cs?highlight=9,13)]

Requesting the `/metrics` endpoint with the above configuration would result in only Counter Metrics being shown in the response.

## Overriding the Global Filter

In cases where a global filter is set as well as a filter on a reporter, the reporter's filter will be used instead. 

It is also possible to override the global filter if you where to retrieve a snapshot of the current metrics data via the Advanced Metric contract.

[!code-csharp[Main](../../src/samples/OverrideGlobalFilterWhenGetMetricsData.cs?highlight=12,13)]

## Supported Filtering

Metrics can be filtered by:

- Metric Type
- Tags
- Context
- Metric Name (Exact match or where name starts with)

> [!NOTE]
> Custom Metric Filters can be implemented rather than using the [DefaultMetricsFilter](../../api/App.Metrics.DefaultMetricsFilter.html) by implementing [IMetricsFilter](../../api/App.Metrics.IMetricsFilter.html) 

## Next Steps

- [Metric Types](../metric-types/index.md)
- [Organizing Metrics - Contexts and Tagging](../fundamentals/organizing-metrics.md)
- [Reporting Metrics](../reporting/index.md)
- [Sample Applications](../../samples/index.md)