``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312784 Hz, Resolution=301.8609 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=327 B  

```
                          Method |      Mean |    StdDev | Scaled | Scaled-StdDev |
-------------------------------- |---------- |---------- |------- |-------------- |
                  TimeAlgorithmR | 6.3791 us | 0.0723 us |   1.00 |          0.00 |
      TimeAlgorithmRUsingContext | 6.4238 us | 0.0342 us |   1.01 |          0.01 |
             TimeForwardDecaying | 6.5160 us | 0.0852 us |   1.02 |          0.02 |
 TimeForwardDecayingUsingContext | 6.6087 us | 0.0477 us |   1.04 |          0.01 |
               TimeSlidingWindow | 6.3314 us | 0.0463 us |   0.99 |          0.01 |
   TimeSlidingWindowUsingContext | 6.3609 us | 0.0589 us |   1.00 |          0.01 |
