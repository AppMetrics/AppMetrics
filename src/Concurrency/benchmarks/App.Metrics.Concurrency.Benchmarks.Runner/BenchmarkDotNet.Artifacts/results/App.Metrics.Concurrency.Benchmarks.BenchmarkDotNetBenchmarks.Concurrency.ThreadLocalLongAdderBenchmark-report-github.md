``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-RXDNSD : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|    Method |     Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------- |---------:|---------:|---------:|-------:|------:|------:|----------:|
| Decrement | 13.09 ns | 0.296 ns | 0.644 ns |      - |     - |     - |         - |
|       Get | 40.12 ns | 1.191 ns | 3.494 ns | 0.0172 |     - |     - |      72 B |
| Increment | 12.35 ns | 0.269 ns | 0.450 ns |      - |     - |     - |         - |
