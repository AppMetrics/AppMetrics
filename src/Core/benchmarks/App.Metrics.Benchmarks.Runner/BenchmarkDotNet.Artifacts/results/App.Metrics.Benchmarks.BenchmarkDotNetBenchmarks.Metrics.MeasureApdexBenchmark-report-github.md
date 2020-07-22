``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-QBNDVI : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                                Method |         Mean |      Error |     StdDev |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------------------------- |-------------:|-----------:|-----------:|---------:|------:|------:|-----------:|
|                                  Many | 1,134.791 μs | 19.0997 μs | 17.8659 μs | 261.7188 |     - |     - | 1070.33 KB |
|                        TimeAlgorithmR |     6.157 μs |  0.1130 μs |  0.1725 μs |   0.2670 |     - |     - |    1.11 KB |
|      TrackUsingAlgorithmRUsingContext |     6.014 μs |  0.1154 μs |  0.1283 μs |   0.2670 |     - |     - |    1.11 KB |
|             TrackUsingForwardDecaying |     6.256 μs |  0.0738 μs |  0.0616 μs |   0.3052 |     - |     - |    1.27 KB |
| TrackUsingForwardDecayingUsingContext |     6.945 μs |  0.1358 μs |  0.2033 μs |   0.2899 |     - |     - |     1.2 KB |
|               TrackUsingSlidingWindow |     6.855 μs |  0.1261 μs |  0.1180 μs |   0.2670 |     - |     - |    1.11 KB |
|   TrackUsingSlidingWindowUsingContext |     6.988 μs |  0.0942 μs |  0.0881 μs |   0.2670 |     - |     - |    1.11 KB |
