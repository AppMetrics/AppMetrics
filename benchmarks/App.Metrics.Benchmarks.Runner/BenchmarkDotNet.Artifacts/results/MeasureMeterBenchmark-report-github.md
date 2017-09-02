``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                         Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |------------------------------- |---------:|----------:|----------:|-------:|----------:|
 |                           Mark | 207.3 ns |  1.666 ns |  1.391 ns | 0.0608 |     256 B |
 |                 MarkMetricItem | 355.2 ns |  4.219 ns |  3.946 ns | 0.0725 |     304 B |
 | MarkMetricItemWithMultipleTags | 758.0 ns | 12.736 ns | 11.913 ns | 0.1575 |     664 B |
 |                  MarkUserValue | 313.5 ns |  5.697 ns |  4.757 ns | 0.0606 |     256 B |
