``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-SIELXX : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|         Method |       Mean |     Error |    StdDev |     Median | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------- |-----------:|----------:|----------:|-----------:|------:|------:|------:|----------:|
|      Decrement | 11.8272 ns | 0.3821 ns | 1.1146 ns | 11.2655 ns |     - |     - |     - |         - |
|            Get |  0.1689 ns | 0.0380 ns | 0.1089 ns |  0.1495 ns |     - |     - |     - |         - |
|      Increment | 13.0077 ns | 0.2934 ns | 0.7360 ns | 12.9705 ns |     - |     - |     - |         - |
| IncrementValue | 10.9379 ns | 0.1303 ns | 0.1155 ns | 10.9208 ns |     - |     - |     - |         - |
