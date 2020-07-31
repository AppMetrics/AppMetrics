``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|          Method |           Mean |        Error |        StdDev |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|---------------- |---------------:|-------------:|--------------:|---------:|------:|------:|-----------:|
|            Many | 1,074,349.0 ns | 45,520.53 ns | 125,376.76 ns | 253.9063 |     - |     - | 1039.06 KB |
|        SetValue |       992.5 ns |     19.57 ns |      24.74 ns |   0.2766 |     - |     - |    1.13 KB |
| SetValueNotLazy |     1,015.6 ns |     14.63 ns |      12.97 ns |   0.2537 |     - |     - |    1.04 KB |
