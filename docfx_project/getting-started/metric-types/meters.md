# Meters

A Meter measures the rate at which an event occurs along with a total count of the occurances. Rates that are measured are the *mean*, *one minute*, *five minute* and *fifteen minute*. The *mean* rate is an average rate of events for the lifetime of the application which does not provide a sense of recency, x-minute rates use an [Exponential Weighted Moving Average](https://en.wikipedia.org/wiki/Moving_average#Exponential moving average) (**EWMA**) for their calculations which do provide a sense of recency.

Meters provide the ability to track a count and percentage of each item within a set along with the rate of each item, for example if we were measuring the rate of HTTP requests made to an API protected with OAuth2, we could also track the rate at which each client was making these requests along with the number of requests and their percentage of the overall count within the same meter.

## Using Meters

[!code-csharp[Main](../../src/samples/Meters.cs?start=3&end=10)]

And if we wanted to track the rate at which each HTTP status was occuring with an API:

[!code-csharp[Main](../../src/samples/Meters.cs?start=14&end=22)]

Which for example when using the JSON formatter would result in:

[!code-json[Main](../../src/samples/MeterExample.json)]    

> [!NOTE]
> When reporting counters we should keep in mind that they are a cumulative count, see notes in the [Counters](counters.md#reporting-counters) documentation.
> A Meters values can also be reset like a [Counter](counters.md) as shown below.

[!code-csharp[Main](../../src/samples/Meters.cs?start=26)]

## Related Docs

- [Getting Started](../intro.md#measuring-application-metrics)
- [Configuration](../fundamentals/configuration.md)