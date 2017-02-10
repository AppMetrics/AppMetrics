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
                 DecrementMetricItem | 354.5642 ns |  1.8053 ns | 0.0603 |     303 B |
 DecrementMetricItemWithMulitpleTags | 767.3976 ns | 24.6548 ns | 0.1325 |     662 B |
                 IncrementMetricItem | 351.1309 ns |  4.2417 ns | 0.0635 |     303 B |
 IncrementMetricItemWithMulitpleTags | 761.7331 ns | 24.8115 ns | 0.1340 |     662 B |
