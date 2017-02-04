``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=327 B  

```
                          Method |      Mean |    StdDev |
-------------------------------- |---------- |---------- |
                  TimeAlgorithmR | 6.1268 us | 0.0533 us |
      TimeAlgorithmRUsingContext | 6.2322 us | 0.0729 us |
             TimeForwardDecaying | 6.4141 us | 0.1030 us |
 TimeForwardDecayingUsingContext | 6.2310 us | 0.0511 us |
               TimeSlidingWindow | 6.1556 us | 0.1224 us |
   TimeSlidingWindowUsingContext | 6.0575 us | 0.0595 us |
