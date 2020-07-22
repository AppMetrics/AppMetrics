``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.330 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host]     : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT
  Job-GQMFMQ : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|    Method |         Mean |      Error |     StdDev |    Gen 0 | Gen 1 | Gen 2 |  Allocated |
|---------- |-------------:|-----------:|-----------:|---------:|------:|------:|-----------:|
|      Many | 1,026.397 μs | 13.4742 μs | 12.6038 μs | 253.9063 |     - |     - | 1039.07 KB |
| Decrement |     1.001 μs |  0.0139 μs |  0.0116 μs |   0.2537 |     - |     - |    1.04 KB |
| Increment |     1.030 μs |  0.0202 μs |  0.0379 μs |   0.2537 |     - |     - |    1.04 KB |
