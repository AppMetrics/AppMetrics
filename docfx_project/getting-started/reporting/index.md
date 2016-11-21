# Reporting

The current release of App.Metrics supports Console and Text File Reporters. An InfluxDB Reporter is planned for the next release.

## Console Reporter

[Configure a Console Application](../intro.md#configuring-a-console-application) with App Metrics, you can than add the Console Report Provider as below:

[!code-csharp[Main](../../src/samples/MetricsProgram.cs?highlight=30,31,32,33,18)]

## Text File Reporter

[Configure a Console Application](../intro.md#configuring-a-console-application) with App Metrics, similar to the Console Reporter example above a Text Report Provider can be added as below:

[!code-csharp[Main](../../src/samples/TextFileReporterSetup.cs)]

> [!NOTE]
> ### Don't see the reporter your looking for?
> Pull requests or more than welcome, for now see the [Console](https://github.com/alhardy/AppMetrics/tree/master/src/App.Metrics.Extensions.Reporting.Console) and [Text File](https://github.com/alhardy/AppMetrics/tree/master/src/App.Metrics.Extensions.Reporting.TextFile) reporters for guidance on implementing a new reporter.