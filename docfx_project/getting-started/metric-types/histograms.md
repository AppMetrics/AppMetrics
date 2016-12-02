# Histograms

Histograms measure the statistical distribution of a set of values including the *min*, *max*, *mean*, *median*, *standard deviation* and [quantiles](https://en.wikipedia.org/wiki/Quantile) i.e. the *75th percentile*, *90th percentile*, *95th percentile*, *99th percentile* and *99.9th percentile* allowing us to sample observations for things like response sizes. 

A use case for a Histogram could be tracking the POST and PUT requests sizes made to a web service or tracking the file sizes uploaded to an endpoint.

## Using Histograms

[!code-csharp[Main](../../src/samples/Histograms.cs?start=3&end=22&highlight=15)]

Histograms also allow us to track the *min*, *max* and *last value* that has been recorded in cases when a "user value" is provided when updating the histogram. For example if we wanted to measure the statistical distribution of files uploaded to a file service and track which applications are uploading the largest files, we could use an applications client id when updating the histgoram:

[!code-csharp[Main](../../src/samples/Histograms.cs?start=26&end=33)]

Which for example when using the [JSON formatter](../intro.md#configuring-a-web-host) would result in something similar to:

[!code-json[Main](../../src/samples/HistogramExample.json)]    

## Resevoir Sampling

In high performance applications it is not possible to keep the entire data stream of a histogram in memory. To work around this [resevoir sampling](../sampling/index.md) algorithms allow us to maintain a small, manageable resevoir which is statistically representative of an entire data stream.

> [!NOTE]
> ### Just a Placeholder for now..
> Implement and document  [Apdex Score](https://en.wikipedia.org/wiki/Apdex)

## Related Docs

- [Getting Started](../intro.md#measuring-application-metrics)
- [Configuration](../fundamentals/configuration.md)