``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-QBNDVI : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                         Method |         Mean |      Error |      StdDev |       Median |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|------------------------------- |-------------:|-----------:|------------:|-------------:|---------:|------:|------:|-----------:|
|                           Many | 1,131.786 μs | 35.9278 μs | 104.8032 μs | 1,098.962 μs | 253.9063 |     - |     - | 1039.06 KB |
|                           Mark |     1.028 μs |  0.0153 μs |   0.0143 μs |     1.026 μs |   0.2537 |     - |     - |    1.04 KB |
|                 MarkMetricItem |     1.134 μs |  0.0157 μs |   0.0215 μs |     1.133 μs |   0.2632 |     - |     - |    1.08 KB |
| MarkMetricItemWithMultipleTags |     1.472 μs |  0.0267 μs |   0.0591 μs |     1.456 μs |   0.3510 |     - |     - |    1.44 KB |
|                  MarkUserValue |     1.108 μs |  0.0158 μs |   0.0263 μs |     1.105 μs |   0.2537 |     - |     - |    1.04 KB |
