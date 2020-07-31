``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                Method |           Mean |        Error |        StdDev |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|---------------------- |---------------:|-------------:|--------------:|---------:|------:|------:|-----------:|
|                  Many | 1,192,708.6 ns | 38,484.36 ns | 113,472.01 ns | 261.7188 |     - |     - | 1070.31 KB |
|      UpdateAlgorithmR |     1,047.4 ns |     20.45 ns |      33.03 ns |   0.2613 |     - |     - |    1.07 KB |
| UpdateForwardDecaying |     1,246.7 ns |     18.82 ns |      17.60 ns |   0.2842 |     - |     - |    1.16 KB |
|   UpdateSlidingWindow |       959.5 ns |     12.61 ns |      11.79 ns |   0.2613 |     - |     - |    1.07 KB |
