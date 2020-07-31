``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                          Method |         Mean |      Error |      StdDev |       Median |  Ratio | RatioSD |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------------------------------- |-------------:|-----------:|------------:|-------------:|-------:|--------:|---------:|------:|------:|-----------:|
|                            Many | 1,481.624 μs | 56.9374 μs | 163.3641 μs | 1,407.686 μs | 287.01 |   21.91 | 261.7188 |     - |     - | 1070.31 KB |
|                  TimeAlgorithmR |     5.676 μs |  0.0757 μs |   0.0709 μs |     5.678 μs |   1.00 |    0.00 |   0.2670 |     - |     - |    1.11 KB |
|      TimeAlgorithmRUsingContext |     5.406 μs |  0.0696 μs |   0.0651 μs |     5.377 μs |   0.95 |    0.02 |   0.2670 |     - |     - |    1.11 KB |
|             TimeForwardDecaying |     5.723 μs |  0.1066 μs |   0.1095 μs |     5.720 μs |   1.01 |    0.02 |   0.2899 |     - |     - |     1.2 KB |
| TimeForwardDecayingUsingContext |     5.906 μs |  0.0585 μs |   0.0547 μs |     5.914 μs |   1.04 |    0.02 |   0.2899 |     - |     - |     1.2 KB |
|               TimeSlidingWindow |     5.404 μs |  0.0855 μs |   0.0800 μs |     5.372 μs |   0.95 |    0.02 |   0.2670 |     - |     - |    1.11 KB |
|   TimeSlidingWindowUsingContext |     5.343 μs |  0.1040 μs |   0.1156 μs |     5.338 μs |   0.94 |    0.03 |   0.2670 |     - |     - |    1.11 KB |
