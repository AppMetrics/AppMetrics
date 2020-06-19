``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-RXDNSD : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|               Method |     Mean |    Error |    StdDev |   Median | Ratio | RatioSD |   Gen 0 |  Gen 1 |  Gen 2 | Allocated |
|--------------------- |---------:|---------:|----------:|---------:|------:|--------:|--------:|-------:|-------:|----------:|
|           AtomicLong | 894.6 μs | 33.18 μs |  96.79 μs | 882.5 μs |  1.00 |    0.00 | 17.5781 | 2.9297 | 2.9297 |   2.64 KB |
|     PaddedAtomicLong | 967.2 μs | 38.37 μs | 111.33 μs | 954.0 μs |  1.10 |    0.20 | 17.5781 | 2.9297 | 2.9297 |   2.76 KB |
|     StripedLongAdder | 884.5 μs | 28.67 μs |  83.63 μs | 866.4 μs |  1.00 |    0.14 | 17.5781 | 2.9297 | 2.9297 |   2.69 KB |
| ThreadLocalLongAdder | 977.5 μs | 33.56 μs |  96.28 μs | 946.7 μs |  1.10 |    0.15 | 41.0156 | 5.8594 | 2.9297 | 139.82 KB |
