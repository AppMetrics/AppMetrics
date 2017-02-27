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
                  TimeAlgorithmR | 6.4479 us | 0.0412 us |
      TimeAlgorithmRUsingContext | 6.5789 us | 0.0537 us |
             TimeForwardDecaying | 6.6163 us | 0.0595 us |
 TimeForwardDecayingUsingContext | 6.5689 us | 0.0514 us |
               TimeSlidingWindow | 6.3412 us | 0.0466 us |
   TimeSlidingWindowUsingContext | 6.3853 us | 0.0474 us |
