``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                          Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |  Gen 0 | Allocated |
 |-------------------------------- |---------:|----------:|----------:|-------:|---------:|-------:|----------:|
 |                  TimeAlgorithmR | 8.080 us | 0.0983 us | 0.0871 us |   1.00 |     0.00 | 0.0763 |     328 B |
 |      TimeAlgorithmRUsingContext | 8.071 us | 0.1106 us | 0.1035 us |   1.00 |     0.02 | 0.0763 |     328 B |
 |             TimeForwardDecaying | 8.181 us | 0.0782 us | 0.0731 us |   1.01 |     0.01 | 0.0763 |     328 B |
 | TimeForwardDecayingUsingContext | 8.249 us | 0.0521 us | 0.0462 us |   1.02 |     0.01 | 0.0763 |     328 B |
 |               TimeSlidingWindow | 7.921 us | 0.0421 us | 0.0393 us |   0.98 |     0.01 | 0.0763 |     328 B |
 |   TimeSlidingWindowUsingContext | 7.885 us | 0.0539 us | 0.0478 us |   0.98 |     0.01 | 0.0763 |     328 B |
