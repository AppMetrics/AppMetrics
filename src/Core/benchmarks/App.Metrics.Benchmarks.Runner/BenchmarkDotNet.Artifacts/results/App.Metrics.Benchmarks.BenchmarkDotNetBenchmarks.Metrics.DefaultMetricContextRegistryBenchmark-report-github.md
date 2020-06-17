``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-VIOCGT : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                       Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------------------- |---------:|----------:|----------:|-------:|------:|------:|----------:|
|     ResolveApdexFromRegistry | 2.461 μs | 0.0474 μs | 0.1181 μs | 0.5684 |     - |     - |   2.33 KB |
|   ResolveCounterFromRegistry | 2.396 μs | 0.0440 μs | 0.0411 μs | 0.5684 |     - |     - |   2.34 KB |
|     ResolveGaugeFromRegistry | 2.513 μs | 0.0632 μs | 0.1813 μs | 0.5684 |     - |     - |   2.33 KB |
| ResolveHistogramFromRegistry | 2.703 μs | 0.0583 μs | 0.1653 μs | 0.5684 |     - |     - |   2.34 KB |
|     ResolveMeterFromRegistry | 2.681 μs | 0.0535 μs | 0.1437 μs | 0.5684 |     - |     - |   2.33 KB |
|     ResolveTimerFromRegistry | 2.932 μs | 0.1503 μs | 0.4338 μs | 0.5684 |     - |     - |   2.33 KB |
