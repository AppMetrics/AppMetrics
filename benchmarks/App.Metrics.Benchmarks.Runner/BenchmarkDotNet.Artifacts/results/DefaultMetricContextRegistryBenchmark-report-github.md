``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0
  [Host] : .NET Core 4.6.00001.0, 64bit RyuJIT
  Core   : .NET Core 4.6.00001.0, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                       Method |     Mean |     Error |    StdDev |  Gen 0 | Allocated |
 |----------------------------- |---------:|----------:|----------:|-------:|----------:|
 |     ResolveApdexFromRegistry | 85.60 ns | 1.0084 ns | 0.9433 ns | 0.0457 |     192 B |
 |   ResolveCounterFromRegistry | 87.40 ns | 0.9301 ns | 0.8700 ns | 0.0457 |     192 B |
 |     ResolveGaugeFromRegistry | 86.03 ns | 1.7418 ns | 1.6293 ns | 0.0457 |     192 B |
 | ResolveHistogramFromRegistry | 87.99 ns | 0.9447 ns | 0.8836 ns | 0.0457 |     192 B |
 |     ResolveMeterFromRegistry | 86.79 ns | 1.5577 ns | 1.4571 ns | 0.0457 |     192 B |
 |     ResolveTimerFromRegistry | 87.77 ns | 0.6855 ns | 0.6077 ns | 0.0457 |     192 B |
