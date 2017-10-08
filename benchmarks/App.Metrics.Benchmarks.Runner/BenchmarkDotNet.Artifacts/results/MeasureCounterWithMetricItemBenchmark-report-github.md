``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                              Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
 |------------------------------------ |---------:|---------:|---------:|-------:|----------:|
 |                 DecrementMetricItem | 367.3 ns | 4.053 ns | 3.791 ns | 0.0725 |     304 B |
 | DecrementMetricItemWithMulitpleTags | 825.0 ns | 7.675 ns | 7.179 ns | 0.1574 |     664 B |
 |                 IncrementMetricItem | 372.1 ns | 3.974 ns | 3.717 ns | 0.0725 |     304 B |
 | IncrementMetricItemWithMulitpleTags | 837.4 ns | 8.324 ns | 7.379 ns | 0.1574 |     664 B |
