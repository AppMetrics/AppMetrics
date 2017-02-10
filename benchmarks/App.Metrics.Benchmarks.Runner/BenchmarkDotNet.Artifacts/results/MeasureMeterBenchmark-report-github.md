``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
                         Method |        Mean |    StdErr |     StdDev |  Gen 0 | Allocated |
------------------------------- |------------ |---------- |----------- |------- |---------- |
                           Mark | 188.3100 ns | 0.5034 ns |  1.8151 ns | 0.0484 |     255 B |
                 MarkMetricItem | 334.4727 ns | 1.8489 ns |  7.1608 ns | 0.0608 |     303 B |
 MarkMetricItemWithMultipleTags | 767.8756 ns | 5.7194 ns | 21.4002 ns | 0.1127 |     662 B |
                  MarkUserValue | 300.1847 ns | 3.0066 ns | 13.4458 ns | 0.0479 |     255 B |
