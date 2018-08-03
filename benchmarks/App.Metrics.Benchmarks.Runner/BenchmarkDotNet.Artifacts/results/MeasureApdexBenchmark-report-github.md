``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                                Method |     Mean |     Error |    StdDev |   Median |  Gen 0 | Allocated |
|-------------------------------------- |---------:|----------:|----------:|---------:|-------:|----------:|
|                        TimeAlgorithmR | 8.543 us | 0.1894 us | 0.5464 us | 8.469 us | 0.1678 |     712 B |
|      TrackUsingAlgorithmRUsingContext | 8.515 us | 0.1692 us | 0.4604 us | 8.373 us | 0.1678 |     712 B |
|             TrackUsingForwardDecaying | 8.798 us | 0.1791 us | 0.5281 us | 8.702 us | 0.1984 |     872 B |
| TrackUsingForwardDecayingUsingContext | 8.921 us | 0.1774 us | 0.5202 us | 8.933 us | 0.1831 |     808 B |
|               TrackUsingSlidingWindow | 8.439 us | 0.1685 us | 0.4319 us | 8.412 us | 0.1678 |     712 B |
|   TrackUsingSlidingWindowUsingContext | 8.489 us | 0.1693 us | 0.4691 us | 8.433 us | 0.1678 |     712 B |
