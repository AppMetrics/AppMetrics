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
                  TimeAlgorithmR | 6.5453 us | 0.0415 us |
      TimeAlgorithmRUsingContext | 6.5484 us | 0.0656 us |
             TimeForwardDecaying | 6.7652 us | 0.1139 us |
 TimeForwardDecayingUsingContext | 6.9712 us | 0.1792 us |
               TimeSlidingWindow | 6.4857 us | 0.0561 us |
   TimeSlidingWindowUsingContext | 6.4780 us | 0.0527 us |
