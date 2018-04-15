``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.371 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008530
  [Host] : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT
  Core   : .NET Core 2.1.0-preview2-26406-04 (CoreCLR 4.6.26406.07, CoreFX 4.6.26406.04), 64bit RyuJIT

Job=Core  Runtime=Core  

```
| Method |     Mean |    Error |   StdDev | Allocated |
|------- |---------:|---------:|---------:|----------:|
| Update | 120.9 ns | 1.797 ns | 1.681 ns |       0 B |
