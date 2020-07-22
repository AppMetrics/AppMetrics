``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-IMUXWQ : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------- |---------:|----------:|----------:|-------:|------:|------:|----------:|
|      UpdateAlgorithmR | 1.172 μs | 0.0266 μs | 0.0772 μs | 0.2613 |     - |     - |   1.07 KB |
| UpdateForwardDecaying | 1.294 μs | 0.0127 μs | 0.0106 μs | 0.2842 |     - |     - |   1.16 KB |
|   UpdateSlidingWindow | 1.023 μs | 0.0203 μs | 0.0285 μs | 0.2613 |     - |     - |   1.07 KB |
