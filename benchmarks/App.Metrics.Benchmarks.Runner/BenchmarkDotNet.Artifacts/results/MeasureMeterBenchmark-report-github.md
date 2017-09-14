``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312790 Hz, Resolution=301.8604 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                         Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |------------------------------- |---------:|----------:|----------:|-------:|----------:|
 |                           Mark | 201.7 ns | 0.6167 ns | 0.5467 ns | 0.0608 |     256 B |
 |                 MarkMetricItem | 375.0 ns | 2.6869 ns | 2.2437 ns | 0.0725 |     304 B |
 | MarkMetricItemWithMultipleTags | 731.5 ns | 3.1664 ns | 2.9618 ns | 0.1575 |     664 B |
 |                  MarkUserValue | 309.2 ns | 3.9066 ns | 3.4631 ns | 0.0606 |     256 B |
