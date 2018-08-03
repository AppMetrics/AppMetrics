``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|    Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
|---------- |---------:|---------:|---------:|-------:|----------:|
| Decrement | 824.7 ns | 13.73 ns | 11.46 ns | 0.1516 |     640 B |
| Increment | 847.0 ns | 24.25 ns | 23.82 ns | 0.1516 |     640 B |
