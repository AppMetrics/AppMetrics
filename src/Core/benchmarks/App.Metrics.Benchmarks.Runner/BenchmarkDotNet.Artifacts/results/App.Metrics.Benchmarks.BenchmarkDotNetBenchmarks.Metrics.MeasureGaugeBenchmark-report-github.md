``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-QBNDVI : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|          Method |         Mean |      Error |     StdDev |       Median |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|---------------- |-------------:|-----------:|-----------:|-------------:|---------:|------:|------:|-----------:|
|            Many | 1,180.104 μs | 23.4645 μs | 58.8678 μs | 1,159.823 μs | 253.9063 |     - |     - | 1039.06 KB |
|        SetValue |     1.110 μs |  0.0140 μs |  0.0117 μs |     1.109 μs |   0.2766 |     - |     - |    1.13 KB |
| SetValueNotLazy |     1.165 μs |  0.0206 μs |  0.0338 μs |     1.157 μs |   0.2537 |     - |     - |    1.04 KB |
