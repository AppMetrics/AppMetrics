``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-IMUXWQ : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|          Method |       Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |-----------:|---------:|---------:|-------:|------:|------:|----------:|
|        SetValue | 1,045.3 ns | 28.45 ns | 82.52 ns | 0.2766 |     - |     - |   1.13 KB |
| SetValueNotLazy |   946.7 ns | 18.93 ns | 25.92 ns | 0.2537 |     - |     - |   1.04 KB |
