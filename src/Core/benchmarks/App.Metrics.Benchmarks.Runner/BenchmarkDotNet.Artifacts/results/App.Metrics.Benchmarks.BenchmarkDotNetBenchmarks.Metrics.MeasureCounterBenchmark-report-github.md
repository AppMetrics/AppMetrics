``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|    Method |         Mean |        Error |       StdDev |       Median |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|---------- |-------------:|-------------:|-------------:|-------------:|---------:|------:|------:|-----------:|
|      Many | 974,794.4 ns | 19,453.67 ns | 45,854.59 ns | 955,788.4 ns | 253.9063 |     - |     - | 1039.06 KB |
| Decrement |     908.5 ns |     14.50 ns |     12.85 ns |     908.3 ns |   0.2537 |     - |     - |    1.04 KB |
| Increment |     893.4 ns |     17.03 ns |     15.93 ns |     890.2 ns |   0.2537 |     - |     - |    1.04 KB |
