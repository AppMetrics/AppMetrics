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
| Decrement | 5.7325 ns | 0.1520 ns | 0.4004 ns | 5.7903 ns |     - |     - |     - |         - |
|       Get | 0.0555 ns | 0.0285 ns | 0.0654 ns | 0.0420 ns |     - |     - |     - |         - |
| Increment | 5.6319 ns | 0.1476 ns | 0.2878 ns | 5.6699 ns |     - |     - |     - |         - |
