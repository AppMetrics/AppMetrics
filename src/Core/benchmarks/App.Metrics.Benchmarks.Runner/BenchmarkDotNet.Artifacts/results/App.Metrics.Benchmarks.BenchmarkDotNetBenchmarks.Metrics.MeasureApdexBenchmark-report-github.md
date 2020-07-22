``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-IMUXWQ : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                                Method |     Mean |     Error |    StdDev |   Median |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------------- |---------:|----------:|----------:|---------:|-------:|------:|------:|----------:|
|                        TimeAlgorithmR | 7.358 μs | 0.3757 μs | 1.0719 μs | 7.065 μs | 0.2670 |     - |     - |   1.11 KB |
|      TrackUsingAlgorithmRUsingContext | 6.099 μs | 0.1465 μs | 0.4272 μs | 5.975 μs | 0.2670 |     - |     - |   1.11 KB |
|             TrackUsingForwardDecaying | 6.977 μs | 0.2072 μs | 0.6011 μs | 6.797 μs | 0.3052 |     - |     - |   1.27 KB |
| TrackUsingForwardDecayingUsingContext | 5.950 μs | 0.1339 μs | 0.3840 μs | 5.818 μs | 0.2899 |     - |     - |    1.2 KB |
|               TrackUsingSlidingWindow | 5.865 μs | 0.1167 μs | 0.3310 μs | 5.726 μs | 0.2670 |     - |     - |   1.11 KB |
|   TrackUsingSlidingWindowUsingContext | 5.846 μs | 0.1160 μs | 0.3346 μs | 5.733 μs | 0.2670 |     - |     - |   1.11 KB |
