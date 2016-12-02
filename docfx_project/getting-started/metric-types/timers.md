# Timers

A Timer is a combination of a [Histogram](histograms.md) and a [Meter](meters.md) allowing us to measure the duration of a type of event, the rate of its occurrence and provide duration statistics. For example, the [AspNet Middleware Extension](../intro.md) provides the ability to record a timer per endpoint as show shown in the JSON example below.

## Using Timers

Timers can be recorded by either passing an action into the `Time` method or by using a `using` statement as show below. When a `using` statement is used the Timer will end recording upon disposing.

[!code-csharp[Main](../../src/samples/Timers.cs?start=3&end=18)]

Timers, like [Histogram](histograms.md) also allow us to track the *min*, *max* and *last value* that has been recorded in cases when a "user value" is provided when recording the timer. For example, when timing requests where the endpoint has couple of features flags implemented, we could track which feature flag as producing the min and max response times. 

[!code-csharp[Main](../../src/samples/Timers.cs?start=22&end=25)]

Which for example when using the [JSON formatter](../intro.md#configuring-a-web-host) would result in something similar to:

[!code-json[Main](../../src/samples/TimerExample.json)]    

> [!NOTE]
> When reporting metrics with counts we should keep in mind that they are a cumulative count, see notes in the [Counters](counters.md#reporting-counters) documentation.
> A Timers values can also be reset like a [Counter](counters.md) as shown below.

[!code-csharp[Main](../../src/samples/Timers.cs?start=29)]

## Related Docs

- [Getting Started](../intro.md#measuring-application-metrics)
- [Configuration](../fundamentals/configuration.md)