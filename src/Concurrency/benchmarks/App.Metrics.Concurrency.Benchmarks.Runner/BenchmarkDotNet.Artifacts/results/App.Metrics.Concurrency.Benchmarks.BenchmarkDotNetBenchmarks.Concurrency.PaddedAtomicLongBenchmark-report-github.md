``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-RXDNSD : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|    Method |      Mean |     Error |    StdDev |    Median | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------- |----------:|----------:|----------:|----------:|------:|------:|------:|----------:|
| Decrement | 5.4577 ns | 0.1477 ns | 0.3891 ns | 5.4999 ns |     - |     - |     - |         - |
|       Get | 0.0202 ns | 0.0148 ns | 0.0438 ns | 0.0000 ns |     - |     - |     - |         - |
| Increment | 5.9072 ns | 0.1514 ns | 0.3934 ns | 5.9195 ns |     - |     - |     - |         - |
