``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  Core   : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                       Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
|----------------------------- |---------:|----------:|----------:|-------:|----------:|
|     ResolveApdexFromRegistry | 1.952 us | 0.0284 us | 0.0237 us | 0.2327 |     984 B |
|   ResolveCounterFromRegistry | 1.947 us | 0.0181 us | 0.0160 us | 0.2327 |     984 B |
|     ResolveGaugeFromRegistry | 1.588 us | 0.0124 us | 0.0104 us | 0.2327 |     984 B |
| ResolveHistogramFromRegistry | 1.935 us | 0.0165 us | 0.0154 us | 0.2327 |     992 B |
|     ResolveMeterFromRegistry | 1.620 us | 0.0163 us | 0.0145 us | 0.2327 |     984 B |
|     ResolveTimerFromRegistry | 1.615 us | 0.0137 us | 0.0115 us | 0.2327 |     984 B |
