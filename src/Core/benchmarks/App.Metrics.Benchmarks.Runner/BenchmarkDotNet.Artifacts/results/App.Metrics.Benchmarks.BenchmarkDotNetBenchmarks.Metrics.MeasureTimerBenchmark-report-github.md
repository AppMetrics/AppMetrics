``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-IMUXWQ : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                          Method |     Mean |     Error |    StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------------- |---------:|----------:|----------:|------:|--------:|-------:|------:|------:|----------:|
|                  TimeAlgorithmR | 6.016 μs | 0.1169 μs | 0.1888 μs |  1.00 |    0.00 | 0.2670 |     - |     - |   1.11 KB |
|      TimeAlgorithmRUsingContext | 5.403 μs | 0.1059 μs | 0.0939 μs |  0.90 |    0.04 | 0.2670 |     - |     - |   1.11 KB |
|             TimeForwardDecaying | 5.952 μs | 0.1183 μs | 0.2362 μs |  1.00 |    0.06 | 0.2899 |     - |     - |    1.2 KB |
| TimeForwardDecayingUsingContext | 6.234 μs | 0.1206 μs | 0.2264 μs |  1.04 |    0.05 | 0.2899 |     - |     - |    1.2 KB |
|               TimeSlidingWindow | 6.512 μs | 0.1700 μs | 0.4850 μs |  1.06 |    0.07 | 0.2594 |     - |     - |   1.11 KB |
|   TimeSlidingWindowUsingContext | 6.340 μs | 0.2017 μs | 0.5916 μs |  0.99 |    0.07 | 0.2670 |     - |     - |   1.11 KB |
