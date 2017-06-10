``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10.0.16199
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312785 Hz, Resolution=301.8608 ns, Timer=TSC
dotnet cli version=2.0.0-preview1-005977
  [Host] : .NET Core 4.6.25302.01, 64bit RyuJIT
  Core   : .NET Core 4.6.25302.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |                       Method |     Mean |    Error |   StdDev |   Median |  Gen 0 | Allocated |
 |----------------------------- |---------:|---------:|---------:|---------:|-------:|----------:|
 |     ResolveApdexFromRegistry | 90.02 ns | 1.752 ns | 2.086 ns | 89.11 ns | 0.0457 |     192 B |
 |   ResolveCounterFromRegistry | 93.26 ns | 1.894 ns | 4.157 ns | 92.07 ns | 0.0457 |     192 B |
 |     ResolveGaugeFromRegistry | 91.70 ns | 1.869 ns | 4.103 ns | 89.93 ns | 0.0457 |     192 B |
 | ResolveHistogramFromRegistry | 96.08 ns | 1.960 ns | 4.003 ns | 95.13 ns | 0.0457 |     192 B |
 |     ResolveMeterFromRegistry | 94.19 ns | 1.912 ns | 4.197 ns | 93.23 ns | 0.0457 |     192 B |
 |     ResolveTimerFromRegistry | 90.03 ns | 3.256 ns | 3.198 ns | 89.20 ns | 0.0457 |     192 B |
