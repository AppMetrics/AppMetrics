``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
              Method |        Mean |     StdDev |  Gen 0 | Allocated |
-------------------- |------------ |----------- |------- |---------- |
           Decrement | 203.1457 ns |  2.7116 ns | 0.0486 |     255 B |
 DecrementMetricItem | 561.3463 ns | 40.4305 ns | 0.0609 |     366 B |
  DecrementUserValue | 319.7285 ns |  2.2260 ns | 0.0484 |     255 B |
           Increment | 207.4588 ns |  2.1375 ns | 0.0476 |     255 B |
 IncrementMetricItem | 523.2124 ns |  7.4678 ns | 0.0629 |     366 B |
  IncrementUserValue | 319.2846 ns |  4.7603 ns | 0.0484 |     255 B |
 |     255 B |
