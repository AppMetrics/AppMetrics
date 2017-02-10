``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
                         Method |        Mean |    StdDev |  Gen 0 | Allocated |
------------------------------- |------------ |---------- |------- |---------- |
                           Mark | 194.3923 ns | 0.7245 ns | 0.0489 |     255 B |
                 MarkMetricItem | 335.4640 ns | 1.4359 ns | 0.0603 |     303 B |
 MarkMetricItemWithMultipleTags | 743.6069 ns | 3.4109 ns | 0.1336 |     662 B |
                  MarkUserValue | 299.9856 ns | 1.9863 ns | 0.0490 |     255 B |
