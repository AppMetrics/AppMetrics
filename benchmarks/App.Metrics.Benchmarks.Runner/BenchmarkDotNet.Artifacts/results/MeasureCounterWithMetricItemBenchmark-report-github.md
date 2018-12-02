``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                              Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
|------------------------------------ |---------:|----------:|----------:|-------:|----------:|
|                 DecrementMetricItem | 1.091 us | 0.0315 us | 0.0922 us | 0.1621 |     688 B |
| DecrementMetricItemWithMultipleTags | 1.697 us | 0.0571 us | 0.1675 us | 0.2480 |    1048 B |
|                 IncrementMetricItem | 1.172 us | 0.0404 us | 0.1192 us | 0.1621 |     688 B |
| IncrementMetricItemWithMultipleTags | 2.031 us | 0.0865 us | 0.2550 us | 0.2480 |    1048 B |
