``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-RXDNSD : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|    Method |     Mean |     Error |    StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------- |---------:|----------:|----------:|------:|------:|------:|----------:|
| Decrement | 8.503 ns | 0.2411 ns | 0.6955 ns |     - |     - |     - |         - |
|       Get | 1.190 ns | 0.0603 ns | 0.0592 ns |     - |     - |     - |         - |
| Increment | 7.618 ns | 0.0753 ns | 0.0628 ns |     - |     - |     - |         - |
