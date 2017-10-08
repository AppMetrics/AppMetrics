``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                Method |     Mean |     Error |   StdDev |  Gen 0 | Allocated |
 |---------------------- |---------:|----------:|---------:|-------:|----------:|
 |      UpdateAlgorithmR | 384.0 ns |  5.313 ns | 4.970 ns | 0.0682 |     288 B |
 | UpdateForwardDecaying | 557.2 ns | 10.141 ns | 9.486 ns | 0.0677 |     288 B |
 |   UpdateSlidingWindow | 300.3 ns |  2.888 ns | 2.702 ns | 0.0682 |     288 B |
