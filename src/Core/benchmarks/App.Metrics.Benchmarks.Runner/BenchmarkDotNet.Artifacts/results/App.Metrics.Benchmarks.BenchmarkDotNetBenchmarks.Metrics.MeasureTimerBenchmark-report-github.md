``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-MADEYH : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                          Method |         Mean |      Error |     StdDev |  Ratio | RatioSD |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------------------- |-------------:|-----------:|-----------:|-------:|--------:|---------:|------:|------:|-----------:|
|                            Many | 1,461.130 μs | 29.1810 μs | 39.9433 μs | 252.77 |    7.97 | 261.7188 |     - |     - | 1070.31 KB |
|                  TimeAlgorithmR |     5.778 μs |  0.0579 μs |  0.0514 μs |   1.00 |    0.00 |   0.2670 |     - |     - |    1.11 KB |
|      TimeAlgorithmRUsingContext |     6.404 μs |  0.1073 μs |  0.1054 μs |   1.11 |    0.02 |   0.2670 |     - |     - |    1.11 KB |
|             TimeForwardDecaying |     6.600 μs |  0.1287 μs |  0.2003 μs |   1.14 |    0.04 |   0.2899 |     - |     - |     1.2 KB |
| TimeForwardDecayingUsingContext |     6.383 μs |  0.0864 μs |  0.0766 μs |   1.10 |    0.02 |   0.2899 |     - |     - |     1.2 KB |
|               TimeSlidingWindow |     6.339 μs |  0.1261 μs |  0.2071 μs |   1.11 |    0.03 |   0.2670 |     - |     - |    1.11 KB |
|   TimeSlidingWindowUsingContext |     5.955 μs |  0.0986 μs |  0.0874 μs |   1.03 |    0.02 |   0.2670 |     - |     - |    1.11 KB |
