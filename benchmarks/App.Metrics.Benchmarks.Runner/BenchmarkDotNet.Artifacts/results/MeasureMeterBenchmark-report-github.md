``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
         Method |          Mean |     StdDev |  Gen 0 | Allocated |
--------------- |-------------- |----------- |------- |---------- |
           Mark |   193.7937 ns |  0.8007 ns | 0.0482 |     255 B |
 MarkMetricItem | 1,261.6435 ns | 15.8472 ns | 0.2853 |   1.38 kB |
  MarkUserValue |   309.2564 ns |  5.5220 ns | 0.0490 |     255 B |
