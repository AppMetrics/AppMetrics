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
-------------------- |-------------- |----------- |------- |---------- |
           Decrement |   197.7471 ns |  1.3114 ns | 0.0484 |     255 B |
 DecrementMetricItem | 1,314.8832 ns | 28.3000 ns | 0.2853 |   1.38 kB |
  DecrementUserValue |   301.1133 ns |  3.2988 ns | 0.0481 |     255 B |
           Increment |   193.6961 ns |  2.7010 ns | 0.0484 |     255 B |
 IncrementMetricItem | 1,273.7235 ns | 32.4396 ns | 0.2843 |   1.38 kB |
  IncrementUserValue |   345.3350 ns |  4.0224 ns | 0.0506 |     255 B |
