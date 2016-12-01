// Histogram Basic

var postAndPutRequestSize = new HistogramOptions
{
    Name = "Web Request Post & Put Size",
    MeasurementUnit = Unit.Bytes
};

public async Task Invoke(HttpContext context)
{
    var httpMethod = context.Request.Method.ToUpperInvariant();

    if (httpMethod == "POST" || httpMethod == "PUT")
    {
        if (context.Request.Headers != null && context.Request.Headers.ContainsKey("Content-Length"))
        {            
            _metrics.Update(long.Parse(context.Request.Headers["Content-Length"].First())); 
        }
    }

    await Next(context);
}

// Histogram User Value

var fileSize = new HistogramOptions
{
    Name = "Document File Size",
    MeasurementUnit = Unit.Calls
};

_metrics.Update(httpStatusMeter, CalculateDocumentSize(), "client_1");
_metrics.Update(httpStatusMeter, CalculateDocumentSize(), "client_2");

// Histogram Advanced

_metrics.Advanced.Meter(httpStatusMeter).Reset();