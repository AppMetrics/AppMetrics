``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
              Method |        Mean |    StdDev |  Gen 0 | Allocated |
-------------------- |------------ |---------- |------- |---------- |
           Decrement | 200.0111 ns | 1.5733 ns | 0.0481 |     255 B |
 DecrementMetricItem | 408.6757 ns | 5.1452 ns | 0.0635 |     366 B |
  DecrementUserValue | 312.3322 ns | 9.4250 ns | 0.0476 |     255 B |
           Increment | 208.3164 ns | 5.3108 ns | 0.0474 |     255 B |
 IncrementMetricItem | 415.5476 ns | 6.3695 ns | 0.0599 |     366 B |
  IncrementUserValue | 306.8503 ns | 2.8819 ns | 0.0484 |     255 B |
0614 |     366 B |
  IncrementUserValue | 324.3867 ns | 3.1953 ns | 12.3754 ns | 0.0481 |     255 B |
UserValue | 316.2327 ns | 2.3623 ns | 12.7211 ns | 0.0474 |     255 B |
ns | 140.2562 ns | 378.8982 ns | 0.0479 |     255 B |
