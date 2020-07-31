``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                         Method |           Mean |        Error |       StdDev |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|------------------------------- |---------------:|-------------:|-------------:|---------:|------:|------:|-----------:|
|                           Many | 1,020,855.8 ns | 29,442.20 ns | 84,947.49 ns | 253.9063 |     - |     - | 1039.06 KB |
|                           Mark |       930.3 ns |     17.75 ns |     17.43 ns |   0.2537 |     - |     - |    1.04 KB |
|                 MarkMetricItem |     1,017.6 ns |     16.62 ns |     15.55 ns |   0.2632 |     - |     - |    1.08 KB |
| MarkMetricItemWithMultipleTags |     1,353.5 ns |     11.97 ns |      9.99 ns |   0.3510 |     - |     - |    1.44 KB |
|                  MarkUserValue |       990.9 ns |     19.50 ns |     19.15 ns |   0.2537 |     - |     - |    1.04 KB |
