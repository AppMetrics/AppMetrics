``` ini

BenchmarkDotNet=v0.10.8, OS=Windows 10.0.16215
Processor=Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
dotnet cli version=2.0.0-preview1-005977
  [Host] : .NET Core 4.6.25302.01, 64bit RyuJIT
  Core   : .NET Core 4.6.25302.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 |     Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
 |----------- |---------:|---------:|---------:|-------:|----------:|
 | ReadHealth | 43.33 us | 1.319 us | 3.888 us | 1.5564 |   6.48 KB |
