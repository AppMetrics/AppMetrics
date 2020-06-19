``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-XTIMUD : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|               Method |      Mean |    Error |   StdDev |    Median | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------- |----------:|---------:|---------:|----------:|------:|--------:|-------:|------:|------:|----------:|
|           AtomicLong |  30.24 ns | 0.636 ns | 1.368 ns |  29.65 ns |  1.00 |    0.00 |      - |     - |     - |         - |
|     PaddedAtomicLong |  29.83 ns | 0.591 ns | 0.847 ns |  29.64 ns |  0.98 |    0.05 |      - |     - |     - |         - |
|     StripedLongAdder |  47.11 ns | 0.976 ns | 1.927 ns |  46.17 ns |  1.56 |    0.07 |      - |     - |     - |         - |
| ThreadLocalLongAdder | 186.50 ns | 3.753 ns | 3.686 ns | 185.44 ns |  5.96 |    0.29 | 0.0610 |     - |     - |     256 B |
