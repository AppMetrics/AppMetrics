``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
|---------------------- |-----------:|----------:|----------:|-------:|----------:|
|      UpdateAlgorithmR | 1,095.1 ns | 29.146 ns | 50.275 ns | 0.1583 |     672 B |
| UpdateForwardDecaying | 1,253.6 ns |  7.863 ns |  6.566 ns | 0.1812 |     768 B |
|   UpdateSlidingWindow |   929.2 ns |  9.036 ns |  8.010 ns | 0.1593 |     672 B |
