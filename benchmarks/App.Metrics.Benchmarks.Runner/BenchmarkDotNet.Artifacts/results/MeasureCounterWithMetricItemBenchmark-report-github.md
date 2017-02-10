``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
                              Method |        Mean |     StdDev |  Gen 0 | Allocated |
------------------------------------ |------------ |----------- |------- |---------- |
                 DecrementMetricItem | 356.5641 ns |  4.3973 ns | 0.0610 |     303 B |
 DecrementMetricItemWithMulitpleTags | 764.4359 ns | 25.1468 ns | 0.1345 |     662 B |
                 IncrementMetricItem | 355.5709 ns |  4.5915 ns | 0.0610 |     303 B |
 IncrementMetricItemWithMulitpleTags | 812.5405 ns |  7.2268 ns | 0.1106 |     662 B |
