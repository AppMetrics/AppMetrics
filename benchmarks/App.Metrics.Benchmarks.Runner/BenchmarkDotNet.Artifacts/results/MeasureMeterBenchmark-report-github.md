``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                         Method |       Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------------------- |-----------:|----------:|----------:|-------:|----------:|
|                           Mark |   806.7 ns |  4.304 ns |  3.815 ns | 0.1526 |     640 B |
|                 MarkMetricItem | 1,032.3 ns | 20.663 ns | 28.284 ns | 0.1621 |     688 B |
| MarkMetricItemWithMultipleTags | 1,427.4 ns | 11.398 ns | 10.661 ns | 0.2480 |    1048 B |
|                  MarkUserValue |   866.1 ns |  6.293 ns |  5.887 ns | 0.1516 |     640 B |
