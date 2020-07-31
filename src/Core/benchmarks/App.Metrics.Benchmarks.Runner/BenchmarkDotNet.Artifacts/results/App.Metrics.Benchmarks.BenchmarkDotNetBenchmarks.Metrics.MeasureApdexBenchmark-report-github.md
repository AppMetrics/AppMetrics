``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                                Method |         Mean |      Error |     StdDev |       Median |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------------------------- |-------------:|-----------:|-----------:|-------------:|---------:|------:|------:|-----------:|
|                                  Many | 1,078.526 μs | 21.0817 μs | 25.8902 μs | 1,077.925 μs | 261.7188 |     - |     - | 1070.31 KB |
|                        TimeAlgorithmR |     5.515 μs |  0.1057 μs |  0.1175 μs |     5.456 μs |   0.2670 |     - |     - |    1.11 KB |
|      TrackUsingAlgorithmRUsingContext |     5.533 μs |  0.0925 μs |  0.0773 μs |     5.543 μs |   0.2670 |     - |     - |    1.11 KB |
|             TrackUsingForwardDecaying |     6.061 μs |  0.0902 μs |  0.0704 μs |     6.080 μs |   0.3052 |     - |     - |    1.27 KB |
| TrackUsingForwardDecayingUsingContext |     5.751 μs |  0.1128 μs |  0.3107 μs |     5.629 μs |   0.2899 |     - |     - |     1.2 KB |
|               TrackUsingSlidingWindow |     5.072 μs |  0.0630 μs |  0.0797 μs |     5.043 μs |   0.2670 |     - |     - |    1.11 KB |
|   TrackUsingSlidingWindowUsingContext |     5.124 μs |  0.0928 μs |  0.1361 μs |     5.095 μs |   0.2670 |     - |     - |    1.11 KB |
