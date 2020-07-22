``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-IMUXWQ : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                         Method |       Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------- |-----------:|---------:|---------:|-------:|------:|------:|----------:|
|                           Mark |   982.6 ns | 19.18 ns | 26.88 ns | 0.2537 |     - |     - |   1.04 KB |
|                 MarkMetricItem | 1,100.3 ns | 20.22 ns | 18.91 ns | 0.2632 |     - |     - |   1.08 KB |
| MarkMetricItemWithMultipleTags | 1,451.9 ns | 31.32 ns | 91.85 ns | 0.3510 |     - |     - |   1.44 KB |
|                  MarkUserValue | 1,085.8 ns | 21.06 ns | 25.07 ns | 0.2537 |     - |     - |   1.04 KB |
