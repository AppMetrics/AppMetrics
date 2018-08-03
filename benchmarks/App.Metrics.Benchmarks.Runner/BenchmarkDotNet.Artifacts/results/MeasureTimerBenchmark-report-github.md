``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                          Method |     Mean |     Error |    StdDev |   Median | Scaled | ScaledSD |  Gen 0 | Allocated |
|-------------------------------- |---------:|----------:|----------:|---------:|-------:|---------:|-------:|----------:|
|                  TimeAlgorithmR | 8.106 us | 0.1576 us | 0.1876 us | 8.082 us |   1.00 |     0.00 | 0.1678 |     712 B |
|      TimeAlgorithmRUsingContext | 8.199 us | 0.1634 us | 0.2395 us | 8.245 us |   1.01 |     0.04 | 0.1678 |     712 B |
|             TimeForwardDecaying | 9.511 us | 0.2369 us | 0.6643 us | 9.405 us |   1.17 |     0.09 | 0.1831 |     808 B |
| TimeForwardDecayingUsingContext | 8.947 us | 0.2472 us | 0.7210 us | 8.700 us |   1.10 |     0.09 | 0.1831 |     808 B |
|               TimeSlidingWindow | 8.471 us | 0.1785 us | 0.5092 us | 8.383 us |   1.05 |     0.07 | 0.1678 |     712 B |
|   TimeSlidingWindowUsingContext | 9.193 us | 0.3337 us | 0.9683 us | 8.845 us |   1.13 |     0.12 | 0.1678 |     712 B |
