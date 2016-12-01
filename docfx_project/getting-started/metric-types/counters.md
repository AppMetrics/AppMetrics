# Counters

Counters are one of the simpliest metrics types supported and allow us to track how many times something has happened. They are an atomic 64-bit integer which can be incremented or decremented.

Counters are ideal for counting occurances, events or errors for example. They also provide the ability to track a count and percentage of each item from a finite set, for example if we were tracking the number of emails our application has sent, we could also track the number of each type of email sent and their percentage of the overall count within the same counter.

## Using Counters

[!code-csharp[Main](../../src/samples/Counters.cs?start=3&end=12)]

And if we wanted to track the number of emails sent by type for example:

[!code-csharp[Main](../../src/samples/Counters.cs?start=16&end=24)]

Which for example when using the JSON formatter would result in:

[!code-json[Main](../../src/samples/CounterExample.json)]    

## Reporting Counters

When reporting counters we should keep in mind that they are a cumulative count, therefore if we want to sum the counters over a given interval we may not see the expected result since this will be a sum of the cumulative count at each reporting interval, also since metrics are recorded in memory a new deployment of the application will of course reset the count.  

We can solve this by resetting the counter each time we flush our metrics to a data store. For example:

[!code-csharp[Main](../../src/samples/Counters.cs?start=28&end=28)]

> [!NOTE]
> At the moment the above would have to apply for all counters within the reporter. The next release will provide a specific counter type which will be reset after reporting.

## Related Docs

- [Getting Started](../intro.md)
- [Configuration](../fundamentals/configuration.md)