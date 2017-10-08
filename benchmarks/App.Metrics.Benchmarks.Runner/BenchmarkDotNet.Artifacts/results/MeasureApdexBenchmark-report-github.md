``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                                Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |-------------------------------------- |---------:|----------:|----------:|-------:|----------:|
 |                        TimeAlgorithmR | 8.182 us | 0.1167 us | 0.1092 us | 0.0763 |     328 B |
 |      TrackUsingAlgorithmRUsingContext | 7.972 us | 0.1564 us | 0.1386 us | 0.0763 |     328 B |
 |             TrackUsingForwardDecaying | 8.191 us | 0.0669 us | 0.0559 us | 0.0916 |     392 B |
 | TrackUsingForwardDecayingUsingContext | 8.236 us | 0.0901 us | 0.0799 us | 0.0763 |     328 B |
 |               TrackUsingSlidingWindow | 7.901 us | 0.0305 us | 0.0285 us | 0.0763 |     328 B |
 |   TrackUsingSlidingWindowUsingContext | 8.054 us | 0.1654 us | 0.1904 us | 0.0763 |     328 B |
