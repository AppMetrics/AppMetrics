// Timer Basic

var requestTimer = new TimerOptions
{
    Name = "Request Timer",
    MeasurementUnit = Unit.Requests,
    DurationUnit = TimeUnit.Milliseconds,
    RateUnit = TimeUnit.Milliseconds
};

_metrics.Time(requestTimer, () => PerformRequest());

// OR

using(_metrics.Time(requestTimer))
{
    PerformRequest();
}

// Timer Items

using(_metrics.Time(requestTimer, "feature-1"))
{
    PerformRequest();
}

// Meter Advanced

_metrics.Advanced.Timer(httpStatusMeter).Reset();