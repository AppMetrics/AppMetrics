``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-NLFVLB : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
| Method |     Mean |     Error |    StdDev |    Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |---------:|----------:|----------:|---------:|------:|------:|----------:|
|   Many | 1.430 ms | 0.0274 ms | 0.0473 ms | 292.9688 |     - |     - |   1.17 MB |
