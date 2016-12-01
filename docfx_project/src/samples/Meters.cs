// Meter Basic

var cacheHitsMeter = new MeterOptions
{
    Name = "Cache Hits",
    MeasurementUnit = Unit.Calls
};

_metrics.Mark(cacheHitsMeter); 
_metrics.Mark(cacheHitsMeter, 10);

// Meter Items

var httpStatusMeter = new MeterOptions
{
    Name = "Http Status",
    MeasurementUnit = Unit.Calls
};

_metrics.Mark(httpStatusMeter, "200");
_metrics.Mark(httpStatusMeter, "500");
_metrics.Mark(httpStatusMeter, "401");

// Meter Advanced

_metrics.Advanced.Meter(httpStatusMeter).Reset();