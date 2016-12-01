# Gauges

A Gauge is simply an action that returns the instantaneous measure of a value, where the value abitrarily increases and decreases, for example CPU usage.

Gauges are ideal to use for measured values such as current memory usage, cpu usage, temperatures, disk space etc.

## Using Gauges

[!code-csharp[Main](../../src/samples/Gauges.cs?start=3&end=11&highlight=9)]

Which for example when using the JSON formatter would result in:

[!code-json[Main](../../src/samples/GaugeExample.json)]

### Derived Gauges

Derived Gauges allow you to derive a value from another Gauge and using a transformation, calculate the measurement.

[!code-csharp[Main](../../src/samples/Gauges.cs?start=17&end=28&highlight=12)]

### Ratio Gauges

Ratio Gauges allow you to measure a Gauge with a measurement which is the ratio between two values.

[!code-csharp[Main](../../src/samples/Gauges.cs?start=32&end=47&highlight=16)]

