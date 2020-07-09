``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-QBNDVI : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                Method |         Mean |      Error |     StdDev |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|---------------------- |-------------:|-----------:|-----------:|---------:|------:|------:|-----------:|
|                  Many | 1,310.022 μs | 25.8414 μs | 53.3670 μs | 261.7188 |     - |     - | 1070.31 KB |
|      UpdateAlgorithmR |     1.242 μs |  0.0165 μs |  0.0146 μs |   0.2613 |     - |     - |    1.07 KB |
| UpdateForwardDecaying |     1.461 μs |  0.0257 μs |  0.0264 μs |   0.2842 |     - |     - |    1.16 KB |
|   UpdateSlidingWindow |     1.161 μs |  0.0211 μs |  0.0198 μs |   0.2613 |     - |     - |    1.07 KB |
